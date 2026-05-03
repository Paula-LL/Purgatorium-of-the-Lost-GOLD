using System;
using System.Threading.Tasks;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;
public class MongoDBManager : MonoBehaviour
{
    public static MongoDBManager Instancia { get; private set; }

    [Header("MongoDB Atlas Config")]
    [Tooltip("Connection string de MongoDB Atlas (mongodb+srv://...). NO publicar al repositori!")]
    [SerializeField] private string connectionString =
        "mongodb+srv://<usuari>:<contrasenya>@cluster0.xxxxx.mongodb.net/?retryWrites=true&w=majority";

    [Tooltip("Nom de la base de dades")]
    [SerializeField] private string nomBaseDeDades = "PurgatoriumDB";

    [Tooltip("Col·lecció on es guarden les sessions de joc")]
    [SerializeField] private string colSessions = "sessions_joc";

    [Tooltip("Col·lecció on es guarden les estadístiques agregades per jugador")]
    [SerializeField] private string colEstadistiques = "estadistiques_jugador";

    private MongoClient    _client;
    private IMongoDatabase _db;
    private IMongoCollection<BsonDocument> _colSessions;
    private IMongoCollection<BsonDocument> _colEstats;

    private bool _connectat = false;

    private string _playerId = "anonymous";

    private DateTime _iniciSessio;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);

        _playerId = PlayerPrefs.GetString("PlayerID", GenerarPlayerId());
        PlayerPrefs.SetString("PlayerID", _playerId);

        _iniciSessio = DateTime.UtcNow;
        ConnectarAtlas();
    }

    private void OnApplicationQuit()
    {
        // Enviar la sessió completa en tancar el joc
        _ = EnviarSessioAsync();
    }

    private void ConnectarAtlas()
    {
        try
        {
            _client = new MongoClient(connectionString);
            _db     = _client.GetDatabase(nomBaseDeDades);
            _colSessions = _db.GetCollection<BsonDocument>(colSessions);
            _colEstats   = _db.GetCollection<BsonDocument>(colEstadistiques);
            _connectat   = true;
            Debug.Log("[MongoDB] Connexió establerta amb MongoDB Atlas.");
        }
        catch (Exception ex)
        {
            _connectat = false;
            Debug.LogWarning($"[MongoDB] No s'ha pogut connectar: {ex.Message}");
        }
    }

    public async Task EnviarSessioAsync()
    {
        if (!_connectat || _colSessions == null)
        {
            Debug.LogWarning("[MongoDB] No connectat. La sessió no s'ha enviat.");
            return;
        }

        EstadisticasJuego s = EstadisticasJuego.Instancia;
        if (s == null)
        {
            Debug.LogWarning("[MongoDB] EstadisticasJuego no disponible.");
            return;
        }

        try
        {
            var sessio = new BsonDocument
            {
                { "playerId",        _playerId },
                { "iniciSessio",     _iniciSessio },
                { "fiSessio",        DateTime.UtcNow },
                { "duradaSessioSeg", (DateTime.UtcNow - _iniciSessio).TotalSeconds },
                { "plataforma",      Application.platform.ToString() },
                { "versioJoc",       Application.version },
                {
                    "estadistiques", new BsonDocument
                    {
                        { "partidesJugades",  s.PartidasJugadas },
                        { "victorias",        s.Victorias },
                        { "enemicsElimitats", s.EnemigosMatados },
                        { "bossesDerrots",    s.BossesMatados },
                        { "morts",            s.MuerteJugador },
                        { "danyHet",          s.DanoHecho },
                        { "danyRebut",        s.DanoRecibido },
                        { "cartesRecollides", s.CartasRecogidas },
                        { "tempsTotal",       s.TiempoTotalJugado }
                    }
                }
            };

            await _colSessions.InsertOneAsync(sessio);
            Debug.Log("[MongoDB] Sessió enviada correctament. El Trigger s'executarà al servidor.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[MongoDB] Error en enviar la sessió: {ex.Message}");
        }
    }

    public void EnviarSessio()
    {
        _ = EnviarSessioAsync();
    }

    public async Task<BsonDocument> ObtenirEstatsGlobalsAsync()
    {
        if (!_connectat || _colEstats == null) return null;

        try
        {
            var filtre = Builders<BsonDocument>.Filter.Eq("playerId", _playerId);
            return await _colEstats.Find(filtre).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[MongoDB] Error en obtenir estadístiques: {ex.Message}");
            return null;
        }
    }

    private string GenerarPlayerId()
    {
        return "player_" + SystemInfo.deviceUniqueIdentifier.Substring(0, 8);
    }

    public bool EstaConnectat() => _connectat;
    public string GetPlayerId() => _playerId;
}

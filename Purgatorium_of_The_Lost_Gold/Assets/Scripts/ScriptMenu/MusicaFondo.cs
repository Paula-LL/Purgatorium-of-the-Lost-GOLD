using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    public static MusicaFondo Instance { get; private set; }

    void Awake()
    {
        // Si ya existe una instancia y no es esta, destruye esta
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Establece esta como la instancia única
        Instance = this;

        // Hace que persista entre escenas
        DontDestroyOnLoad(gameObject);
    }
}
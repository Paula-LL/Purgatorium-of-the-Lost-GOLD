using UnityEngine;
using UnityEngine.SceneManagement;

public class CondiciondeVictoria : MonoBehaviour
{
    [Header("Configuración de Victoria")]
    [Tooltip("Nombre de la escena a cargar cuando el boss sea destruido")]
    public string sceneName;

    [Tooltip("Tag del objeto Boss que debe ser destruido")]
    public string bossTag = "Boss";

    private GameObject bossObject;
    private bool bossDestruido = false;

    void Start()
    {
        // Buscar el objeto con el tag de Boss al inicio
        bossObject = GameObject.FindGameObjectWithTag(bossTag);

        if (bossObject == null)
        {
            Debug.LogWarning($"No se encontró ningún objeto con el tag '{bossTag}' en la escena.");
        }
        else
        {
            Debug.Log($"Boss encontrado: {bossObject.name}");
        }
    }

    void Update()
    {
        // Verificar si el boss ha sido destruido
        if (!bossDestruido && bossObject == null)
        {
            // Buscar nuevamente por si acaso el objeto fue destruido pero la referencia no se actualizó
            GameObject boss = GameObject.FindGameObjectWithTag(bossTag);

            if (boss == null)
            {
                BossDestruido();
            }
            else
            {
                bossObject = boss;
            }
        }
    }

    void BossDestruido()
    {
        bossDestruido = true;
        EstadisticasJuego.RegistrarVictoria();
        Debug.Log($"¡El boss con tag '{bossTag}' ha sido destruido! Cargando escena: {sceneName}");
        
        // Cambiar a la escena indicada
        if (!string.IsNullOrEmpty(sceneName))
        {
           
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No se ha especificado un nombre de escena en CondiciondeVictoria");
        }
    }

    // Método opcional para suscribirse a eventos del Boss si tiene un sistema de salud
    public void SuscribirABossHealth(BossHealth bossHealth)
    {
        if (bossHealth != null)
        {
            // Aquí podrías suscribirte a un evento personalizado del Boss
            // Por ejemplo: bossHealth.OnBossDied += BossDestruido;
            Debug.Log("Suscrito al sistema de salud del boss");
        }
    }
}
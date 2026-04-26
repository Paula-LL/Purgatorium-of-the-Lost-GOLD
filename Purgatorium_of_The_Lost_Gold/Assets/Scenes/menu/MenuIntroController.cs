using UnityEngine;
using UnityEngine.UI;

public class MenuIntroController : MonoBehaviour
{
    [Header("Luces")]
    public Light[] lights;
    public Light luzGrande;
    public float maxIntensity = 40f;

    [Header("Partículas (velas)")]
    public ParticleSystem[] candleParticles;

    [Header("UI Inicio")]
    public CanvasGroup introCanvas;

    [Header("Duración")]
    public float duration = 0f;
    public float durationtitle = 0f;

    float time = 0f;
    bool started = false;

    void Start()
    {
        // Asegurarse de que todo empieza apagado
        foreach (Light l in lights)
            l.intensity = 0;

        foreach (ParticleSystem p in candleParticles)
            p.Stop();
    }

    void Update()
    {
        if (!started) return;

        time += Time.deltaTime;
        float t = time / duration;
        float ttitle = time / durationtitle;

        // Fade OUT del texto
        introCanvas.alpha = Mathf.Lerp(1, 0, ttitle);

        // Encender luces
        foreach (Light l in lights)
        {
            l.intensity = Mathf.Lerp(0, maxIntensity, t);
        }

        luzGrande.intensity = Mathf.Lerp(0, 800, t);
    }

    public void StartIntro()
    {
        started = true;

        foreach (ParticleSystem p in candleParticles)
        {
            p.Play();
        }
    }
}

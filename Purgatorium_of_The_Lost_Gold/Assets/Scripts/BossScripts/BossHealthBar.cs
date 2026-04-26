using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Image      barraRelleno;
    [SerializeField] private TMP_Text   textoVida;

    void Update()
    {
        if (bossHealth == null) return;

        float porcentaje = bossHealth.VidaMaxima > 0f
            ? bossHealth.VidaActual / bossHealth.VidaMaxima
            : 0f;

        if (barraRelleno != null)
            barraRelleno.fillAmount = porcentaje;

        if (textoVida != null)
            textoVida.text = $"{Mathf.CeilToInt(bossHealth.VidaActual)} / {Mathf.CeilToInt(bossHealth.VidaMaxima)}";
    }
}

using UnityEngine;

/// <summary>
/// Adjuntar al mismo GameObject que el colliderDanio de Ataque4.
/// Delega los eventos de trigger al script Ataque4 del boss.
/// </summary>
public class TriggerReporterAtaque4 : MonoBehaviour
{
    public Ataque4 ataque4;

    void OnTriggerEnter(Collider other)
    {
        if (ataque4 != null) ataque4.RecibirColision(other);
    }
}

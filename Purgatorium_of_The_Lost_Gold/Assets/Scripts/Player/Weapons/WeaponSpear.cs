using UnityEngine;

public class WeaponSpear : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    private PlayerAttack playerAttack;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerAttack = GetComponentInParent<PlayerAttack>();

        if (playerAttack == null)
            Debug.LogError("WeaponSpear: PlayerAttack NOT FOUND in parent!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemigoDist dist = other.GetComponent<EnemigoDist>();
            EnemigoBase baseEnemy = other.GetComponent<EnemigoBase>();

            if (dist != null)
                dist.TakeDamage(playerAttack.currentAttackDamage);
            else if (baseEnemy != null)
                baseEnemy.TakeDamage(playerAttack.currentAttackDamage);

            EstadisticasJuego.RegistrarDanoHecho(playerAttack.currentAttackDamage);
        }

        if (other.CompareTag("Boss"))
        {
            BossHealth boss = other.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.RecibirDanio(playerAttack.currentAttackDamage);
                EstadisticasJuego.RegistrarDanoHecho(playerAttack.currentAttackDamage);
            }
        }
    }

    public void EnableTriggerCapsule()
    {
        capsuleCollider.enabled = true;
    }

    public void DisableTriggerCapsule()
    {
        capsuleCollider.enabled = false;
    }
}

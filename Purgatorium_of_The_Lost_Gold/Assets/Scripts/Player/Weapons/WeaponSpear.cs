using UnityEngine;

public class WeaponSpear : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    private PlayerAttack playerAttack;
    [SerializeField] private ParticleSystem spearGlow;

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
            {
                spearGlow.Play();
                dist.TakeDamage(playerAttack.currentAttackDamage);
                ShowDamage(playerAttack.currentAttackDamage);
            } 
            else if (baseEnemy != null)
            {
                spearGlow.Play();
                baseEnemy.TakeDamage(playerAttack.currentAttackDamage);
                ShowDamage(playerAttack.currentAttackDamage);
            }
               

            EstadisticasJuego.RegistrarDanoHecho(playerAttack.currentAttackDamage);
        }

        if (other.CompareTag("Boss"))
        {
            BossHealth boss = other.GetComponent<BossHealth>();
            if (boss != null)
            {
                spearGlow.Play();   
                boss.RecibirDanio(playerAttack.currentAttackDamage);
                EstadisticasJuego.RegistrarDanoHecho(playerAttack.currentAttackDamage);
            }
        }
       
    }
    public void ShowDamage(float amount)
    {
        SpawnDamagePopups.Instance.DamageDone(amount, transform.position, false);
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

using System;

[Serializable]
public class PlayerStats
{
    public float baseHealth;
    public float maxHealth;
    public float currentHealth;

    public float baseDamage;
    public float attackDamage;

    public float defense;

    public float critChance;
    public float critMultiplier;

    public float attackSpeed;

    public float attackRadius;
    public float attackDistance;
    public float attackDuration;

    public Movement movement;

    public PlayerStats()
    {
        maxHealth = 5;
        baseHealth = maxHealth;
        currentHealth = maxHealth;

        baseDamage = 1;
        attackDamage = baseDamage;

        defense = 0;

        critChance = 0.1f;
        critMultiplier = 2f;

        attackSpeed = 1f;

        attackRadius = 0.4f;
        attackDistance = 1.2f;
        attackDuration = 0.2f;

        movement = new Movement();
    }

    [Serializable]
    public class Movement
    {
        public float moveSpeed = 5f;
        public float dashSpeed = 20f;
        public float dashDuration = 0.1f;
        public float rotationSpeed = 10f;

        public Movement() { }

        public Movement(Movement movement)
        {
            moveSpeed = movement.moveSpeed;
            dashSpeed = movement.dashSpeed;
            dashDuration = movement.dashDuration;
            rotationSpeed = movement.rotationSpeed;
        }
    }
}
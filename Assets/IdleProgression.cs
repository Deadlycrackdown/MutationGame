using UnityEngine;

public class IdleProgression : MonoBehaviour
{
    public MutationNest mutationNest;
    public float idleAttackInterval = 5f;
    public float attackRadius = 5f;

    private float idleTimer = 0f;

    private void Update()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleAttackInterval)
        {
            AttackNearbyAnimals();
            idleTimer = 0f;
        }
    }

    private void AttackNearbyAnimals()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, attackRadius);

        foreach (Collider collider in nearbyColliders)
        {
            Animal animal = collider.GetComponent<Animal>();
            if (animal != null)
            {
                int attackPower = CalculateAttackPower();
                animal.TakeDamage(attackPower);
                mutationNest.OnAnimalClicked(animal); // Call OnAnimalClicked method in MutationNest
            }
        }
    }

    private int CalculateAttackPower()
    {
        // Implement the logic to calculate the attack power based on the nest's upgrades or other factors
        // Return the calculated attack power
        return mutationNest.attackPower;
    }
}

using UnityEngine;

public class TentacleBall : MonoBehaviour
{
    public Animal associatedAnimal;
    public float speed = 5f;
    public float tentacleRemovalInterval = 0.5f;

    private MutationNest mutationNest;
    private float timeSinceLastRemoval = 0f;
    private bool isFlying = false; // Flag to indicate if the ball is flying

    private void Start()
    {
        mutationNest = FindObjectOfType<MutationNest>();
    }

    private void Update()
    {
        if (isFlying)
        {
            if (associatedAnimal == null || !associatedAnimal.gameObject.activeSelf)
            {
                Destroy(gameObject); // Destroy the tentacle ball if the associated animal becomes missing or inactive
                return;
            }

            Vector3 targetPosition = associatedAnimal.transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Remove tentacles at intervals along the path
            timeSinceLastRemoval += Time.deltaTime;
            if (timeSinceLastRemoval >= tentacleRemovalInterval)
            {
                RemoveTentacle();
                timeSinceLastRemoval = 0f;
            }

            // Destroy the tentacle ball when it reaches the animal
            if (Vector3.Distance(transform.position, targetPosition) < tentacleRemovalInterval)
            {
                Destroy(gameObject);
            }
        }
    }

    private void RemoveTentacle()
    {
        if (mutationNest != null)
        {
            // Remove the last tentacle attached to the nest
            mutationNest.RemoveLastTentacle();
        }
    }

    // Method to start flying the tentacle ball
    public void StartFlying()
    {
        isFlying = true;
    }
}

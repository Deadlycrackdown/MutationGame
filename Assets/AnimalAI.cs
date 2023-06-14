using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Transform player;
    private Vector3 initialMovementDirection;

    private void Start()
    {
        // Find the player object or assign it through some other means
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Calculate the initial movement direction based on the player's position
        Vector3 targetDirection = (player.position - transform.position).normalized;
        float playerSide = Mathf.Sign(Vector3.Dot(targetDirection, transform.right));
        initialMovementDirection = transform.forward + transform.right * playerSide;
    }

    private void Update()
    {
        // Move the animal in the initial movement direction
        transform.position += initialMovementDirection * moveSpeed * Time.deltaTime;

        // Rotate the animal to face the initial movement direction
        transform.rotation = Quaternion.LookRotation(initialMovementDirection);
    }
}

using UnityEngine;

public class DestroyOutOfCamera : MonoBehaviour
{
    private Renderer objectRenderer;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnBecameInvisible()
    {
        // Destroy the game object when it becomes invisible by going out of the camera's view
        Destroy(gameObject);
    }
}

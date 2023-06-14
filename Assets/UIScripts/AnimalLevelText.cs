using UnityEngine;

public class AnimalLevelText : MonoBehaviour
{
    public int fontSize = 24;
    public Vector3 offset = new Vector3(0f, 1.5f, 0f); // Offset from the animal's position

    private GameObject levelTextGO;
    private TextMesh textMesh;
    private Animal animal;

    private void Start()
    {
        // Create a new GameObject for the text
        levelTextGO = new GameObject("LevelText");
        levelTextGO.transform.SetParent(transform); // Set the animal as the parent

        // Add a TextMesh component to the new GameObject
        textMesh = levelTextGO.AddComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = fontSize;
        textMesh.characterSize = 0.1f; // Adjust the character size for better clarity

        // Set the initial level text
        animal = GetComponent<Animal>();
        textMesh.text = "Level: " + animal.level.ToString();

        // Set the text position based on the offset
        levelTextGO.transform.localPosition = offset;
    }

    private void Update()
    {
        // Update the text position if the animal moves
        if (animal != null)
        {
            levelTextGO.transform.position = transform.position + offset;
        }
    }
}

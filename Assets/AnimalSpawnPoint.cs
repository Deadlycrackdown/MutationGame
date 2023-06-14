using UnityEngine;

public class AnimalSpawnPoint : MonoBehaviour
{
    public GameObject animalPrefab;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 5f;

    private MutationNest mutationNest;

    private void Start()
    {
        mutationNest = FindObjectOfType<MutationNest>();
        StartSpawning();
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnAnimalsCoroutine());
    }

    private System.Collections.IEnumerator SpawnAnimalsCoroutine()
    {
        while (true)
        {
            SpawnAnimal();
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnAnimal()
    {
        GameObject animalObject = Instantiate(animalPrefab, transform.position, Quaternion.identity);
        Animal animal = animalObject.GetComponent<Animal>();
        if (animal != null && mutationNest != null)
        {
            int nestEvolutionStage = mutationNest.currentEvolutionStage;
            int levelVariation = Random.Range(-nestEvolutionStage, nestEvolutionStage + 1);
            int animalLevel = Mathf.Clamp(nestEvolutionStage + levelVariation, 1, mutationNest.maxEvolutionStages);
            animal.SetLevel(animalLevel);
        }
    }
}

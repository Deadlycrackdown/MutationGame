using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MutationNest : MonoBehaviour
{
    public int attackPower = 1;
    public int experiencePoints = 0;
    public int requiredExperienceForUpgrade = 10;
    public int currentEvolutionStage = 1;
    public int mutationPointsPerEvolution = 1;
    public int maxEvolutionStages = 5;
    public int mutationPoints = 0;

    private Coroutine pullCoroutine;
    private int remainingPulls = 0; // Counter for simultaneous pulls
    private bool isCooldownActive = false; // Flag for pull cooldown

    public int simultaneousTentacles = 1;
    public int maxPullsWithoutCooldown = 2; // Maximum pulls without cooldown
    public float enemyPullTime = 2f;
    public float tentacleRange = 5f;

    public bool hasSpecialAbilities = false;

    private List<Animal> attractedAnimals = new List<Animal>();
    private List<GameObject> tentacles = new List<GameObject>(); // List to store the spawned tentacles

    public GameObject tentacleBallPrefab; // Prefab for the tentacle ball
    public float tentacleBallRemovalDistance = 2f; // Distance threshold to remove tentacle balls

    public float maxTentacleBallSize = 0.25f; // min size of the tentacle balls at end
    public float minTentacleBallSize = 1f; // max size of the tentacle balls at start

    public delegate void MutationPointsUpdated();
    public static event MutationPointsUpdated OnMutationPointsUpdated;

    private void Start()
    {
        AdjustTentacleBallSize();
    }

    public void OnAnimalClicked(Animal animal)
    {
        int damageDealt = attackPower;
        float pullTime = CalculatePullTime(animal.level);

        if (remainingPulls > 0)
        {
            if (pullCoroutine == null)
            {
                pullCoroutine = StartCoroutine(PullAnimalCoroutine(animal, pullTime));
                remainingPulls--;
            }
        }
        else if (!isCooldownActive)
        {
            if (pullCoroutine == null)
            {
                pullCoroutine = StartCoroutine(PullAnimalCoroutine(animal, pullTime));
                StartCoroutine(StartPullCooldown());
            }
        }
    }

    public void RemoveLastTentacle()
    {
        if (tentacles.Count > 0)
        {
            GameObject lastTentacle = tentacles[tentacles.Count - 1];
            tentacles.Remove(lastTentacle);
            Destroy(lastTentacle);
        }
    }

    private IEnumerator PullAnimalCoroutine(Animal animal, float pullTime)
    {
        attractedAnimals.Add(animal);
        animal.StartAttraction(transform); // Pass the nestTransform as an argument

        List<Vector3> path = CalculatePathToNest(animal);
        SpawnTentacleBalls(path);

        yield return new WaitForSeconds(pullTime);

        animal.StopAttraction();

        int damageDealt = attackPower;
        animal.TakeDamage(damageDealt);
        experiencePoints += damageDealt;

        attractedAnimals.Remove(animal);

        if (animal.health <= 0)
        {
            Destroy(animal.gameObject);
        }

        if (experiencePoints >= requiredExperienceForUpgrade)
        {
            UpgradeNest();
        }

        pullCoroutine = null;
    }

    private float CalculatePullTime(int animalLevel)
    {
        // Adjust pull time based on the level difference between the nest and the animal
        float adjustedPullTime = enemyPullTime;

        if (animalLevel > currentEvolutionStage)
        {
            int levelDifference = animalLevel - currentEvolutionStage;
            adjustedPullTime *= Mathf.Pow(2f, levelDifference);
        }

        return adjustedPullTime;
    }

    private IEnumerator StartPullCooldown()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(enemyPullTime);
        remainingPulls = maxPullsWithoutCooldown;
        isCooldownActive = false;
    }

    private void UpgradeNest()
    {
        if (currentEvolutionStage < maxEvolutionStages)
        {
            currentEvolutionStage++;
            experiencePoints = 0;
            mutationPoints += mutationPointsPerEvolution;
            // Update nest appearance based on the evolution stage
        }
        else
        {
            mutationPoints += mutationPointsPerEvolution;
        }

        // Trigger the mutation points update event
        OnMutationPointsUpdated?.Invoke();
    }

    public void UpgradeSimultaneousTentacles(int newTentaclesCount)
    {
        simultaneousTentacles = newTentaclesCount;
        remainingPulls = simultaneousTentacles;
    }

    public void UpgradeEnemyPullTime(float newPullTime)
    {
        enemyPullTime = newPullTime;
    }

    public void UpgradeTentacleRange(float newRange)
    {
        tentacleRange = newRange;
    }

    public void UpgradeSpecialAbilities(bool enableAbilities)
    {
        hasSpecialAbilities = enableAbilities;
    }

    public void ActivateSpecialAbilities()
    {
        if (!hasSpecialAbilities)
            return;

        // Pull all animals within range to the nest
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, tentacleRange);
        foreach (Collider collider in nearbyColliders)
        {
            Animal animal = collider.GetComponent<Animal>();
            if (animal != null && !attractedAnimals.Contains(animal))
            {
                float pullTime = CalculatePullTime(animal.level);

                attractedAnimals.Add(animal);
                animal.StartAttraction(transform); // Pass the nestTransform as an argument

                List<Vector3> path = CalculatePathToNest(animal);
                SpawnTentacleBalls(path);

                StartCoroutine(PullAnimalCoroutine(animal, pullTime));
            }
        }

        // Disable special abilities after activation
        hasSpecialAbilities = false;
    }

    private void SpawnTentacleBalls(List<Vector3> path)
    {
        int ballsToSpawn = Mathf.Min(simultaneousTentacles, path.Count);

        for (int i = 0; i < ballsToSpawn; i++)
        {
            Vector3 ballPosition = path[i];
            GameObject tentacleBallObj = Instantiate(tentacleBallPrefab, ballPosition, Quaternion.identity);
            TentacleBall tentacleBall = tentacleBallObj.GetComponent<TentacleBall>();
            tentacleBall.associatedAnimal = attractedAnimals[attractedAnimals.Count - 1];

            // Set the size of the tentacle ball
            float size = CalculateTentacleBallSize(i, ballsToSpawn);
            tentacleBall.transform.localScale = new Vector3(size, size, size);

            // Start flying the tentacle ball
            tentacleBall.StartFlying();
        }
    }

    private float CalculateTentacleBallSize(int index, int totalBalls)
    {
        // Calculate the size of the tentacle ball based on the index and total number of balls
        float size = maxTentacleBallSize - ((maxTentacleBallSize - minTentacleBallSize) * index / (totalBalls - 1));
        return size;
    }

    private List<Vector3> CalculatePathToNest(Animal animal)
    {
        List<Vector3> path = new List<Vector3>();

        // Calculate the path from the animal to the nest
        Vector3 direction = (transform.position - animal.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, animal.transform.position);
        float stepSize = distance / simultaneousTentacles;

        for (int i = 0; i < simultaneousTentacles; i++)
        {
            Vector3 position = animal.transform.position + direction * (stepSize * i);
            path.Add(position);
        }

        return path;
    }

    private void AdjustTentacleBallSize()
    {
        // Adjust the size of the tentacle balls based on the number of simultaneous tentacles
        float size = maxTentacleBallSize - ((maxTentacleBallSize - minTentacleBallSize) * (simultaneousTentacles - 1) / (maxEvolutionStages - 1));
        tentacleBallPrefab.transform.localScale = new Vector3(size, size, size);
    }
}
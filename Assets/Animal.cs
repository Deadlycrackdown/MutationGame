using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Animal : MonoBehaviour
{
    public int health = 10;
    public int level = 1;
    private bool isBeingAbsorbed = false;
    private bool isAttracted = false;
    private Transform nestTransform;
    private float attractionSpeed = 5f;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isBeingAbsorbed)
        {
            isBeingAbsorbed = true;
            StartCoroutine(AbsorbAnimalCoroutine());
        }
    }

    private IEnumerator AbsorbAnimalCoroutine()
    {
        // Perform any necessary visual effects or animations for the absorption

        yield return new WaitForSeconds(2f); // Placeholder value, adjust as needed

        // Remove the animal from the scene
        Destroy(gameObject);
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        // Additional logic to handle any changes associated with the new level
    }

    public void StartAttraction(Transform nestTransform)
    {
        this.nestTransform = nestTransform;
        isAttracted = true;
    }

    public void StopAttraction()
    {
        isAttracted = false;
    }

    private void Update()
    {
        if (isAttracted && nestTransform != null)
        {
            Vector2 direction = nestTransform.position - transform.position;
            direction.Normalize();

            transform.position += (Vector3)direction * attractionSpeed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
           // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnMouseDown()
    {
        if (!isAttracted && !isBeingAbsorbed)
        {
            MutationNest mutationNest = FindObjectOfType<MutationNest>();
            if (mutationNest != null)
            {
                mutationNest.OnAnimalClicked(this);
            }
        }
    }
}

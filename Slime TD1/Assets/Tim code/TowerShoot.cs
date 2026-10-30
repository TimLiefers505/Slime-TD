
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    [Header("Tower Stats")]
    public float range = 10f;
    public float fireRate = 1f;
    public float damage = 20f;
    public float rotationSpeed = 5f;

    private float nextFireTime;

    void Update()
    {
        // Zoek alle enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Zoek een enemy binnen bereik
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance <= range)
            {
                // Richting de enemy
                Vector3 direction = enemy.transform.position - transform.position;

                // Zorg dat de tower niet omhoog of omlaag draait
                direction.y = 0;

                // Draai de tower naar de enemy
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }

                // Schiet op de enemy
                if (Time.time >= nextFireTime)
                {
                    EnemyHealth health = enemy.GetComponent<EnemyHealth>();

                    if (health != null)
                    {
                        health.TakeDamage(damage);
                    }

                    nextFireTime = Time.time + fireRate;
                }

                // Stop met zoeken als we een enemy hebben gevonden
                break;
            }
        }
    }
}


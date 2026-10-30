
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public float range = 10f;
    public float fireRate = 1f;
    public float damage = 20f;

    private float nextFireTime;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Enemy is dichtbij genoeg
            if (distance <= range)
            {
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

                break;
            }
        }
    }
}

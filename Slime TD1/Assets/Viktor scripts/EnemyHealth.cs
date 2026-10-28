
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public int damageToPlayer = 1;
    public int reward = 10;

    private float currentHealth;
    private WaveManager waveManager;
    private bool resolved = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Initialize(WaveManager manager, int wave)
    {
        waveManager = manager;

        // Enemies krijgen meer HP naarmate de waves stijgen.
        maxHealth *= 1f + (wave - 1) * 0.15f;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (resolved)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (resolved)
            return;

        resolved = true;

        if (waveManager != null)
            waveManager.EnemyDefeated(this);

        Destroy(gameObject);
    }

    public void ReachEnd()
    {
        if (resolved)
            return;

        resolved = true;

        if (waveManager != null)
            waveManager.EnemyReachedEnd(this);

        Destroy(gameObject);
    }
}

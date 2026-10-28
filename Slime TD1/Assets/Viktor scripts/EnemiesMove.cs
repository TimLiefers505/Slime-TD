
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public Transform[] waypoints;

    private int currentWaypoint = 0;
    private bool isMoving = true;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (!isMoving || waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 direction = target.position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                isMoving = false;

                if (enemyHealth != null)
                    enemyHealth.ReachEnd();
                else
                    Destroy(gameObject);
            }
        }
    }

    public void SetWaypoints(Transform[] points)
    {
        waypoints = points;
        currentWaypoint = 0;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}

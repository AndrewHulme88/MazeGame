using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pathUpdateInterval = 0.2f;
    [SerializeField] private float chaseDistance = 7f;
    [SerializeField] private float wanderRadius = 8f;
    [SerializeField] private float wanderInterval = 1.5f;
    [SerializeField] private int maxWanderAttempts = 20;
    [SerializeField] private Animator animator;

    private List<Vector3Int> currentPath;
    private int pathIndex;
    private float repathTimer;
    private float wanderTimer;
    private Vector3Int currentWanderTarget;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (MazeGrid.Instance == null)
        {
            enabled = false;
            return;
        }

        Vector3Int startCell = MazeGrid.Instance.WorldToCell(transform.position);

        transform.position = MazeGrid.Instance.CellToWorldCenter(startCell);
    }

    private void Update()
    {
        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0f)
        {
            Repath();
            repathTimer = pathUpdateInterval;
        }

        MoveAlongPath();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 moveDir = GetCurrentMoveDirection();
        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);
    }

    private Vector2 GetCurrentMoveDirection()
    {
        if(currentPath == null || pathIndex >= currentPath.Count)
            return Vector2.down;

        Vector3 currentTarget = MazeGrid.Instance.CellToWorldCenter(currentPath[pathIndex]);
        Vector3 delta = currentTarget - transform.position;

        if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return new Vector2(Mathf.Sign(delta.x), 0f);

        if (Mathf.Abs(delta.y) > 0.001f)
            return new Vector2(0f, Mathf.Sign(delta.y));

        return Vector2.down;
    }

    private void Repath()
    {
        Vector3Int enemyCell = MazeGrid.Instance.WorldToCell(transform.position);
        Vector3Int playerCell = MazeGrid.Instance.WorldToCell(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
        {
            currentPath = GridPathfinder.FindPath(enemyCell, playerCell);

            if(currentPath == null)
            {
                return;
            }

            pathIndex = 1;
            return;
        }

        wanderTimer -= pathUpdateInterval;

        if(currentPath == null || pathIndex >= currentPath.Count || wanderTimer <= 0f)
        {
            currentWanderTarget = GetRandomWanderTarget(enemyCell);
            currentPath = GridPathfinder.FindPath(enemyCell, currentWanderTarget);
            pathIndex = 1;
            wanderTimer = wanderInterval;
        }
    }

    private void MoveAlongPath()
    {
        if(currentPath == null || currentPath.Count == 0 || pathIndex >= currentPath.Count)
            return;

        Vector3 targetPos = MazeGrid.Instance.CellToWorldCenter(currentPath[pathIndex]);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos;
            pathIndex++;
        }
    }

    private Vector3Int GetRandomWanderTarget(Vector3Int origin)
    {
        for (int i = 0; i < maxWanderAttempts; i++)
        {
            int offsetX = Random.Range(-Mathf.RoundToInt(wanderRadius), Mathf.RoundToInt(wanderRadius) + 1);
            int offsetY = Random.Range(-Mathf.RoundToInt(wanderRadius), Mathf.RoundToInt(wanderRadius) + 1);

            Vector3Int candidate = new Vector3Int(origin.x + offsetX, origin.y + offsetY, 0);

            if(MazeGrid.Instance.InBounds(candidate) && MazeGrid.Instance.IsWalkable(candidate))
            {
                return candidate;
            }
        }

        return origin;
    }
}

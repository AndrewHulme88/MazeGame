using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pathUpdateInterval = 0.2f;

    private List<Vector3Int> currentPath;
    private int pathIndex;
    private float repathTimer;

    private void Update()
    {
        repathTimer += Time.deltaTime;

        if (repathTimer <= 0f)
        {
            Repath();
            repathTimer = pathUpdateInterval;
        }

        MoveAlongPath();
    }

    private void Repath()
    {
        Vector3Int enemyCell = MazeGrid.Instance.WorldToCell(transform.position);
        Vector3Int playerCell = MazeGrid.Instance.WorldToCell(player.position);

        currentPath = GridPathfinder.FindPath(enemyCell, playerCell);
        pathIndex = 1;
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
}

using UnityEngine;
using System.Collections.Generic;

public static class GridPathfinder
{
    private static readonly Vector3Int[] Directions =
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    public static List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        if (!MazeGrid.Instance.IsWalkable(start) || !MazeGrid.Instance.IsWalkable(goal))
            return null;

        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        frontier.Enqueue(start);
        cameFrom[start] = start;

        while(frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            if(current == goal)
            {
                break;
            }

            foreach(var dir in Directions)
            {
                Vector3Int next = current + dir;

                if (!MazeGrid.Instance.InBounds(next)) continue;
                if (!MazeGrid.Instance.IsWalkable(next)) continue;
                if (cameFrom.ContainsKey(next)) continue;

                frontier.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        if (!cameFrom.ContainsKey(goal))
            return null;

        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int step = goal;

        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}

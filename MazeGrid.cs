using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGrid : MonoBehaviour
{
    public static MazeGrid Instance;

    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap wallTilemap;

    private BoundsInt bounds;
    private bool[,] walkableGrid;

    public BoundsInt Bounds => bounds;

    private void Awake()
    {
        Instance = this;
        Debug.Log("MazeGrid Awake: Building walkable grid...");
        BuildWalkableGrid();
    }

    public void BuildWalkableGrid()
    {
        bounds = wallTilemap.cellBounds;
        walkableGrid = new bool[bounds.size.x, bounds.size.y];

        for(int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(bounds.xMin + x, bounds.yMax + y, 0);
                bool hasWall = wallTilemap.HasTile(cellPos);
                walkableGrid[x, y] = !hasWall;
            }
        }
    }

    public bool InBounds(Vector3Int cellPos)
    {
        return cellPos.x >= bounds.xMin && cellPos.x < bounds.xMax &&
               cellPos.y >= bounds.yMin && cellPos.y < bounds.yMax;
    }

    public bool IsWalkable(Vector3Int cellPos)
    {
        if (!InBounds(cellPos))
            return false;

        int x = cellPos.x - bounds.xMin;
        int y = cellPos.y - bounds.yMin;
        return walkableGrid[x, y];
    }

    public void SetBlocked(Vector3Int cellPos, bool blocked)
    {
        if (!InBounds(cellPos))
            return;

        int x = cellPos.x - bounds.xMin;
        int y = cellPos.y - bounds.yMin;
        walkableGrid[x, y] = !blocked;
    }

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public Vector3 CellToWorldCenter(Vector3Int cellPos)
    {
        return grid.GetCellCenterWorld(cellPos);
    }
}

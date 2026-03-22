using UnityEngine;

public class PlacedBlock : MonoBehaviour
{
    private Vector3Int cellPosition;
    private float lifetime;

    public void Initialize(Vector3Int cellPos, float duration)
    {
        this.cellPosition = cellPos;
        this.lifetime = duration;
        MazeGrid.Instance.SetBlocked(cellPosition, true);
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            MazeGrid.Instance.SetBlocked(cellPosition, false);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(MazeGrid.Instance != null)
        {
            MazeGrid.Instance.SetBlocked(cellPosition, false);
        }
    }
}
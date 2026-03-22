using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject startPoint;

    private void Start()
    {
        ReturnToStartPoint();
    }

    public void ReturnToStartPoint()
    {
        if (startPoint != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = startPoint.transform.position;
            }
        }
    }
}

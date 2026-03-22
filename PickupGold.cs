using UnityEngine;

public class PickupGold : MonoBehaviour
{
    [SerializeField] private int goldAmount = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //GameManager.Instance.AddGold(1);
            Destroy(gameObject);
        }
    }
}

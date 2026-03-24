using UnityEngine;

public class PickupGold : MonoBehaviour
{
    [SerializeField] private int goldAmount = 100;
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddGold(goldAmount);

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}

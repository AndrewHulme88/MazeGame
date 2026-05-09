using UnityEngine;

public class PickupGold : MonoBehaviour
{
    [SerializeField] private int goldAmount = 100;
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private AudioClip pickupSound;

    private AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddGold(goldAmount);

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            audioPlayer.PlayOneShot(pickupSound);

            FindFirstObjectByType<LevelEnd>().DecreaseCoinCount();

            Destroy(gameObject);
        }
    }
}

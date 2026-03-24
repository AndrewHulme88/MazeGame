using UnityEngine;
using TMPro;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsRemainingText;

    private int totalCoinsInLevel;
    private bool allCoinsCollected = false;

    private void Start()
    {
        coinsRemainingText.gameObject.SetActive(false);
        totalCoinsInLevel = FindObjectsByType<PickupGold>(FindObjectsSortMode.None).Length;
        UpdateCoinsRemainingText();
    }

    public void DecreaseCoinCount()
    {
        totalCoinsInLevel--;
        UpdateCoinsRemainingText();

        if (totalCoinsInLevel <= 0)
        {
            allCoinsCollected = true;
            coinsRemainingText.gameObject.SetActive(false);
        }
    }

    private void UpdateCoinsRemainingText()
    {
        if (coinsRemainingText != null)
        {
            coinsRemainingText.text = $"Coins Remaining: {totalCoinsInLevel}";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(allCoinsCollected)
            {
                //GameManager.Instance.LevelCompleted();
            }
            else
            {
                coinsRemainingText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coinsRemainingText.gameObject.SetActive(false);
        }
    }
}
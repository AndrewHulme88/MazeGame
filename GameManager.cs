using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentGold = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindFirstObjectByType<UIManager>().UpdateGold(currentGold);
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        FindFirstObjectByType<UIManager>().UpdateGold(currentGold);
    }

    public void LevelCompleted()
    {
        Debug.Log("Level Completed! Total Gold: " + currentGold);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

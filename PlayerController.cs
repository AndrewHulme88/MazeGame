using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public int playerLives = 3;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        FindFirstObjectByType<UIManager>().UpdateLives(playerLives);
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerLives--;

            FindFirstObjectByType<UIManager>().UpdateLives(playerLives);
            FindFirstObjectByType<LevelManager>().ReturnToStartPoint();

            if (playerLives <= 0)
            {
                playerLives = 0;
                Debug.Log("Game Over!");
                // Implement game over logic here (e.g., reload scene, show game over screen)
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    public int playerLives = 3;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        FindFirstObjectByType<UIManager>().UpdateLives(playerLives);

        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    private void Update()
    {
        moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();

        if(moveInput.sqrMagnitude > 0.01f)
        {
            if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                moveInput = new Vector2(Mathf.Sign(moveInput.x), 0f);
            }
            else
            {
                moveInput = new Vector2(0f, Mathf.Sign(moveInput.y));
            }

            lastMoveDirection = moveInput;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        animator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
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

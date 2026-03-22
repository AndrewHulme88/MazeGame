using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private float blockLifetime = 5f;
    [SerializeField] private int MaxBlocks = 3;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask blockingLayers;

    private InputAction moveAction;
    private InputAction placeBlockAction;
    private Queue<PlacedBlock> placedBlocks = new Queue<PlacedBlock>();
    private Vector3Int lastMoveDirection = Vector3Int.right;

    private void Awake()
    {
        if(playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        moveAction = playerInput.actions["Move"];
        placeBlockAction = playerInput.actions["PlaceBlock"];
    }

    private void Update()
    {
        UpdateLastMoveDirection();

        if(placeBlockAction.WasPressedThisFrame())
        {
            PlaceBlock();
        }
    }

    private void UpdateLastMoveDirection()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if(moveInput.sqrMagnitude < 0.01f)
        {
            return;
        }

        if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            lastMoveDirection = moveInput.x > 0 ? Vector3Int.right : Vector3Int.left;
        }
        else
        {
            lastMoveDirection = moveInput.y > 0 ? Vector3Int.up : Vector3Int.down;
        }
    }

    private void PlaceBlock()
    {
        Vector3Int playerCell = MazeGrid.Instance.WorldToCell(transform.position);
        Vector3Int targetCell = playerCell + lastMoveDirection;

        if(!MazeGrid.Instance.InBounds(targetCell))
        {
            return;
        }

        if(!MazeGrid.Instance.IsWalkable(targetCell))
        {
            return;
        }

        Collider2D hit = Physics2D.OverlapBox(
            MazeGrid.Instance.CellToWorldCenter(targetCell),
            Vector2.one * 0.8f,
            0f,
            blockingLayers
        );

        if(hit != null)
        {
            return;
        }

        if(placedBlocks.Count >= MaxBlocks)
        {
            PlacedBlock oldestBlock = placedBlocks.Dequeue();

            if (oldestBlock != null)
            {
                Destroy(oldestBlock.gameObject);
            }
        }

        GameObject blockObj = Instantiate(
            blockPrefab,
            MazeGrid.Instance.CellToWorldCenter(targetCell),
            Quaternion.identity
        );

        PlacedBlock block = blockObj.GetComponent<PlacedBlock>();
        block.Initialize(targetCell, blockLifetime);
        placedBlocks.Enqueue(block);
    }
}

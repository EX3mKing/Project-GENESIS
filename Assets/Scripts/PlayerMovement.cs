using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Transform movePoint;
    public bool allowDiagonalMovement;

    public InputActionReference move;
    private Vector2 _moveDirection;

    public LayerMask whatStopsMovement;
    
    private void Start()
    {
        movePoint.SetParent(null);
    }

    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
        _moveDirection = new Vector2(Mathf.Round(_moveDirection.x), Mathf.Round(_moveDirection.y));
        Vector3 nextMovePointPosition = movePoint.position;
        
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, movePoint.position) > 0.1f) return;

        bool availableMoveX = !Physics2D.OverlapCircle(movePoint.position + new Vector3(_moveDirection.x, 0f, 0f), 0.2f, whatStopsMovement);
        bool availableMoveY = !Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, _moveDirection.y, 0f), 0.2f, whatStopsMovement);
        
        bool shouldMoveX = availableMoveX && Mathf.Abs(_moveDirection.x) > 0.1f;
        bool shouldMoveY = availableMoveY && Mathf.Abs(_moveDirection.y) > 0.1f;

        if (allowDiagonalMovement)
        {
            bool availableMoveDiagonal = !Physics2D.OverlapCircle(movePoint.position + new Vector3(_moveDirection.x, _moveDirection.y, 0f), 0.2f, whatStopsMovement);
            bool shouldMoveDiagonal = availableMoveDiagonal && shouldMoveX && shouldMoveY;
            
            if (shouldMoveDiagonal)
            {
                nextMovePointPosition += new Vector3(_moveDirection.x, _moveDirection.y, 0f);
            }
            else
            {
                if (shouldMoveX)
                {
                    nextMovePointPosition += new Vector3(_moveDirection.x, 0f, 0f);
                }
                else if (shouldMoveY)
                {
                    nextMovePointPosition += new Vector3(0f, _moveDirection.y, 0f);
                }
            }
        }
        else
        {
            if (shouldMoveX)
            {
                nextMovePointPosition += new Vector3(_moveDirection.x, 0f, 0f);
            }
            else if (shouldMoveY)
            {
                nextMovePointPosition += new Vector3(0f, _moveDirection.y, 0f);
            }
        }
        
        movePoint.position = nextMovePointPosition;
    }
}

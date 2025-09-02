using System;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float moveSpeed;
    public Transform movePoint;
    public Transform lastMovePoint;
    public bool allowDiagonalMovement;
    public LayerMask whatStopsMovement;
    
    protected Vector2 lastLookingDirection = Vector2.right;
    protected bool moving = false;

    private void Start()
    {
        movePoint.SetParent(null);
        lastMovePoint.SetParent(null);
        lastMovePoint.position = movePoint.position;
    }

    protected void MoveByDirection(Vector2 direction)
    {
        Vector3 nextMovePointPosition = movePoint.position;
        
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        moving = true;
        if (Vector3.Distance(transform.position, movePoint.position) > 0.1f) return;
        
        bool availableMoveX = !Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, 0f, 0f), 0.2f, whatStopsMovement);
        bool availableMoveY = !Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, direction.y, 0f), 0.2f, whatStopsMovement);
        
        bool shouldMoveX = availableMoveX && Mathf.Abs(direction.x) > 0.1f;
        bool shouldMoveY = availableMoveY && Mathf.Abs(direction.y) > 0.1f;
        
        bool isXPositive = direction.x > 0f;
        bool isYPositive = direction.y > 0f;

        if (Mathf.Abs(direction.y) > 0.1f) lastLookingDirection = isYPositive ? Vector2.up : Vector2.down;
        if (Mathf.Abs(direction.x) > 0.1f) lastLookingDirection = isXPositive ? Vector2.right : Vector2.left;
        
        if (allowDiagonalMovement)
        {
            bool availableMoveDiagonal = !Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y, 0f), 0.2f, whatStopsMovement);
            bool shouldMoveDiagonal = availableMoveDiagonal && shouldMoveX && shouldMoveY;
            
            if (shouldMoveDiagonal)
            {
                nextMovePointPosition += new Vector3(direction.x, direction.y, 0f);
                lastLookingDirection = isXPositive ? Vector2.right : Vector2.left;
            }
            else
            {
                if (shouldMoveX)
                {
                    nextMovePointPosition += new Vector3(direction.x, 0f, 0f);
                }
                else if (shouldMoveY)
                {
                    nextMovePointPosition += new Vector3(0f, direction.y, 0f);
                }
            }
        }
        else
        {
            if (shouldMoveX)
            {
                nextMovePointPosition += new Vector3(direction.x, 0f, 0f);
            }
            else if (shouldMoveY)
            {
                nextMovePointPosition += new Vector3(0f, direction.y, 0f);
            }
        }

        if (!movePoint.position.Equals(nextMovePointPosition))
        {
            lastMovePoint.position = movePoint.position;
            movePoint.position = nextMovePointPosition;
        }
        else
        {
            moving = false;
        }
    }

    protected void MoveToPoint(Vector3 point)
    {
        Vector2 direction = point - movePoint.position;
        MoveByDirection(direction);
    }
}

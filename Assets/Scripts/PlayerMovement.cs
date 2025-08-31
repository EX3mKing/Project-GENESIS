using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : EntityMovement
{
    /*
    public float moveSpeed;
    public Transform movePoint;
    public bool allowDiagonalMovement;
    public LayerMask whatStopsMovement;
    */

    public InputActionReference move;
    private Vector2 _moveDirection;
    public Animator animator;
    
    private void Start()
    {
        movePoint.SetParent(null);
    }

    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
        _moveDirection = new Vector2(Mathf.Round(_moveDirection.x), Mathf.Round(_moveDirection.y));
        Move(_moveDirection);
        Animate(); // needs to be after Move
    }

    private void Animate()
    {
        if (_moveDirection.magnitude > 0.1f) animator.SetFloat("IsMoving", 1f);
        else animator.SetFloat("IsMoving", 0f);
        animator.SetFloat("DirX", lastLookingDirection.x);
        animator.SetFloat("DirY", lastLookingDirection.y);
    }
}

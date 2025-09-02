using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : EntityMovement
{
    public InputActionReference move;
    private Vector2 _moveDirection;
    public Animator animator;
    public MiniAnimator miniAnimator;

    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
        _moveDirection = new Vector2(Mathf.Round(_moveDirection.x), Mathf.Round(_moveDirection.y));
        MoveByDirection(_moveDirection);
        miniAnimator.UpdateAnimation(_moveDirection.magnitude > 0.1f, lastLookingDirection); // needs to be after move
        //Animate(); // needs to be after Move
    }

    private void Animate()
    {
        if (_moveDirection.magnitude > 0.1f) animator.SetFloat("IsMoving", 1f);
        else animator.SetFloat("IsMoving", 0f);
        animator.SetFloat("DirX", lastLookingDirection.x);
        animator.SetFloat("DirY", lastLookingDirection.y);
    }
}

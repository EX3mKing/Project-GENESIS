using UnityEngine;

public class FollowerMovement : EntityMovement
{
    public Transform targetLastMovePoint;
    public MiniAnimator animator;
    void Update()
    {
        MoveToPoint(targetLastMovePoint.position);
        animator.UpdateAnimation(moving, lastLookingDirection);
    }
}

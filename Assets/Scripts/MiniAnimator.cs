using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAnimator : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Sprite defaultSprite;
    public float timeBetweenFramesWhileMoving;
    public float timeBetweenFramesWhileIdle;
    
    public List<Sprite> IdleDownSprites;
    public List<Sprite> IdleUpSprites;
    public List<Sprite> IdleLeftSprites;
    public List<Sprite> IdleRightSprites;
    
    public List<Sprite> MoveDownSprites;
    public List<Sprite> MoveUpSprites;
    public List<Sprite> MoveLeftSprites;
    public List<Sprite> MoveRightSprites;
    
    public bool flipXWhileLookingRight = false;
    public bool flipXWhileLookingLeft = false;
    public bool flipYWhileLookingUp = false;
    public bool flipYWhileLookingDown = false;
    public bool primaryLookDirectionIsVertical;
    
    private bool _isMoving = false;
    private bool _wasMoving = false;
    private Vector2 _lookingDirection = Vector2.down;
    private Vector2 _lastLookingDirection = Vector2.down;
    private float countDown;

    public void UpdateAnimation(bool moving, Vector2 direction)
    {
        _isMoving = moving;
        _lookingDirection = direction;

        if ((_isMoving != _wasMoving) || (_lookingDirection != _lastLookingDirection))
        {
            PickAndStartNewAnimation();
        }

        _wasMoving = _isMoving;
        _lastLookingDirection =  _lookingDirection;
    }
    
    private IEnumerator IE_StepThroughAnimation(List<Sprite> frames)
    {
        int currentFrame = 0;
        renderer.sprite = frames[currentFrame];
        while (true)
        {
            if (_isMoving) yield return new WaitForSeconds(timeBetweenFramesWhileMoving);
            else yield return new WaitForSeconds(timeBetweenFramesWhileIdle);
            renderer.sprite = frames[currentFrame];
            currentFrame++;
            if (currentFrame >= frames.Count) currentFrame = 0;
        }
    }

    private void PickAndStartNewAnimation()
    {
        StopAllCoroutines();
        renderer.flipX = false;
        renderer.flipY = false;
        List<Sprite> spritesForNextAnimation = new List<Sprite>();
        
        Vector2 direction = Vector2.zero;
        if (_lookingDirection.magnitude < 0.1f) direction = _lastLookingDirection;
        else direction = _lookingDirection;
        
        if (_isMoving)
        {
            switch (direction)
            {
                case Vector2 v when v.Equals(Vector2.up):
                    spritesForNextAnimation = MoveUpSprites;
                    renderer.flipY = flipYWhileLookingUp;
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    spritesForNextAnimation = MoveDownSprites;
                    renderer.flipY = flipYWhileLookingDown;
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    spritesForNextAnimation = MoveLeftSprites;
                    renderer.flipX = flipXWhileLookingLeft;
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    spritesForNextAnimation = MoveRightSprites;
                    renderer.flipX = flipXWhileLookingRight;
                    break;
                default:
                    Debug.LogError("smthing is wrong champ");
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Vector2 v when v.Equals(Vector2.up):
                    spritesForNextAnimation = IdleUpSprites;
                    renderer.flipY = flipYWhileLookingUp;
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    spritesForNextAnimation = IdleDownSprites;
                    renderer.flipY = flipYWhileLookingDown;
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    spritesForNextAnimation = IdleLeftSprites;
                    renderer.flipX = flipXWhileLookingLeft;
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    spritesForNextAnimation = IdleRightSprites;
                    renderer.flipX = flipXWhileLookingRight;
                    break;
                default:
                    Debug.LogError("smthing is wrong champ");
                    break;
            }
        }

        StartCoroutine(IE_StepThroughAnimation(spritesForNextAnimation));
    }
}

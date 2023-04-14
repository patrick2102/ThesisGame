using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerScript : MonoBehaviour
{
    public enum RobotSprites
    {
        frontSprites, backSprites, leftSprites, rightSprites
    }

    public enum RobotAnimation
    {
        Idle, Walk, WalkSide, IdleSide
    }

    public enum Direction
    { 
        Left, Right, Up, Down
    }

    public List<SpriteRenderer> spriteRenderers;

    private List<List<Sprite>> sprites;
    public List<Sprite> frontSprites;
    public List<Sprite> backSprites;
    public List<Sprite> leftSprites;
    public List<Sprite> rightSprites;

    public Animator animator;

    [SerializeField] private Rigidbody2D rb;


    public RobotSprites currentSprite = RobotSprites.frontSprites;
    public RobotAnimation currentAnimation = RobotAnimation.Idle;
    public Direction currentDirection = Direction.Down;

    public float walkAnimationThreshold = 1.0f;
    public float directionChangeThreshold = 0.01f;



    //public List<Texture2D> spriteSheetTextures;



    private void Start()
    {
        // Get all SpriteRenderer components in the children of the game object
        //spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());

        sprites = new List<List<Sprite>>(){
            frontSprites,
            backSprites,
            leftSprites,
            rightSprites
        };
    }

    void Update()
    {
        CheckForDirectionChange();
    }

    public void CheckForDirectionChange()
    {
        Vector2 direction = rb.velocity;
        Direction newDirection = currentDirection;

        
        animator.SetFloat("Speed", direction.magnitude);

        if (direction.magnitude > directionChangeThreshold)
        {
            // Check if x direction is larger than y direction
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Check if x direction is positive or negative
                if (direction.x > 0)
                {
                    newDirection = Direction.Right;
                }
                else
                {
                    newDirection = Direction.Left;
                }
            }
            else
            {
                // Check if y direction is positive or negative
                if (direction.y > 0)
                {
                    newDirection = Direction.Up;
                }
                else
                {
                    newDirection = Direction.Down;
                }
            }
        }
        if (newDirection != currentDirection)
        {
            //Check if the new direction is a different axis than the current direction
            if (newDirection == Direction.Left || newDirection == Direction.Right)
            {
                if (currentDirection == Direction.Up || currentDirection == Direction.Down)
                {
                    animator.SetTrigger("SwitchDirection");
                }
            }
            else if (newDirection == Direction.Up || newDirection == Direction.Down)
            {
                if (currentDirection == Direction.Left || currentDirection == Direction.Right)
                {
                    animator.SetTrigger("SwitchDirection");
                }
            }

            currentDirection = newDirection;

            switch (currentDirection)
            {
                case Direction.Left:
                    ChangeSprite(RobotSprites.leftSprites);
                    //ChangeAnimation(RobotAnimation.WalkSide);
                    break;
                case Direction.Right:
                    ChangeSprite(RobotSprites.rightSprites);
                    //ChangeAnimation(RobotAnimation.WalkSide);
                    break;
                case Direction.Up:
                    ChangeSprite(RobotSprites.backSprites);
                    //ChangeAnimation(RobotAnimation.Walk);
                    break;
                case Direction.Down:
                    ChangeSprite(RobotSprites.frontSprites);
                    //ChangeAnimation(RobotAnimation.Walk);
                    break;
            }
        }

    }

    public void ChangeSprite(RobotSprites sprite)
    {
        currentSprite = sprite;

        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            // Change sprite for each SpriteRenderer using the current frame of the sprite sheet
            spriteRenderers[i].sprite = sprites[(int)currentSprite][i];
        }
    }


    /*
    public void ChangeAnimation(RobotAnimation animation)
    {
        currentAnimation = animation;

        if (animation == RobotAnimation.Walk || animation == RobotAnimation.WalkSide)
        {
            if (currentSprite == RobotSprites.leftSprites || currentSprite == RobotSprites.rightSprites)
            {
                animator.SetFloat("AnimationState", (float)RobotAnimation.WalkSide);
            }
            else
            {
                animator.SetFloat("AnimationState", (float)RobotAnimation.Walk);
            }
        }
        else if (animation == RobotAnimation.Idle || animation == RobotAnimation.IdleSide)
        {
            if (currentSprite == RobotSprites.leftSprites || currentSprite == RobotSprites.rightSprites)
            {
                animator.SetFloat("AnimationState", (float)RobotAnimation.IdleSide);
            }
            else
            {
                animator.SetFloat("AnimationState", (float)RobotAnimation.Idle);
            }
        }
    }
    */
}

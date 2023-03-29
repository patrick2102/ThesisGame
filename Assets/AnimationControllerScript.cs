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
        Idle, Walk, WalkSide
    }



    public List<SpriteRenderer> spriteRenderers;

    private List<List<Sprite>> sprites;
    public List<Sprite> frontSprites;
    public List<Sprite> backSprites;
    public List<Sprite> leftSprites;
    public List<Sprite> rightSprites;

    public Animator animator;


    public RobotSprites currentSprite = RobotSprites.frontSprites;
    public RobotAnimation currentAnimation = RobotAnimation.Idle;
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


    public void Update()
    {

        if (Input.GetKeyUp(KeyCode.W))
        {
            ChangeSprite(RobotSprites.backSprites);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            ChangeSprite(RobotSprites.leftSprites);

        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            ChangeSprite(RobotSprites.frontSprites);

        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            ChangeSprite(RobotSprites.rightSprites);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            ChangeAnimation(RobotAnimation.Idle);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            ChangeAnimation(RobotAnimation.Walk);
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            ChangeAnimation(RobotAnimation.WalkSide);
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

    public void ChangeAnimation(RobotAnimation animation)
    {
        currentAnimation = animation;
        animator.SetInteger("RobotAnimation", (int)currentAnimation);

        //change animation
    }
}

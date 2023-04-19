using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerScript : MonoBehaviour
{
    public enum RobotSprites
    {
        frontSprites, backSprites, leftSprites, rightSprites, happySprites, sadSprites, angrySprites, distractSprites
    }

    public enum RobotAnimation
    {
        Idle, Walk, WalkSide, IdleSide
    }

    public enum Direction
    {
        Left, Right, Up, Down
    }

    public enum EmotionState
    {
        idle, happy, scared, angry
    }

    public List<SpriteRenderer> spriteRenderers;

    private List<List<Sprite>> sprites;
    public List<Sprite> frontSprites;
    public List<Sprite> backSprites;
    public List<Sprite> leftSprites;
    public List<Sprite> rightSprites;
    public List<Sprite> happySprites;
    public List<Sprite> sadSprites;
    public List<Sprite> angrySprites;
    public List<Sprite> distractSprites;

    public Animator animator;

    [SerializeField] private Rigidbody2D rb;


    public RobotSprites currentSprite = RobotSprites.frontSprites;
    public RobotAnimation currentAnimation = RobotAnimation.Idle;
    public Direction currentDirection = Direction.Down;

    public float walkAnimationThreshold = 1.0f;
    public float directionChangeThreshold = 0.01f;

    public float animationTime = 0.0f;
    public float animationTimeMax = 2.0f;
    public bool emoting = false;

    [SerializeField] private Animation happyEmote;
    [SerializeField] private Animation sadEmote;
    [SerializeField] private Animation angryEmote;

    private AudioSource audioSource;

    public AudioClip happySound;
    public AudioClip angrySound;
    public AudioClip sadSound;
    public AudioClip distractSound;

    private void Start()
    {
        // Get all SpriteRenderer components in the children of the game object
        sprites = new List<List<Sprite>>(){
            frontSprites,
            backSprites,
            leftSprites,
            rightSprites,
            happySprites,
            sadSprites,
            angrySprites,
            distractSprites
        };

        happyEmote.gameObject.SetActive(false);
        sadEmote.gameObject.SetActive(false);
        angryEmote.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        //happyEmote.Play();
    }

    void Update()
    {
        //Debug.Log("Emote playing: " + happyEmote.isPlaying);

        if (emoting)
        {
            animationTime += Time.deltaTime;
            if (animationTime > animationTimeMax)
            {
                emoting = false;
                animationTime = 0.0f;
                ChangeSprite(RobotSprites.frontSprites);
                happyEmote.gameObject.SetActive(false);
                sadEmote.gameObject.SetActive(false);
                angryEmote.gameObject.SetActive(false);
            }

            //happyEmote.isPlaying;
        }
        else
        {
            CheckForDirectionChange();
        }
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
                animator.SetBool("IsHorizontal", true);
            }
            else if (newDirection == Direction.Up || newDirection == Direction.Down)
            {
                animator.SetBool("IsHorizontal", false);
            }

            currentDirection = newDirection;

            switch (currentDirection)
            {
                case Direction.Left:
                    ChangeSprite(RobotSprites.leftSprites);
                    break;
                case Direction.Right:
                    ChangeSprite(RobotSprites.rightSprites);
                    break;
                case Direction.Up:
                    ChangeSprite(RobotSprites.backSprites);
                    break;
                case Direction.Down:
                    ChangeSprite(RobotSprites.frontSprites);
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

    public void TriggerHappyAnimation()
    {
        if (GameManager.emotionVersion)
        {
            emoting = true;
            ChangeSprite(RobotSprites.happySprites);
            happyEmote.gameObject.SetActive(true);
            happyEmote.Play();
            audioSource.PlayOneShot(happySound);
        }
        //happyEmote.
    }

    public void TriggerSadAnimation()
    {
        if (GameManager.emotionVersion)
        {
            emoting = true;
            ChangeSprite(RobotSprites.sadSprites);
            sadEmote.gameObject.SetActive(true);
            sadEmote.Play();
            audioSource.PlayOneShot(sadSound);
        }
    }

    public void TriggerAngryAnimation()
    {
        if (GameManager.emotionVersion)
        {
            emoting = true;
            ChangeSprite(RobotSprites.angrySprites);
            angryEmote.gameObject.SetActive(true);
            angryEmote.Play();
            audioSource.PlayOneShot(angrySound);
        }
    }

    public void TriggerDistractAnimation(float time)
    {
        emoting = true;
        ChangeSprite(RobotSprites.distractSprites);
        animationTime = animationTimeMax - time;
        audioSource.PlayOneShot(distractSound);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerPlayer : MonoBehaviour
{
    public enum PlayerSprites
    {
        frontSprites, backSprites, leftSprites, rightSprites
    }

    public List<SpriteRenderer> spriteRenderers;

    private List<List<Sprite>> sprites;
    public List<Sprite> frontSprites;
    public List<Sprite> backSprites;
    public List<Sprite> leftSprites;
    public List<Sprite> rightSprites;

    public Animator animator;

    [SerializeField] private Rigidbody2D rb;


    public PlayerSprites currentSprite = PlayerSprites.frontSprites;

    public float directionChangeThreshold = 0.01f;

    public float animationTime = 0.0f;
    public float animationTimeMax = 2.0f;
    public bool emoting = false;

    private void Start()
    {
        // Get all SpriteRenderer components in the children of the game object
        sprites = new List<List<Sprite>>(){
            frontSprites,
            backSprites,
            leftSprites,
            rightSprites,
        };
    }

    void Update()
    {
        CheckForDirectionChange();
    }

    public void CheckForDirectionChange()
    {
        Vector2 direction = rb.velocity;
        animator.SetFloat("Speed",direction.magnitude);
        if (Input.GetKey(KeyCode.W))
        {
            ChangeSprite(PlayerSprites.backSprites);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ChangeSprite(PlayerSprites.frontSprites);
        }
        else if (Input.GetKey(KeyCode.A)) {
            ChangeSprite(PlayerSprites.leftSprites);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ChangeSprite(PlayerSprites.rightSprites);
        }
    }

    public void ChangeSprite(PlayerSprites sprite)
    {
        currentSprite = sprite;

        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            // Change sprite for each SpriteRenderer using the current frame of the sprite sheet
            spriteRenderers[i].sprite = sprites[(int)currentSprite][i];
        }
    }
}

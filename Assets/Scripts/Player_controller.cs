using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D playerRB;
    private Vector2 movement;

    public static Player_controller instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> footsteps;

    [SerializeField] float timeBetweenSteps = 0.5f;
    private float timeBetweenStepsCounter;

    private int lastIndex = 0;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.y) > 0)
        { 
            timeBetweenStepsCounter += Time.deltaTime;
            if (timeBetweenStepsCounter >= timeBetweenSteps)
            {
                timeBetweenStepsCounter = 0f;
                PlayFootStep();
            }
        }
    }


    void PlayFootStep()
    {
        //Get a random footstep from the Footsteps list
        var index = Random.Range(0, footsteps.Count);

        //To prevent the same footstep from playing twice in a row we check if the index is the same as the
        //last index and if it is we get a new index until it is different from the last index

        int count = 0;

        while (lastIndex == index)
        {
            index = Random.Range(0, footsteps.Count);
            count++;
            if (count > 10)
                break;
        }

        lastIndex = index;

        audioSource.clip = footsteps[index];
        audioSource.Play();

    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(movement.x, movement.y).normalized * moveSpeed;
    }


}

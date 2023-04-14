using UnityEngine;

public class RobotEmotionStateHandler : MonoBehaviour
{
    public enum EmotionState
    {
        idle, happy, scared
    }

    private EmotionState currentEmotionState;
    public float scaredRangeRadius;
    public float animationTime = 0.0f;
    public float animationTimeMax = 2.0f;

    private AudioSource audioSource;

    public AudioClip happySound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        CheckScaredRange();
        CheckForStateUpdate();
    }

    public void CheckForStateUpdate()
    {
        if (currentEmotionState != EmotionState.idle)
        { 
            animationTime += Time.deltaTime;

            if (animationTime >= animationTimeMax)
            {
                SwitchRobotEmotionState(EmotionState.idle);
            }
        }
    }

    public void SwitchRobotEmotionState(EmotionState newState)
    {
        if (currentEmotionState != newState)
        {
            switch (newState)
            {
                case EmotionState.happy:
                    currentEmotionState = newState;
                    // Make call to set animation / sound effect to happy, such as when a pressure plate puzzle suceeded 
                    audioSource.clip = happySound;
                    audioSource.Play();
                    animationTime = 0.0f;
                    Debug.Log("Robot is happy! Yay! :D");
                    break;

                case EmotionState.scared:
                    currentEmotionState = newState;
                    // Make call to set animation / sound effect to scared, such as when a monster is in close proximity to the robot
                    animationTime = 0.0f;
                    Debug.Log("ROBOT IS SCARED!!!");
                    break;
                case EmotionState.idle: 
                    currentEmotionState = newState;
                    animationTime = 0.0f;
                    Debug.Log("Robot neutral");
                    break;
            }
        }
    }

    private void CheckScaredRange()
    {
        var fun = Physics2D.OverlapCircleAll(GameObject.FindGameObjectWithTag(GameObjectTags.Robot.ToString()).GetComponent<Rigidbody2D>().position, scaredRangeRadius);

        foreach (Collider2D col in fun)
        {
            switch (col.gameObject.tag)
            {
                case "Monster":
                    SwitchRobotEmotionState(EmotionState.scared);
                    break;
            }
        }
    }
}

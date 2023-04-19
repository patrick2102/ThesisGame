using UnityEngine;

public class RobotEmotionStateHandler : MonoBehaviour
{
    public enum EmotionState
    {
        idle, happy, scared, angry
    }

    private EmotionState currentEmotionState;
    public float scaredRangeRadius;
    public float animationTime = 0.0f;
    public float animationTimeMax = 2.0f;
    public float timeBetweenScare = 0.0f;
    public float timeBetweenScareMax = 2.0f;

    [SerializeField] private AnimationControllerScript animationController;


    private void Start()
    {
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

        timeBetweenScare += Time.deltaTime;
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
                    
                    animationController.TriggerHappyAnimation();
                    Debug.Log("Robot is happy! Yay! :D");
                    break;

                case EmotionState.scared:
                    currentEmotionState = newState;
                    animationController.TriggerSadAnimation();
                    Debug.Log("ROBOT IS SCARED!!!");
                    break;

                case EmotionState.angry:
                    currentEmotionState = newState;
                    animationController.TriggerAngryAnimation();
                    Debug.Log("ROBOT IS ANGRY!!!");
                    break;

                case EmotionState.idle: 
                    currentEmotionState = newState;
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
                    if (timeBetweenScare >= timeBetweenScareMax)
                    {
                        SwitchRobotEmotionState(EmotionState.scared);
                        timeBetweenScare = 0.0f;
                    }
                    break;
            }
        }
    }
}

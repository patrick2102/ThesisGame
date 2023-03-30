using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RobotEmotionStateHandler : MonoBehaviour
{
    public enum EmotionState
    {
        idle, happy, scared
    }

    private EmotionState currentEmotionState;
    public float scaredRangeRadius;

    void FixedUpdate()
    {
        CheckScaredRange();
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
                    Debug.Log("Robot is happy! Yay! :D");
                    break;

                case EmotionState.scared:
                    currentEmotionState = newState;
                    // Make call to set animation / sound effect to scared, such as when a monster is in close proximity to the robot
                    Debug.Log("ROBOT IS SCARED!!!");
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

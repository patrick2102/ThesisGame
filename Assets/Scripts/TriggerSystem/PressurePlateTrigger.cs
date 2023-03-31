using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : TriggerBase
{
    public GameObject door;
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private Transform moveToPosition;
    [SerializeField] private GameObject objectToRemove;
    [SerializeField] private CheckpointTrigger checkpoint;
    public bool resetPositionAtRestart;
    private Vector3 originalDoorPosition;
    private RobotEmotionStateHandler robotEmotionStateHandler;

    public void Awake()
    {
        originalDoorPosition= door.transform.position;
        robotEmotionStateHandler = GameObject.FindGameObjectWithTag(GameObjectTags.Robot.ToString()).GetComponent<RobotEmotionStateHandler>();
    }

    public override void HandleTriggerEnter(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                if(doorCollider != null)
                    doorCollider.enabled = false;
                door.transform.position = moveToPosition.position;
                if (objectToRemove != null)
                    objectToRemove.SetActive(false);
                break;
            case nameof(GameObjectTags.Robot):
                if (doorCollider != null)
                    doorCollider.enabled = false;
                door.transform.position = moveToPosition.position;
                if (objectToRemove != null)
                    objectToRemove.SetActive(false);
                robotEmotionStateHandler.SwitchRobotEmotionState(RobotEmotionStateHandler.EmotionState.happy);
                break;
            default:
                break;
        }
    }

    public override void HandleTriggerExit(string tag)
    {
        return;
    }

    public Vector3 GetOriginalDoorPosition()
    {
        return originalDoorPosition;
    }

    public void ResetTrigger()
    {
        if (checkpoint != null && !checkpoint.checkpointTriggered)
        {
            if (doorCollider != null)
                doorCollider.enabled = true;

            if (objectToRemove != null)
                objectToRemove.SetActive(true);

            door.transform.position = originalDoorPosition;
        }
    }
}

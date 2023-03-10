using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : TriggerBase
{
    public GameObject door;
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private Transform moveToPosition;
    [SerializeField] private GameObject objectToRemove;
    public bool resetPositionAtRestart;
    private Vector3 originalDoorPosition;

    public void Awake()
    {
        originalDoorPosition= door.transform.position;
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
                    Destroy(objectToRemove);
                break;
            case nameof(GameObjectTags.Robot):
                if (doorCollider != null)
                    doorCollider.enabled = false;
                door.transform.position = moveToPosition.position;
                if (objectToRemove != null)
                    Destroy(objectToRemove);
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
}

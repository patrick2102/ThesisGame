using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : TriggerBase
{
    [SerializeField] private GameObject door;
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private Transform moveToPosition;

    public override void HandleTriggerEnter(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                doorCollider.enabled = false;
                door.transform.position = moveToPosition.position;
                break;
            case nameof(GameObjectTags.Robot):
                doorCollider.enabled = false;
                door.transform.position = moveToPosition.position;
                break;
            default:
                break;
        }
    }

    public override void HandleTriggerExit(string tag)
    {
        return;
    }
}

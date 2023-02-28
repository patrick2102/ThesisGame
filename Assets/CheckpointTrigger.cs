using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : TriggerBase
{

    [SerializeField] private Transform respawnPoint;
    public int checkpointOrder;
    public bool checkpointTriggered = false;

    public override void HandleTrigger(string tag)
    {
        if (checkpointTriggered)
            return;

        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                GameManager.instance.TriggerCheckpoint(this);
                checkpointTriggered = true;
                break;
            default:
                break;
        }
    }

    public Transform GetSpawnPoint()
    {
        return respawnPoint;
    }
}

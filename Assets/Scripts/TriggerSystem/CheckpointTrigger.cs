using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : TriggerBase
{

    [SerializeField] private Transform respawnPoint;
    public int checkpointOrder;
    private bool playerTriggered = false;
    private bool robotTriggered = false;
    public bool checkpointTriggered = false;

    public override void HandleTriggerEnter(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                playerTriggered = true;
                break;
            case nameof(GameObjectTags.Robot):
                robotTriggered = true;
                break;
            default:
                break;
        }
        TriggerCheckpoint();
    }

    public Transform GetSpawnPoint()
    {
        return respawnPoint;
    }

    public override void HandleTriggerExit(string tag)
    {
        return;
    }
    public void TriggerCheckpoint()
    {
        if (playerTriggered && robotTriggered)
        {
            checkpointTriggered = true;
            GameManager.instance.TriggerCheckpoint(this);
        }
    }
}

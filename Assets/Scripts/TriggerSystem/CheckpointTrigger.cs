using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : TriggerBase
{

    [SerializeField] private Transform respawnPoint;
    public int checkpointOrder;

    public override void HandleTriggerEnter(string tag)
    {

        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                GameManager.instance.TriggerCheckpoint(this);
                break;
            default:
                break;
        }
    }

    public Transform GetSpawnPoint()
    {
        return respawnPoint;
    }

    public override void HandleTriggerExit(string tag)
    {
        return;
    }
}

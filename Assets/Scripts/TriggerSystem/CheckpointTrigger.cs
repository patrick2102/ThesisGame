using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : TriggerBase
{

    [SerializeField] private Transform respawnPoint;
    public int checkpointOrder;

    public override void HandleTrigger(string tag)
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
}

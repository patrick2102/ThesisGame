using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionTrigger : TriggerBase
{
    private bool playerTriggered;
    private bool robotTriggered;

    public int nextLevel;

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
        ChangeLevel();
    }

    public override void HandleTriggerExit(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                playerTriggered = false;
                break;
            case nameof(GameObjectTags.Robot):
                robotTriggered = false;
                break;
            default:
                break;
        }
    }

    public void ChangeLevel() 
    {
        if (playerTriggered && robotTriggered)
        {
            GameManager.instance.ChangeLevel(nextLevel);
        }
    }
}

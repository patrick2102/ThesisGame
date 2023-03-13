using UnityEngine;

public class PlayerDeathTrigger : TriggerBase
{
    public override void HandleTriggerEnter(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                GameManager.instance.PlayerDeath();
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

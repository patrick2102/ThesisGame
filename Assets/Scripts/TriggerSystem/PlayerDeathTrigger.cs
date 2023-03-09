using UnityEngine;

public class PlayerDeathTrigger : TriggerBase
{
    public override void HandleTrigger(string tag)
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
}

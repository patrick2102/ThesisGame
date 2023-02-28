using UnityEngine;

public class DeathTrigger : TriggerBase
{
    public override void HandleTrigger(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                GameManager.instance.PlayerDeath();
                break;
            case nameof(GameObjectTags.Robot):
                GameManager.instance.RobotDeath();
                break;
            default:
                break;
        }
    }
}

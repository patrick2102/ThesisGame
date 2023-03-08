using UnityEngine;

public class RobotDeathTrigger : TriggerBase
{
    public override void HandleTrigger(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Robot):
                GameManager.instance.RobotDeath();
                break;
            default:
                break;
        }
    }
}

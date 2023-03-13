using UnityEngine;

public class RobotDeathTrigger : TriggerBase
{
    public override void HandleTriggerEnter(string tag)
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

    public override void HandleTriggerExit(string tag)
    {
        return;
    }
}

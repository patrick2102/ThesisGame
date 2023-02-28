using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTriggers(collision.gameObject.tag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTriggers(collision.gameObject.tag);
    }

    private void HandleTriggers(string tag)
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

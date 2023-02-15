using UnityEngine;

/*
 * Robot controller used by an AI program to control the behavior of the robot.
 */
public class RobotController : MonoBehaviour
{
    public float speed = 50;
    [SerializeField] Rigidbody2D rb;

    public void MoveDirection(Vector3 direction)
    {
        direction.Normalize();

        var force = speed * direction;
        rb.AddForce(force);
    }
}

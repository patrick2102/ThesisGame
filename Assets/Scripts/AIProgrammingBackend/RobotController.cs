using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotController : MonoBehaviour
{
    public float speed = 50;
    [SerializeField] Rigidbody2D rb;

    private void Start()
    {

    }


    public void MoveDirection(Vector3 direction)
    {
        direction.Normalize();

        var force = speed * direction;
        rb.AddForce(force);
    }
}

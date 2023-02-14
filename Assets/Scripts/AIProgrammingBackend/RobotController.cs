using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotController : MonoBehaviour
{
    public float speed = 100;


    public void MoveDirection(Vector3 direction)
    {
        transform.position += speed * Time.deltaTime * direction;
    }
}

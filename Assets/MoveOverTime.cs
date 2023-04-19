using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    public float objectMoveSpeed;
    private GameObject robot;

    // Start is called before the first frame update
    void Start()
    {
        objectMoveSpeed = 0.5f;
        robot = GameObject.FindGameObjectsWithTag("Robot")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (robot.GetComponent<RobotController>().behaviorState == RobotController.RobotBehaviourState.following)
        {
            float distanceToWater = robot.GetComponent<RobotController>().GetCurrentWaterDistance();
            if (distanceToWater > 1.7f)
            {
                gameObject.transform.position += (objectMoveSpeed * distanceToWater - 0.8f) * Time.deltaTime * Vector3.up;
            }
        }
    }
}

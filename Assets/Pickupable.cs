using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{

    public enum State
    {
        onGround, pickedUp
    }

    public State currentState = State.onGround;

    void Update()
    {
        if (currentState == State.pickedUp)
        {
            //When the object is picked up by the robot, position is updated to be slightly in front of the robot
            transform.position = RobotController.instance.transform.position + new Vector3(0, 2.0f, 0);
        }
    }

    //Set state to pickedUp
    public void PickUp()
    {
        currentState = State.pickedUp;
    }

    public void PutDown()
    {
        currentState = State.onGround;
    }

}

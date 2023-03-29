using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportCommand : AICommand
{
    float timer; // Timer to that counts up to maxTimer to control time before going to the next command
    [SerializeField] public float maxTimer;
    bool firstStep = true;
    Transform target;
    Transform putDownLocation;

    public override ProgramStatus Step(RobotController rbc)
    {
        if (firstStep)
        {
            rbc.SetBehaviorState(RobotController.RobotBehaviourState.pickup);
            firstStep = false;
        }

        if (rbc.behaviorState == RobotController.RobotBehaviourState.pickup)
        {

            if (target == null)
                target = RobotController.instance.objectToPickup;


            var fromRobotToObject = target.position - rbc.transform.position;
            var dist = fromRobotToObject.magnitude;
            fromRobotToObject = fromRobotToObject.normalized;

            rbc.MoveDirection(fromRobotToObject);

            if (dist < 1.0f)
            {
                //Make robot pick up object
                rbc.SetBehaviorState(RobotController.RobotBehaviourState.putdown);
            }

            return ProgramStatus.running;
        }
        else if (rbc.behaviorState == RobotController.RobotBehaviourState.putdown)
        {
            var fromRobotToPutDown = putDownLocation.position - rbc.transform.position;
            var dist = fromRobotToPutDown.magnitude;
            fromRobotToPutDown = fromRobotToPutDown.normalized;

            rbc.MoveDirection(fromRobotToPutDown);

            if (dist < 1.0f)
            {
                //Make robot pick up object
                rbc.SetBehaviorState(RobotController.RobotBehaviourState.none);
                firstStep = true;
                return ProgramStatus.stopped;
            }
            return ProgramStatus.running;
        }


        return ProgramStatus.running;
    }
}

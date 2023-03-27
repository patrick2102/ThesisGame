using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportCommand : AICommand
{
    bool firstRun = true;
    Transform pickUpObject;

    public override ProgramStatus Step(RobotController rbc)
    {
        if (firstRun)
        {
            rbc.SetBehaviorState(RobotController.RobotBehaviourState.pickup);
        }

        if (rbc.behaviorState == RobotController.RobotBehaviourState.pickup)
        {

            if (pickUpObject == null)
                pickUpObject = RobotController.instance.objectToPickup;


            var fromRobotToObject = pickUpObject.position - rbc.transform.position;
            var dist = fromRobotToObject.magnitude;
            fromRobotToObject = fromRobotToObject.normalized;

            timer += Time.deltaTime;
            return ProgramStatus.running;
        }
        else
        {
            rbc.SetBehaviorState(RobotController.RobotBehaviourState.none);
            timer = 0;
            return ProgramStatus.stopped;
        }
    }
}

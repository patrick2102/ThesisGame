using UnityEngine;

public class TransportCommand : AICommand
{
    private bool isFirstStep = true;
    [SerializeField] private float pickupDistance;
    private Transform targetObject;
    private Transform putDownLocation;

    public override ProgramStatus Step(RobotController robotController)
    {
        if (isFirstStep)
        {
            robotController.SetBehaviorState(RobotController.RobotBehaviourState.pickup);
            isFirstStep = false;
        }

        if (robotController.behaviorState == RobotController.RobotBehaviourState.pickup)
        {

            if (targetObject == null)
                targetObject = RobotController.instance.objectToPickup;


            var fromRobotToObject = targetObject.position - robotController.transform.position;
            var dist = fromRobotToObject.magnitude;
            fromRobotToObject = fromRobotToObject.normalized;

            robotController.MoveDirection(fromRobotToObject);

            if (dist < pickupDistance)
            {
                //Make robot pick up object
                robotController.SetBehaviorState(RobotController.RobotBehaviourState.putdown);
            }

            return ProgramStatus.running;
        }
        else if (robotController.behaviorState == RobotController.RobotBehaviourState.putdown)
        {
            var fromRobotToPutDown = putDownLocation.position - robotController.transform.position;
            var dist = fromRobotToPutDown.magnitude;
            fromRobotToPutDown = fromRobotToPutDown.normalized;

            robotController.MoveDirection(fromRobotToPutDown);

            if (dist < pickupDistance)
            {
                //Make robot pick up object
                robotController.SetBehaviorState(RobotController.RobotBehaviourState.none);
                isFirstStep = true;
                return ProgramStatus.stopped;
            }
            return ProgramStatus.running;
        }


        return ProgramStatus.stopped;
    }
}

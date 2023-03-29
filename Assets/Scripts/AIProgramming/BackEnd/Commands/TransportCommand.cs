using UnityEngine;

public class TransportCommand : AICommand
{
    private bool isFirstStep = true;
    [SerializeField] private float pickupDistance;
    private Pickupable targetObject;
    private Transform putDownLocation;

    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Pickupable").GetComponent<Pickupable>();
        putDownLocation = GameObject.Find("MoveToSpot").transform;
    }

    public override ProgramStatus Step(RobotController robotController)
    {
        if (isFirstStep)
        {
            robotController.SetBehaviorState(RobotController.RobotBehaviourState.pickup);
            isFirstStep = false;
        }

        if (robotController.behaviorState == RobotController.RobotBehaviourState.pickup)
        {

            // if (targetObject == null)
            //     targetObject = RobotController.instance.objectToPickup;


            var fromRobotToObject = targetObject.transform.position - robotController.transform.position;
            var dist = fromRobotToObject.magnitude;

            robotController.MoveDirection(fromRobotToObject.normalized);

            if (dist < pickupDistance)
            {
                //Make robot pick up object
                robotController.PickUp(targetObject);
            }

            return ProgramStatus.running;
        }
        else if (robotController.behaviorState == RobotController.RobotBehaviourState.putdown)
        {
            var fromRobotToPutDown = putDownLocation.position - robotController.transform.position;
            var dist = fromRobotToPutDown.magnitude;

            robotController.MoveDirection(fromRobotToPutDown.normalized);

            if (dist < pickupDistance)
            {
                //Make robot pick up object
                robotController.PutDown(targetObject);
                isFirstStep = true;
                return ProgramStatus.stopped;
            }
            return ProgramStatus.running;
        }


        return ProgramStatus.stopped;
    }
}

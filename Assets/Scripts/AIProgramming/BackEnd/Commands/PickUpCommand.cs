using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpCommand : AICommand
{
    public float pickupDistance;
    public bool running = false;
    public List<Pickupable> pickupables;
    public Pickupable targetObject;


    void Start()
    {
        //targetObject = GameObject.FindGameObjectWithTag("Pickupable").GetComponent<Pickupable>();
        pickupables = GameObject.FindGameObjectsWithTag("Pickupable").Select(x => x.GetComponent<Pickupable>()).ToList();
    }

    private void FixedUpdate()
    {
        if (!running)
        {
            targetObject = pickupables.OrderBy(x => (x.transform.position - RobotController.instance.transform.position).magnitude).First();


        }
    }

    public override ProgramStatus Step(RobotController robotController)
    {
        running = true;

        var fromRobotToObject = targetObject.transform.position - robotController.transform.position;
        var dist = fromRobotToObject.magnitude;

        robotController.MoveDirection(fromRobotToObject.normalized);

        if (dist < pickupDistance)
        {
            robotController.PickUp(targetObject);
            running = false;
            return ProgramStatus.stopped;
        }
        else
        {
            return ProgramStatus.running;

        }
    }
}

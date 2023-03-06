using System.Collections.Generic;
using UnityEngine;

public class RobotMovementVisualiser : MonoBehaviour
{
    [SerializeField] private RobotController robotSimulation;
    [SerializeField] private LineRenderer robotPathPrefab;
     private LineRenderer robotPath;
    [SerializeField] private Rigidbody2D robotRB;


    private void Start()
    {
        robotPath = Instantiate(robotPathPrefab);
    }

    private void FixedUpdate()
    {
        //Show robot path
        //UpdatePath();

        if (Input.GetKeyUp(KeyCode.L))
            UpdatePath();

    }

    private void UpdatePath()
    {
        var simulation = AIProgramBackendManager.instance.GetActiveProgram();

        robotPath.positionCount = 10;

        if (simulation != null)
        {
            int count = 0;
            var node = simulation.currentNode;

            var positions = new List<Vector2>();

            var robotPosition = robotRB.position;

            while (node != null)
            {
                var command = node.GetCommand();
                var movement = (Vector2)GetForceFromCommand(command) * robotSimulation.speed * (1/robotRB.drag);
                robotPosition += movement;
                //robotPath.SetPosition(i, movement);
                positions.Add(robotPosition);

                node = node.GetNextNode();
                count++;
            }

            robotPath.positionCount = count;

            for (int i = 0; i < count; i++)
            {
                robotPath.SetPosition(i, positions[i]);
            }
        }
    }

    private Vector3 GetForceFromCommand(AICommand command)
    {
        switch (command)
        {
            case DirectionCommand:
                var c = (DirectionCommand)command;
                var f = robotRB.mass * (c.direction / c.maxTimer);
                return f;
            default:
                break;

        }
        return Vector3.zero;
    }

}

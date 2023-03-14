using System.Collections.Generic;
using UnityEngine;

public class RobotMovementVisualiser : MonoBehaviour
{
    [SerializeField] private RobotController robotSimulation;
    [SerializeField] private LineRenderer robotPath;
    [SerializeField] private Rigidbody2D robotRB;
    public static RobotMovementVisualiser instance;
    public bool updatePath = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        //Show robot path
        //UpdatePath();

        if (updatePath)
        {   
            robotPath.gameObject.SetActive(true);
            UpdatePath();
        }
        else
            robotPath.gameObject.SetActive(false);
    }

    private void UpdatePath()
    {
        //var simulation = AIProgramBackendManager.instance.GetActiveProgram();
        var simulation = AIProgram.activeProgram;

        robotPath.positionCount = 10;

        int maxDepth = 100;

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
                if (count > maxDepth)
                    break;
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
                var f = (1f/robotRB.mass) * (c.direction / c.maxTimer);
                return f;
            default:
                break;

        }
        return Vector3.zero;
    }

}

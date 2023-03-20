using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementVisualiser : MonoBehaviour
{
    [SerializeField] private RobotController robotSimulation;
    [SerializeField] private LineRenderer robotPath;
    [SerializeField] private Rigidbody2D robotRB;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    public static RobotMovementVisualiser instance;
    public bool updatePath = true;
    private List<Transform> pathPositions = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            for (int i = 0; i < 20; i++)
            {
                var p = new GameObject("PathPoint");
                p.hideFlags = HideFlags.HideInHierarchy;
                p.transform.position = Vector3.zero;
                pathPositions.Add(p.transform);
                targetGroup.AddMember(p.transform, 0, 4);
            }
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

        int maxDepth = 100;

        if (simulation != null)
        {
            int count = 0;
            var node = simulation.currentNode;

            //var positions = new List<Vector2>();

            var robotPosition = robotRB.position;

            while (node != null)
            {
                var command = node.GetCommand();
                var movement = (Vector2)GetForceFromCommand(command, count);
                robotPosition += movement;
                //robotPath.SetPosition(i, movement);
                pathPositions[count].transform.position = robotPosition;

                node = node.GetNextNode();
                count++;  
                if (count > maxDepth)
                    break;
            }

            robotPath.positionCount = count;

            for (int i = 0; i < count; i++)
            {
                robotPath.SetPosition(i, (Vector2)pathPositions[i].transform.position);
            }

        }

        UpdateCamera();
    }

    private Vector3 GetForceFromCommand(AICommand command, int index)
    {
        switch (command)
        {
            case DirectionCommand:
                var c = (DirectionCommand)command;
                var f = (1f/robotRB.mass) * (c.direction / c.maxTimer) * robotSimulation.speed * (1 / robotRB.drag);
                //targetGroup.FindMember(pathPositions[index].transform);
                targetGroup.m_Targets[index].weight = 4.0f;
                return f;
            default:
                targetGroup.m_Targets[index].weight = 0.0f;
                break;

        }
        return Vector3.zero;
    }

    void UpdateCamera()
    {
        var minBorders = new Vector2(float.MaxValue, float.MaxValue); // 
        var maxBorders = new Vector2(float.MinValue, float.MinValue); // 

        for (int i = 0; i < robotPath.positionCount; i++)
        {
            var pos = robotPath.GetPosition(i);

            minBorders =
                new Vector2(
                    pos.x < minBorders.x ? pos.x : minBorders.x,
                    pos.y < minBorders.y ? pos.y : minBorders.y
                    );

            maxBorders =
                new Vector2(
                    maxBorders.x < pos.x ? pos.x : maxBorders.x,
                    maxBorders.y < pos.y ? pos.y : maxBorders.y
                    );
        }

        var diff = maxBorders - minBorders;

        var orthoSize = Mathf.Max(diff.x, diff.y);

        //pathCamera.m_Lens.OrthographicSize = orthoSize;
        //robotPath.transform.position = robotSimulation.transform.position;
       // pathCamera.transform.position = robotSimulation.transform.position;
    }

}

using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RobotMovementVisualiser : MonoBehaviour
{
    [SerializeField] private LineRenderer robotPath;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    private RobotController robotController;
    private Rigidbody2D robotRB;
    public static RobotMovementVisualiser instance;
    private bool updatePath = false;
    private List<Transform> pathPositions = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        robotController = RobotController.instance;
        robotRB = robotController.gameObject.GetComponent<Rigidbody2D>();
        for (int i = 0; i < 100; i++)
        {
            var p = new GameObject("PathPoint");
            p.hideFlags = HideFlags.HideInHierarchy;
            p.transform.position = Vector3.zero;
            pathPositions.Add(p.transform);
            targetGroup.AddMember(p.transform, 0, 4);
        }
    }

    private void FixedUpdate()
    {
        //Show robot path
        //UpdatePath();

        if (robotPath.positionCount > 0)
        {
            var dist = (robotPath.GetPosition(0) - robotController.transform.position).magnitude;
            if (dist > 0.1f)
                UpdatePath();
        }
    }

    public void RenderPath(bool render)
    {
        robotPath.gameObject.SetActive(render);
    }

    public void UpdatePath()
    {
        robotPath.gameObject.SetActive(true);
        //var simulation = AIProgramBackendManager.instance.GetActiveProgram();
        var simulation = AIProgram.activeProgram;

        int maxDepth = AIProgram.activeProgram.maxCommands;

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
                //robotPath.SetPosition(i, movement);
                robotPosition += movement;
                pathPositions[count].transform.position = robotPosition;

                if(count == 0)
                    targetGroup.m_Targets[0].weight = 4.0f;

                node = node.GetNextNode();
                count++;  
                if (count >= maxDepth)
                    break;
            }

            robotPath.SetPosition(0, pathPositions[0].transform.position);

            float threshold = 0.5f;
            int index = 1;

            robotPath.positionCount = 1;

            for (int i = 1; i < maxDepth; i++)
            {
                targetGroup.m_Targets[i].weight = 0.0f;
            }

            for (int i = 1; i < count; i++)
            {
                if ((pathPositions[i].transform.position - pathPositions[i - 1].transform.position).magnitude > threshold)
                {
                    robotPath.positionCount = index + 1;
                    robotPath.SetPosition(index++, pathPositions[i].transform.position);
                    targetGroup.m_Targets[i].weight = 4.0f;
                }
                else
                {
                    targetGroup.m_Targets[i].weight = 0.0f;
                }
            }

            //robotPath.positionCount = count;

            //for (int i = 0; i < count; i++)
            //{
            //    robotPath.SetPosition(i, (Vector2)pathPositions[i].transform.position);
            //}

        }

        UpdateCamera();
    }

    private Vector3 GetForceFromCommand(AICommand command, int index)
    {
        switch (command)
        {
            case DirectionCommand:
                var c = (DirectionCommand)command;
                var f = (1f/robotRB.mass) * (c.direction / c.maxTimer) * robotController.speed * (1 / robotRB.drag);
                //targetGroup.FindMember(pathPositions[index].transform);
                return f;
            default:
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

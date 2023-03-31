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
            p.transform.position = Vector2.zero;
            pathPositions.Add(p.transform);
            targetGroup.AddMember(p.transform, 0, 4);
        }
    }

    private void FixedUpdate()
    {
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
        if (AIProgram.activeProgram == null) return;

        robotPath.gameObject.SetActive(true);

        var simulation = AIProgram.activeProgram;
        int maxDepth = AIProgram.activeProgram.maxCommands;

        int count = 0;
        var robotPosition = robotRB.position;
        var node = simulation.currentNode;

        while (node != null && count < maxDepth)
        {
            var command = node.GetCommand();
            var movement = (Vector2)GetForceFromCommand(command, count);
            robotPosition += movement;
            pathPositions[count].position = robotPosition;

            if (count == 0)
                targetGroup.m_Targets[0].weight = 4.0f;

            node = node.GetNextNode();
            count++;
        }

        robotPath.SetPosition(0, pathPositions[0].position);

        float threshold = 0.5f;

        int index = 1;


        for (int i = 1; i < maxDepth; i++)
        {
            targetGroup.m_Targets[i].weight = 0.0f;
        }

        for (int i = 1; i < count; i++)
        {
            var dist = (pathPositions[i].position - pathPositions[i - 1].position).magnitude;
            if (dist > threshold)
            {
                robotPath.positionCount = index + 1;
                robotPath.SetPosition(index++, pathPositions[i].position);
                targetGroup.m_Targets[i].weight = 4.0f;
            }
            else
            {
                targetGroup.m_Targets[i].weight = 0.0f;
            }
        }

    }

    private Vector3 GetForceFromCommand(AICommand command, int index)
    {
        switch (command)
        {
            case DirectionCommand:
                var c = (DirectionCommand)command;
                var f = (1f / robotRB.mass) * (c.direction / c.maxTimer) * robotController.speed * (1 / robotRB.drag);
                return f;
            default:
                break;

        }
        return Vector3.zero;
    }
}

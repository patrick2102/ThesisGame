using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProgramFrontendManager : MonoBehaviour
{

    public enum FrontEndStates
    {
        connectingNodes, addingCommand, none
    }

    public FrontEndStates currentState = FrontEndStates.none;

    private List<(Vector2, Vector2)> activeConnections = new List<(Vector2, Vector2)>();

    public KeyCode openUIButton;

    public NodeScript mousedOverNode;
    public NodeScript selectedNode;

    public Vector2 startConnectionPos;

    public static AIProgramFrontendManager instance; // Instance used to ensure singleton behavior.

    private Camera cam;

    [SerializeField] private GameObject commandList;

    private void Awake()
    {
        cam = Camera.main;
        if (instance == null)
        {
            commandList.SetActive(false);
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(openUIButton))
        {

        }
    }

    private void FixedUpdate()
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            //Debug.Log("Line start: " + startConnectionPos);
            //Debug.Log("Line end: " + (Vector2)cam.ScreenToWorldPoint(Input.mousePosition));
            Debug.DrawLine(startConnectionPos, (Vector2)cam.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }

        foreach (var c in activeConnections)
        {
            Debug.DrawLine(c.Item1, c.Item2, Color.green);
        }

    }

    public void SetMousedOverNode(NodeScript node)
    {
        mousedOverNode = node;
    }

    public void LeftClickNode(NodeScript node)
    {
        if (currentState == FrontEndStates.none)
        {
            StartConnectingNodes(node);
        }
    }

    public void UnclickNode(NodeScript node)
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            currentState = FrontEndStates.none;

            if (node.neighbours.Contains(mousedOverNode))
            {
                ConnectNodes(node);
            }
        }
    }

    public void RightClickNode(NodeScript node)
    {
        if (currentState == FrontEndStates.none)
        {
            AddCommand(node);
        }
        else if (currentState == FrontEndStates.addingCommand)
        {
            currentState = FrontEndStates.none;
            selectedNode = null;

            commandList.SetActive(false);
        }
    }

    void StartConnectingNodes(NodeScript node)
    {
        startConnectionPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentState = FrontEndStates.connectingNodes;
    }

    void ConnectNodes(NodeScript node)
    {
        currentState = FrontEndStates.none;

        var connectPoint = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        activeConnections.Add((startConnectionPos, connectPoint));

        mousedOverNode.command.SetNext(node.command);
        node.command.SetPrev(mousedOverNode.command);
    }

    void AddCommand(NodeScript node)
    {
        currentState = FrontEndStates.addingCommand;
        selectedNode = node;

        commandList.SetActive(true);
    }

    public void SelectCommand(CommandButton commandButton)
    {
        currentState = FrontEndStates.none;

        var nextCommand = selectedNode.command.Next();
        var prevCommand = selectedNode.command.Prev();

        selectedNode.command = commandButton.command;

        selectedNode.command.SetNext(nextCommand);
        selectedNode.command.SetPrev(prevCommand);
        prevCommand.SetNext(selectedNode.command);

        selectedNode.nodeText.text = commandButton.commandButtonText.text;

        selectedNode = null;
        commandList.SetActive(false);
    }
}

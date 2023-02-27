using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProgramFrontendManager : MonoBehaviour
{

    public enum FrontEndStates
    {
        connectingNodes, addingCommand, open, closed
    }

    public FrontEndStates currentState = FrontEndStates.closed;
    public FrontEndStates prevState = FrontEndStates.open;

    private List<(Vector2, Vector2)> activeConnections = new List<(Vector2, Vector2)>();

    public KeyCode openUIButton;

    public CircuitNode selectedNode;
    public AICommand selectedCommand;

    public Vector2 startConnectionPos;

    public static AIProgramFrontendManager instance; // Instance used to ensure singleton behavior.

    private Camera cam;

    [SerializeField] private GameObject commandList;
    [SerializeField] private GameObject nodesView;

    private void Awake()
    {
        cam = Camera.main;
        if (instance == null)
        {
            commandList.SetActive(false);
            nodesView.SetActive(false);
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
            if(currentState != FrontEndStates.closed)
            {
                commandList.SetActive(false);
                nodesView.SetActive(false);
                currentState = FrontEndStates.closed;
            }
            if (currentState == FrontEndStates.closed)
            {
                commandList.SetActive(false);
                nodesView.SetActive(true);
                currentState = FrontEndStates.open;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            Debug.DrawLine(startConnectionPos, (Vector2)cam.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }

        foreach (var c in activeConnections)
        {
            Debug.DrawLine(c.Item1, c.Item2, Color.green);
        }

    }

    public void SetMousedOverNode(CircuitNode node)
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            selectedNode = node;
            //Debug.Log(selectedNode);
            if(node != null)
                selectedCommand = node.command;
        }
    }

    public void LeftClickNode()
    {
        if (currentState == FrontEndStates.open)
        {
            startConnectionPos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentState = FrontEndStates.connectingNodes;
        }
    }

    public void UnclickNode(CircuitNode node)
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            currentState = FrontEndStates.open;

            if (node.neighbours.Contains(selectedNode))
            {
                ConnectNodes(node.command);
            }
        }
    }

    public void RightClickNode(CircuitNode node)
    {
        if (currentState == FrontEndStates.open)
        {
            OpenCommandScreen(node);
        }
        else if (currentState == FrontEndStates.addingCommand)
        {
            currentState = FrontEndStates.open;
            selectedNode = null;

            commandList.SetActive(false);
        }
    }

    void ConnectNodes(IAICommand command)
    {
        currentState = FrontEndStates.open;

        //FIXME temporary debug drawline to show nodes are 
        var connectPoint = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        activeConnections.Add((startConnectionPos, connectPoint));

        command.ConnectCommands(selectedCommand);
    }

    void OpenCommandScreen(CircuitNode node)
    {
        currentState = FrontEndStates.addingCommand;
        selectedNode = node;
        selectedCommand = node.command;

        commandList.SetActive(true);
    }

    public void SelectCommand(CommandButton commandButton)
    {
        currentState = FrontEndStates.open;

        var nextCommand = selectedNode.command.Next();
        var prevCommand = selectedNode.command.Prev();

        Destroy(selectedNode.command.transform.gameObject);

        selectedNode.command = Instantiate(commandButton.command);
        selectedNode.command.transform.SetParent(selectedNode.transform);

        selectedNode.command.ConnectCommands(nextCommand);
        prevCommand.ConnectCommands(selectedNode.command);

        selectedNode.nodeText.text = commandButton.commandButtonText.text;

        selectedNode = null;
        //selectedCommand = null;
        commandList.SetActive(false);
    }
}

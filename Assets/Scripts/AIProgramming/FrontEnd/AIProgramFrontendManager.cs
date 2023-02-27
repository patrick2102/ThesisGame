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

    public CircuitNode selectedNode;
    public AICommand selectedCommand;

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
        if (currentState == FrontEndStates.none)
        {
            startConnectionPos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentState = FrontEndStates.connectingNodes;
        }
    }

    public void UnclickNode(CircuitNode node)
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            currentState = FrontEndStates.none;

            if (node.neighbours.Contains(selectedNode))
            {
                ConnectNodes(node.command);
            }
        }
    }

    public void RightClickNode(CircuitNode node)
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

    void ConnectNodes(IAICommand command)
    {
        currentState = FrontEndStates.none;

        //FIXME temporary debug drawline to show nodes are 
        var connectPoint = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        activeConnections.Add((startConnectionPos, connectPoint));

        //node.ConnectTo(selectedNode);

        command.ConnectCommands(selectedCommand);
    }

    void AddCommand(CircuitNode node)
    {
        currentState = FrontEndStates.addingCommand;
        selectedNode = node;
        selectedCommand = node.command;

        commandList.SetActive(true);
    }

    public void SelectCommand(CommandButton commandButton)
    {
        currentState = FrontEndStates.none;

        var nextCommand = selectedNode.command.Next();
        var prevCommand = selectedNode.command.Prev();

        Destroy(selectedNode.command.transform.gameObject);

        selectedNode.command = Instantiate(commandButton.command);
        selectedNode.command.transform.SetParent(selectedNode.transform);

        selectedNode.command.ConnectCommands(nextCommand);
        prevCommand.ConnectCommands(selectedCommand);

        //TODO add insertion of node between two nodes

        //selectedNode.command.SetNext(nextCommand);
        //selectedNode.command.SetPrev(prevCommand);

        //prevCommand.SetNext(selectedNode.command);
        //nextCommand.SetPrev(selectedNode.command);

        selectedNode.nodeText.text = commandButton.commandButtonText.text;

        selectedNode = null;
        commandList.SetActive(false);
    }
}

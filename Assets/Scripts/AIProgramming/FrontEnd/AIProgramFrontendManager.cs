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

    public CircuitNode mousedOverNode;
    public CircuitNode selectedNode;

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

    public void SetMousedOverNode(CircuitNode node)
    {
        mousedOverNode = node;
    }

    public void LeftClickNode(CircuitNode node)
    {
        if (currentState == FrontEndStates.none)
        {
            StartConnectingNodes(node);
        }
    }

    public void UnclickNode(CircuitNode node)
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

    void StartConnectingNodes(CircuitNode node)
    {
        startConnectionPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentState = FrontEndStates.connectingNodes;
    }

    void ConnectNodes(CircuitNode node)
    {
        currentState = FrontEndStates.none;

        var connectPoint = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        activeConnections.Add((startConnectionPos, connectPoint));

        node.command.SetNext(mousedOverNode.command);
        mousedOverNode.command.SetPrev(node.command);
    }

    void AddCommand(CircuitNode node)
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

        Destroy(selectedNode.command.transform.gameObject);

        selectedNode.command = Instantiate(commandButton.command);
        selectedNode.command.transform.SetParent(selectedNode.transform);


        selectedNode.command.SetNext(nextCommand);
        selectedNode.command.SetPrev(prevCommand);
        prevCommand.SetNext(selectedNode.command);
        nextCommand.SetPrev(selectedNode.command);

        selectedNode.nodeText.text = commandButton.commandButtonText.text;

        selectedNode = null;
        commandList.SetActive(false);
    }
}

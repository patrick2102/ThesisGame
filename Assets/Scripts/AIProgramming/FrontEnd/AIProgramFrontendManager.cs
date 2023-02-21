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

    public NodeScript selectedNode;

    public Vector2 startConnectionPos;

    public static AIProgramFrontendManager instance; // Instance used to ensure singleton behavior.

    public bool drawConnection = false;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        if (instance == null)
            instance = this;
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

    void CreateCurcuitBoard(CircuitBoardData cbd)
    {

    }

    public void LeftClickNode(NodeScript node)
    {
        if (currentState == FrontEndStates.none)
        {
            StartConnectingNodes(node);
        }
        else if (currentState == FrontEndStates.connectingNodes)
        {
            if (node.Equals(selectedNode))
            {
                currentState = FrontEndStates.none;
                selectedNode = null;
                
            }
            else if (node.neighbours.Contains(selectedNode))
            {
                ConnectNodes(node);
            }
            else
            {
                Debug.Log("Invalid connection");
            }
        }
    }

    public void RightClickNode(NodeScript node)
    {
        if (currentState == FrontEndStates.none)
        {
            AddCommand();
        }
    }

    void StartConnectingNodes(NodeScript node)
    {
        selectedNode = node;
        startConnectionPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentState = FrontEndStates.connectingNodes;
    }

    void ConnectNodes(NodeScript node)
    {
        currentState = FrontEndStates.none;

        var connectPoint = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);

        activeConnections.Add((startConnectionPos, connectPoint));

        //throw new NotImplementedException();
    }

    void AddCommand()
    {
        currentState = FrontEndStates.addingCommand;



        throw new NotImplementedException();
    }


}

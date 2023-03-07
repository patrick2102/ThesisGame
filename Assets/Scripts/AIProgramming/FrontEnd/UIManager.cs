using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private KeyCode openUIButton;
    [SerializeField] private GameObject commandView;
    [SerializeField] private GameObject nodesView;

    private CircuitNode selectedNode;
    private Vector2 startConnectionPos;

    private enum FrontEndStates
    {
        connectingNodes, addingCommand, open, closed
    }
    private FrontEndStates currentState = FrontEndStates.closed;

    public static UIManager instance; // Instance used to ensure singleton behavior.

    private void Awake()
    {
        if (instance == null)
        {
            currentState = FrontEndStates.closed;
            commandView.SetActive(false);
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
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        if (currentState != FrontEndStates.closed)
        {
            commandView.SetActive(false);
            nodesView.SetActive(false);
            currentState = FrontEndStates.closed;
        }
        else if (currentState == FrontEndStates.closed)
        {
            commandView.SetActive(false);
            nodesView.SetActive(true);
            currentState = FrontEndStates.open;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            Debug.DrawLine(startConnectionPos, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }

        //foreach (var a in nodesWithActiveOutputs)
        //{
        //    var c = a.Value;
        //    Debug.DrawLine(c.Item1, c.Item2, Color.green);
        //}

    }

    public void SetMousedOverNode(CircuitNode node)
    {
        if (currentState == FrontEndStates.connectingNodes)
        {
            selectedNode = node;
        }
    }

    public void LeftClickNode()
    {
        if (currentState == FrontEndStates.open)
        {
            startConnectionPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                ConnectNodes(node);
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

            commandView.SetActive(false);
        }
    }

    private void ConnectNodes(CircuitNode node)
    {
        currentState = FrontEndStates.open;

        //FIXME temporary debug drawline to show nodes are connected 
        //var connectPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //nodesWithActiveOutputs[node] = (startConnectionPos, connectPoint);

        node.SetNextNode(selectedNode);
    }

    private void OpenCommandScreen(CircuitNode node)
    {
        currentState = FrontEndStates.addingCommand;
        selectedNode = node;

        commandView.SetActive(true);
    }

    public void ChangeCommand(CommandButton commandButton)
    {
        currentState = FrontEndStates.open;

        selectedNode.ChangeCommand(commandButton.command);

        selectedNode = null;
        commandView.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private KeyCode openUIButton;
    [SerializeField] private GameObject menuView;
    [SerializeField] private GameObject commandView;
    [SerializeField] private GameObject nodesView;
    [SerializeField] private GameObject interactView;

    private CircuitNode selectedNode;
    private Vector2 startConnectionPos;

    public enum UIState
    {
        connectingNodes, commandScreen, nodeScreen, closed, menuScreen
    }

    private UIState currentState = UIState.closed;
    private UIState prevState = UIState.closed;

    public static UIManager instance; // Instance used to ensure singleton behavior.

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
        currentState = UIState.closed;
        menuView.SetActive(false);
        commandView.SetActive(false);
        nodesView.SetActive(false);
        interactView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(openUIButton))
        //{
        //    SetUI(UIState.nodeScreen);
        //}
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SetUI(UIState.menuScreen);
        }
    }

    private UIState GetNewUIState(UIState newState)
    {
        var state = currentState;
        currentState = (currentState == newState ? UIState.closed : newState);
        prevState = state;
        return currentState;
    }

    public void CloseUI()
    {
        SetUI(UIState.closed);
    }

    public void SetUI(UIState state)
    {
        var newState = GetNewUIState(state);

        if (newState == UIState.nodeScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(false);
            nodesView.SetActive(true);
            interactView.SetActive(false);
        }
        else if (newState == UIState.commandScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(true);
            nodesView.SetActive(true);
            interactView.SetActive(false);
        }

        else if (newState == UIState.menuScreen)
        {
            menuView.SetActive(true);
            commandView.SetActive(false);
            nodesView.SetActive(false);
            interactView.SetActive(false);
        }

        else if (newState == UIState.closed)
        {
            menuView.SetActive(false);
            commandView.SetActive(false);
            nodesView.SetActive(false);
            interactView.SetActive(false);

        }
        currentState = newState;
    }

    private void FixedUpdate()
    {
        if (currentState == UIState.connectingNodes)
        {
            Debug.DrawLine(startConnectionPos, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }
    }

    public void SetMousedOverNode(CircuitNode node)
    {
        if (currentState == UIState.connectingNodes)
        {
            selectedNode = node;
        }
    }

    public void LeftClickNode()
    {
        if (currentState == UIState.nodeScreen)
        {
            startConnectionPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentState = UIState.connectingNodes;
        }
    }

    public void SetInteractScreen(bool active)
    {
        interactView.SetActive(active);
    }

    public void UnclickNode(CircuitNode node)
    {
        if (currentState == UIState.connectingNodes)
        {
            currentState = UIState.nodeScreen;

            if (node.neighbours.Contains(selectedNode))
            {
                ConnectNodes(node);
            }
        }
    }

    public void RightClickNode(CircuitNode node)
    {
        if (currentState == UIState.nodeScreen)
        {
            OpenCommandScreen(node);
        }
        else if (currentState == UIState.commandScreen)
        {
            currentState = UIState.nodeScreen;
            selectedNode = null;

            commandView.SetActive(false);
        }
    }

    private void ConnectNodes(CircuitNode node)
    {
        currentState = UIState.nodeScreen;

        //FIXME temporary debug drawline to show nodes are connected 

        node.SetNextNode(selectedNode);
    }

    private void OpenCommandScreen(CircuitNode node)
    {
        //currentState = UIState.commandScreen;
        selectedNode = node;

        SetUI(UIState.commandScreen);
    }

    public void ChangeCommand(CommandButton commandButton)
    {
        //currentState = UIState.nodeScreen;

        selectedNode.ChangeCommand(commandButton.command);

        selectedNode = null;
        SetUI(UIState.nodeScreen);
        //commandView.SetActive(false);
    }
}

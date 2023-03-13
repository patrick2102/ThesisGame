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
        connectingNodes, commandScreen, nodeScreen, closed, menuScreen, interactScreen
    }

    private UIState currentState = UIState.closed;

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
        SetUI(UIState.closed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Menu should open");
            SetUI(UIState.menuScreen);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            PrintCurrentState();
        }
    }

    public void CloseUI()
    {
        SetUI(UIState.closed);
    }

    public void SetUI(UIState newState)
    {
        currentState = (currentState == newState ? UIState.closed : newState);

        if (currentState == UIState.nodeScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(false);
            nodesView.SetActive(true);
            interactView.SetActive(false);
        }
        else if (currentState == UIState.commandScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(true);
            nodesView.SetActive(true);
            interactView.SetActive(false);
        }

        else if (currentState == UIState.menuScreen)
        {
            menuView.SetActive(true);
            commandView.SetActive(false);
            nodesView.SetActive(false);
            interactView.SetActive(false);
        }

        else if (currentState == UIState.closed)
        {
            menuView.SetActive(false);
            commandView.SetActive(false);
            nodesView.SetActive(false);
            interactView.SetActive(false);

        }
        else if (currentState == UIState.interactScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(false);
            nodesView.SetActive(false);
            interactView.SetActive(true);
        }
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

        selectedNode.ChangeCommand(commandButton.command);

        selectedNode = null;
        SetUI(UIState.nodeScreen);
    }

    public void PrintCurrentState()
    {
        Debug.Log(currentState);
    }
}

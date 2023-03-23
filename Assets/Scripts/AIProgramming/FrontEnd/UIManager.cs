using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private KeyCode openUIButton;
    [SerializeField] private GameObject menuView;
    [SerializeField] private GameObject commandView;
    [SerializeField] private GameObject nodesView;
    [SerializeField] private GameObject interactView;
    [SerializeField] private Canvas canvas;
    [SerializeField] private LineRenderer startConnectionLinePrefab;
    [SerializeField] private LineRenderer connectionLinePrefab;
    private LineRenderer startConnectionLine;
    private LineRenderer connectionLine;

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
        canvas.worldCamera = Camera.main;
        startConnectionLine = Instantiate(startConnectionLinePrefab);
        connectionLine = Instantiate(connectionLinePrefab);
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

        if (currentState == UIState.connectingNodes)
        {
            //Debug.DrawLine(startConnectionPos, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
            startConnectionLine.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (currentState == UIState.nodeScreen || currentState == UIState.commandScreen)
        {
            connectionLine.gameObject.SetActive(true);
            UpdateConnectionLine();
        }
        else
        {
            connectionLine.gameObject.SetActive(false);
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
            GameManager.instance.ChangeView(GameManager.CameraState.pathCamera);
        }
        else if (currentState == UIState.commandScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(true);
            nodesView.SetActive(true);
            interactView.SetActive(false);
            GameManager.instance.ChangeView(GameManager.CameraState.pathCamera);
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
            GameManager.instance.ChangeView(GameManager.CameraState.playerCamera);
        }
        else if (currentState == UIState.interactScreen)
        {
            menuView.SetActive(false);
            commandView.SetActive(false);
            nodesView.SetActive(false);
            interactView.SetActive(true);
            GameManager.instance.ChangeView(GameManager.CameraState.playerCamera);
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
            //startConnectionLine.gameObject.SetActive(true);
            startConnectionLine.SetPosition(0, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //startConnectionPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            else
            {
                DisconnectNodes(node);
            }
        }
        startConnectionLine.SetPosition(0, Vector2.zero);
        startConnectionLine.SetPosition(1, Vector2.zero);
        //startConnectionLine.gameObject.SetActive(false);
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
        node.SetNextNode(selectedNode);
        RobotMovementVisualiser.instance.UpdatePath();
        UpdateConnectionLine();
    }

    private void DisconnectNodes(CircuitNode node)
    {
        currentState = UIState.nodeScreen;
        node.RemoveNextNode();
        RobotMovementVisualiser.instance.UpdatePath();
        UpdateConnectionLine();
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
        RobotMovementVisualiser.instance.UpdatePath();
    }

    public void PrintCurrentState()
    {
        Debug.Log(currentState);
    }

    private void UpdateConnectionLine()
    {
        int maxDepth = 100;

        int count = 0;
        var node = NodesScreen.instance.inputNode;

        var positions = new List<Vector2>();

        while (node != null && count < maxDepth)
        {
            var pos = node.transform.position;

            positions.Add(pos);

            node = node.GetNextNode();

            count++;
        }

        connectionLine.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            connectionLine.SetPosition(i, positions[i]);
        }
    }
}

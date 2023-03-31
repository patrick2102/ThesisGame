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
        connectingNodes, nodeScreen, closed, menuScreen, interactScreen
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
            startConnectionLine.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (currentState == UIState.nodeScreen)
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

        UpdateUIVisibility();
        UpdateCameraView();
    }

    private void UpdateUIVisibility()
    {
        menuView.SetActive(currentState == UIState.menuScreen);
        commandView.SetActive(currentState == UIState.nodeScreen);
        nodesView.SetActive(currentState == UIState.nodeScreen);
        interactView.SetActive(currentState == UIState.interactScreen);
    }

    private void UpdateCameraView()
    {
        if (currentState == UIState.nodeScreen)
        {
            GameManager.instance.ChangeView(GameManager.CameraState.pathCamera);
        }
        else if (currentState == UIState.interactScreen || currentState == UIState.closed)
        {
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
            startConnectionLine.SetPosition(0, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class NodesScreen : MonoBehaviour
{
    public CircuitNode inputNodePrefab;
    public CircuitNode outputNodePrefab;
    public CircuitNode circuitNodePrefab;

    public CircuitNode[,] circuitNodes;

    public NodeType[,] gridRepresentation;

    public static NodesScreen instance;

    public RectTransform screen;

    private AIProgram program;


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        screen = gameObject.GetComponent<RectTransform>();

        gridRepresentation = CircuitInterpreter.ReadString();

        circuitNodes = new CircuitNode[gridRepresentation.GetLength(0), gridRepresentation.GetLength(1)];

        GenerateCircuit(gridRepresentation);
    }

    private void GenerateCircuit(NodeType[,] gridRepresentation)
    {
        var stepSizeX = (screen.rect.width) / (gridRepresentation.GetLength(1));
        // Hardcoded a minus value to the height to make some room for run button at the bottom
        var stepSizeY = (screen.rect.height - 20.0f) / (gridRepresentation.GetLength(0));

        // Decreased stepsize further to make room for run button in at the bottom of screen
        stepSizeY -= 15.0f;

        var stepSize = new Vector2(stepSizeX, stepSizeY);

        var border = stepSize / 2;

        // Added a value to raise the point from which the circuit board is drawn, to make room for the rum button
        border.y += 80.0f;

        int idCounter = 0;

        //Instantiate nodes and add to circuitGrid:
        for (int i = 0; i < gridRepresentation.GetLength(0); i++)
        {
            for (int j = 0; j < gridRepresentation.GetLength(1); j++)
            {
                var node = GridRepresentationInterpreter(gridRepresentation[i, j]);

                if (node != null)
                {
                    var offSet = border + (stepSize * new Vector2(j, gridRepresentation.GetLength(0) - i - 1));

                    node.transform.SetParent(transform);
                    node.transform.position = offSet;
                    node.id = idCounter++;
                }
                if (gridRepresentation[i,j] == NodeType.InputNode)
                {

                    program = new AIProgram(node);
                }
                circuitNodes[i,j] = node;
            }
        }

        //Create connection between nodes:
        for (int i = 0; i < gridRepresentation.GetLength(0); i++)
        {
            for (int j = 0; j < gridRepresentation.GetLength(1); j++)
            {
                CircuitNode rightNode = null;
                CircuitNode downNode = null;

                if (i < gridRepresentation.GetLength(0) - 1)
                    rightNode = circuitNodes[i + 1, j];

                if (j < gridRepresentation.GetLength(1) - 1)
                    downNode = circuitNodes[i, j + 1];

                if (rightNode != null && circuitNodes[i + 1, j] != null && circuitNodes[i, j] != null)
                {
                    circuitNodes[i, j].neighbours.Add(circuitNodes[i + 1, j]);
                    circuitNodes[i + 1, j].neighbours.Add(circuitNodes[i, j]);
                }
                if (downNode != null && circuitNodes[i, j + 1] != null && circuitNodes[i, j] != null)
                {
                    circuitNodes[i, j].neighbours.Add(circuitNodes[i, j + 1]);
                    circuitNodes[i, j + 1].neighbours.Add(circuitNodes[i, j]);
                }
            }
        }
        AIProgramBackendManager.instance.SetActiveProgram(program);
    }

    private CircuitNode GridRepresentationInterpreter(NodeType node)
    {
        switch (node)
        {
            case NodeType.CircuitNode:
                return Instantiate(circuitNodePrefab);
            case NodeType.OutputNode:
                return Instantiate(outputNodePrefab);
            case NodeType.InputNode:
                return Instantiate(inputNodePrefab);
        }

        return null;
    }
}

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class NodesScreen : MonoBehaviour
{
    public Vector2 grideSize;

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
        var bottomRight = screen.rect.max;
        var topLeft = screen.rect.min;

        //var stepSizeX = gridRepresentation.GetLength(0) / (screen.rect.max.x - screen.rect.min.x);
        //var stepSizeY = gridRepresentation.GetLength(1) / (screen.rect.max.y - screen.rect.min.y);
        var stepSizeX =  (screen.rect.width) / gridRepresentation.GetLength(0);
        var stepSizeY = (screen.rect.height) / gridRepresentation.GetLength(1);

        var stepSize = new Vector2(stepSizeX, stepSizeY);

        var border = stepSize / 2;

        int idCounter = 0;

        //Instantiate nodes and add to circuitGrid:
        for (int i = 0; i < gridRepresentation.GetLength(0); i++)
        {
            for (int j = 0; j < gridRepresentation.GetLength(1); j++)
            {
                var node = GridRepresentationInterpreter(gridRepresentation[i, j]);

                if (node != null)
                {
                    var offSet = border + (stepSize * new Vector2(i, j));

                    node.transform.SetParent(transform);
                    node.transform.position = offSet;
                    node.id = idCounter++;
                }
                if(node == inputNodePrefab)
                {
                    program = new AIProgram(node.command);
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


        //Instantiate Input and output nodes
        //var inputNode = Instantiate(inputNodePrefab);
        //inputNode.transform.SetParent(transform);
        //var offSetInput = Vector3.up * (border.y / 2);

        //inputNode.transform.position += circuitNodes[0, gridRepresentation.GetLength(1) - 1].transform.position + offSetInput;
        //inputNode.id = idCounter++;


        //var outNode = Instantiate(outputNodePrefab);
        //outNode.transform.SetParent(transform);
        //var offSetOutput = Vector3.up * (border.y / 2);
        //outNode.transform.position += circuitNodes[gridRepresentation.GetLength(0)-1, 0].transform.position - offSetOutput;


        //circuitNodes[0, gridRepresentation.GetLength(1)-1].neighbours.Add(inputNode);
        //inputNode.neighbours.Add(circuitNodes[0, gridRepresentation.GetLength(1) - 1]);


        //circuitNodes[gridRepresentation.GetLength(0) - 1, 0].neighbours.Add(outNode);
        //outNode.neighbours.Add(circuitNodes[gridRepresentation.GetLength(0) - 1, 0]);

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

    //private void CreateNeighbours(int x, int y)
    //{
    //    //Check neighbours for possible connections:

    //    var node = circuitNodes[x, y];


    //    if
    //    var leftNode = circuitNodes[x-1, y];
    //    var rightNode = circuitNodes[x+1, y];
    //    var upNode = circuitNodes[x, y+1];
    //    var downNode = circuitNodes[x, y-1];

    //    if (leftNode != null)
    //    {
    //        circuitNodes[x, y].neighbours.Add(circuitNodes[x - 1, y]);
    //    }
    //    if (rightNode != null)
    //    {
    //        circuitNodes[x, y].neighbours.Add(circuitNodes[x + 1, y]);

    //    }
    //    if (upNode != null)
    //    {
    //        circuitNodes[x, y].neighbours.Add(circuitNodes[x, y + 1]);

    //    }
    //    if (downNode != null)
    //    {
    //        circuitNodes[x, y].neighbours.Add(circuitNodes[x, y - 1]);

    //    }
    //}

}

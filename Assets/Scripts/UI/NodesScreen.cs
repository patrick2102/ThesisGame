using UnityEngine;

public class NodesScreen : MonoBehaviour
{
    public CircuitNode inputNodePrefab;
    public CircuitNode outputNodePrefab;
    public CircuitNode circuitNodePrefab;

    public CircuitNode inputNode;
    public RectTransform screen;

    public static NodesScreen instance;

    private CircuitNode[,] circuitNodes;

    private void Awake()
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

        GenerateCircuit();
    }

    private void FixedUpdate()
    {
        SetNodePositions();
    }

    private void SetNodePositions()
    {
        var stepSizeX = (screen.rect.width) / (circuitNodes.GetLength(0));
        var stepSizeY = (screen.rect.height - 20.0f) / (circuitNodes.GetLength(1));

        // Decreased stepsize further to make room for run button in at the bottom of screen
        stepSizeY -= 15.0f;

        var stepSize = new Vector2(stepSizeX, stepSizeY);

        var border = stepSize / 2;

        // Added a value to raise the point from which the circuit board is drawn, to make room for the run button
        border.y += 80.0f;

        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {
                var node = circuitNodes[i, j];

                if (node != null)
                {
                    var offSet = border + (stepSize * new Vector2(i, circuitNodes.GetLength(1) - j - 1));
                    node.transform.localScale = new Vector3(1, 1, 1);
                    node.GetComponent<RectTransform>().anchoredPosition = offSet;
                }
            }
        }

    }

    private void GenerateCircuit()
    {
        var gridRepresentation = CircuitInterpreter.ReadString();
        circuitNodes = new CircuitNode[gridRepresentation.GetLength(0), gridRepresentation.GetLength(1)];

        int idCounter = 0;

        //Instantiate nodes and add to circuitGrid:
        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {
                var node = GridRepresentationInterpreter(gridRepresentation[i, j]);

                if (node != null)
                {
                    node.transform.SetParent(transform);
                    node.id = idCounter++;
                }
                if (gridRepresentation[i, j] == NodeType.InputNode)
                {
                    inputNode = node;
                }
                circuitNodes[i, j] = node;
            }
        }

        SetNodePositions();

        ConnectNodesNeighborsOnly();
        //ConnectNodesColumns();

        ////Create connection between nodes:
        //for (int i = 0; i < circuitNodes.GetLength(0); i++)
        //{
        //    for (int j = 0; j < circuitNodes.GetLength(1); j++)
        //    {
        //        CircuitNode rightNode = null;
        //        CircuitNode downNode = null;

        //        if (i < circuitNodes.GetLength(0) - 1)
        //            rightNode = circuitNodes[i + 1, j];

        //        if (j < circuitNodes.GetLength(1) - 1)
        //            downNode = circuitNodes[i, j + 1];

        //        if (rightNode != null && circuitNodes[i + 1, j] != null && circuitNodes[i, j] != null)
        //        {
        //            circuitNodes[i, j].neighbours.Add(circuitNodes[i + 1, j]);
        //            circuitNodes[i + 1, j].neighbours.Add(circuitNodes[i, j]);
        //        }
        //        if (downNode != null && circuitNodes[i, j + 1] != null && circuitNodes[i, j] != null)
        //        {
        //            circuitNodes[i, j].neighbours.Add(circuitNodes[i, j + 1]);
        //            circuitNodes[i, j + 1].neighbours.Add(circuitNodes[i, j]);
        //        }
        //    }
        //}
    }

    private void ConnectNodesNeighborsOnly()
    {
        //Create connection between nodes:
        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {
                CircuitNode downNode = null;
                CircuitNode rightNode = null;

                if (i < circuitNodes.GetLength(0) - 1)
                    downNode = circuitNodes[i + 1, j];

                if (j < circuitNodes.GetLength(1) - 1)
                    rightNode = circuitNodes[i, j + 1];

                if (downNode != null && circuitNodes[i + 1, j] != null && circuitNodes[i, j] != null)
                {
                    circuitNodes[i, j].neighbours.Add(circuitNodes[i + 1, j]);
                    circuitNodes[i + 1, j].neighbours.Add(circuitNodes[i, j]);
                }
                if (rightNode != null && circuitNodes[i, j + 1] != null && circuitNodes[i, j] != null)
                {
                    circuitNodes[i, j].neighbours.Add(circuitNodes[i, j + 1]);
                    circuitNodes[i, j + 1].neighbours.Add(circuitNodes[i, j]);
                }
            }
        }
    }

    private void ConnectNodesColumns()
    {
        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {
                CircuitNode downNode = null;

                if (j < circuitNodes.GetLength(1) - 1)
                    downNode = circuitNodes[i, j + 1];


                if (downNode != null && circuitNodes[i, j + 1] != null && circuitNodes[i, j] != null)
                {
                    circuitNodes[i, j].neighbours.Add(circuitNodes[i, j + 1]);
                    circuitNodes[i, j + 1].neighbours.Add(circuitNodes[i, j]);
                }

                //for (int k = 0; k < circuitNodes.GetLength(1); k++)
                //{
                //    CircuitNode rightNode = null;
                //    if (i < circuitNodes.GetLength(0) - 1)
                //        rightNode = circuitNodes[i + 1, k];

                //    if (rightNode != null && circuitNodes[i + 1, k] != null && circuitNodes[i, j] != null)
                //    {
                //        circuitNodes[i, j].neighbours.Add(circuitNodes[i + 1, k]);
                //        circuitNodes[i + 1, k].neighbours.Add(circuitNodes[i, j]);
                //    }
                //}
            }
        }

        //Create connection between nodes:
        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {
                CircuitNode rightNode = null;
                CircuitNode downNode = null;

                if (i < circuitNodes.GetLength(0) - 1)
                    rightNode = circuitNodes[i + 1, j];

                if (j < circuitNodes.GetLength(1) - 1)
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

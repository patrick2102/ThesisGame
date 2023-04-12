using TMPro;
using UnityEngine;

public class NodesScreen : MonoBehaviour
{
    public CircuitNode inputNodePrefab;
    public CircuitNode outputNodePrefab;
    public CircuitNode circuitNodePrefab;

    public CircuitNode inputNode;
    public RectTransform screen;

    public TMP_InputField inputField;

    public static NodesScreen instance;

    private CircuitNode[,] circuitNodes;
    private GameObject[] inputFields;

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

        inputFields = new GameObject[circuitNodes.Length];
    }

    private void FixedUpdate()
    {
        SetNodePositions();
    }

    private void SetNodePositions()
    {
        var stepSizeX = (screen.rect.width) / (circuitNodes.GetLength(0));
        var stepSizeY = (screen.rect.height) / (circuitNodes.GetLength(1));

        var stepSize = new Vector2(stepSizeX, stepSizeY);

        var border = stepSize / 2;

        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {
                var node = circuitNodes[i, j];

                if (node != null)
                {
                    var offSet = border + (stepSize * new Vector2(i, circuitNodes.GetLength(1) - j - 1));

                    // Add adjustable distance element
                    if (node.GetCommand() is DirectionCommand || node.GetCommand() is FollowCommand)
                    {
                        if (node.GetComponentInChildren<TMP_InputField>() == null)
                        {
                            AddAdjustableFieldToDirectionNode(i + j, node);
                        }
                        if (node.GetComponentInChildren<DirectionCommand>() != null)
                        {
                            var stringTextInput = inputFields[i + j].GetComponent<TMP_InputField>().text;
                            if (stringTextInput.Length > 0) // This check is to avoid problems while the input is blank rather than an int
                            {
                                float textInputAsFloat = float.Parse(stringTextInput);
                                node.GetComponentInChildren<DirectionCommand>().maxTimer = textInputAsFloat;
                            }
                        }
                        if (node.GetComponentInChildren<FollowCommand>() != null)
                        {
                            var stringTextInput = inputFields[i + j].GetComponent<TMP_InputField>().text;
                            if (stringTextInput.Length > 0) // This check is to avoid problems while the input is blank rather than an int
                            {
                                float textInputAsFloat = float.Parse(stringTextInput);
                                node.GetComponentInChildren<FollowCommand>().maxTimer = textInputAsFloat;
                                if (textInputAsFloat == 0)
                                {
                                    node.GetComponentInChildren<FollowCommand>().maxTimer = 999.9f; // This is a simple way of achieving something akin to constant following
                                }
                            } 
                        }
                        offSet.x -= 20.0f;
                    }
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

        //ConnectNodesNeighborsOnly();
        //ConnectNodesColumns();
        ConnectNodesColumnsAndRows();
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

                if (i < circuitNodes.GetLength(0)-1 )
                {
                    for (int k = 0; k < circuitNodes.GetLength(1); k++)
                    {
                        CircuitNode rightNode = circuitNodes[i + 1, k];

                        if (rightNode != null && circuitNodes[i + 1, k] != null && circuitNodes[i, j] != null)
                        {
                            circuitNodes[i, j].neighbours.Add(circuitNodes[i + 1, k]);
                            circuitNodes[i + 1, k].neighbours.Add(circuitNodes[i, j]);
                        }
                    }
                }
            }
        }
    }

    private void ConnectNodesColumnsAndRows()
    {
        for (int i = 0; i < circuitNodes.GetLength(0); i++)
        {
            for (int j = 0; j < circuitNodes.GetLength(1); j++)
            {

                if (j < circuitNodes.GetLength(1) - 1)
                {
                    for (int k = 0; k < circuitNodes.GetLength(0); k++)
                    {
                        CircuitNode downNode = circuitNodes[k, j + 1];

                        if (downNode != null && circuitNodes[k, j + 1] != null && circuitNodes[i, j] != null)
                        {
                            circuitNodes[i, j].neighbours.Add(circuitNodes[k, j + 1]);
                            circuitNodes[k, j + 1].neighbours.Add(circuitNodes[i, j]);
                        }
                    }
                }


                if (i < circuitNodes.GetLength(0) - 1)
                {
                    for (int k = 0; k < circuitNodes.GetLength(1); k++)
                    {
                        CircuitNode rightNode = circuitNodes[i + 1, k];

                        if (rightNode != null && circuitNodes[i + 1, k] != null && circuitNodes[i, j] != null)
                        {
                            circuitNodes[i, j].neighbours.Add(circuitNodes[i + 1, k]);
                            circuitNodes[i + 1, k].neighbours.Add(circuitNodes[i, j]);
                        }
                    }
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

    private void AddAdjustableFieldToDirectionNode(int currentNodeArrayPosition, CircuitNode node)
    {
        inputField.gameObject.SetActive(true);
        inputFields[currentNodeArrayPosition] = Instantiate(inputField.gameObject);
        inputFields[currentNodeArrayPosition].transform.SetParent(node.transform, false);
        Vector2 inputFieldPosition = new(50.0f, 0.0f);
        inputFields[currentNodeArrayPosition].GetComponent<RectTransform>().anchoredPosition = inputFieldPosition;
        inputField.gameObject.SetActive(false);
    }
}

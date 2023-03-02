using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircuitNode : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public List<CircuitNode> neighbours;

    [SerializeField] NodeType nodeType;
    [SerializeField] private CircuitNode nextNode;
    [SerializeField] private AICommand command;
    [SerializeField] private Text nodeText;
    [SerializeField] private LineRenderer nodeConnectionPrefab;
    private LineRenderer nodeConnection;

    public void Start()
    {
        nodeConnection = Instantiate(nodeConnectionPrefab);
        nodeConnection.transform.SetParent(transform);
    }

    public void FixedUpdate()
    {
        if (nextNode != null)
        {
            nodeConnection.gameObject.SetActive(true);

            //var nodeRectTransform = transform;
            //var nextNodeRectTransform = nextNode.transform;

            //Vector2 offset = nextNodeRectTransform.position - nodeRectTransform.position;

            //offset = new Vector2(Mathf.Clamp(offset.x, -25.0f, 25.0f), Mathf.Clamp(offset.y, -25.0f, 25.0f));

            nodeConnection.SetPosition(0, transform.position);
            nodeConnection.SetPosition(1, nextNode.transform.position);
        }
        else
        {
            nodeConnection.gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AIProgramFrontendManager.instance.UnclickNode(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right && nodeType == NodeType.CircuitNode)
        {
            AIProgramFrontendManager.instance.RightClickNode(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AIProgramFrontendManager.instance.LeftClickNode();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AIProgramFrontendManager.instance.SetMousedOverNode(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AIProgramFrontendManager.instance.SetMousedOverNode(null);
    }

    public CircuitNode GetNextNode()
    {
        return nextNode;
    }

    public void SetNextNode(CircuitNode newNextNode)
    {
        nextNode = newNextNode;
    }

    public void ChangeCommand(AICommand newCommand)
    {
        Destroy(command);
        command = Instantiate(newCommand);
        command.transform.SetParent(transform);

        nodeText.text = newCommand.name;
    }

    public AICommand GetCommand()
    {
        return command;
    }

    public override string ToString()
    {
        return gameObject.name + "{nodeType = " + nodeType + " id = " + id + "}";
    }
}

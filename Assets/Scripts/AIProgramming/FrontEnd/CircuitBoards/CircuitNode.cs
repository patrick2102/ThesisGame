using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircuitNode : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
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
    }

    public void FixedUpdate()
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UIManager.instance.UnclickNode(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UIManager.instance.LeftClickNode();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("mousePosition: " + eventData.position);
        UIManager.instance.SetMousedOverNode(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.SetMousedOverNode(null);
    }

    public CircuitNode GetNextNode()
    {
        return nextNode;
    }

    public void SetNextNode(CircuitNode newNextNode)
    {
        nextNode = newNextNode;

        if (nodeConnection != null)
            Destroy(nodeConnection);

        nodeConnection = Instantiate(nodeConnectionPrefab);
        nodeConnection.SetPosition(0, transform.position);
        nodeConnection.SetPosition(1, nextNode.transform.position);
        nodeConnection.transform.SetParent(transform);
    }

    public void RemoveNextNode()
    {
        if (nodeConnection != null)
            Destroy(nodeConnection);

        nextNode = null;
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

    public void OnDrop(PointerEventData eventData)
    {
       //Debug.Log("Something dropped");
    }
}

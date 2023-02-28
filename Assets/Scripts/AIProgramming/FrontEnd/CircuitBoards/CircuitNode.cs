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

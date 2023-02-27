using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircuitNode : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public List<CircuitNode> neighbours;

    [SerializeField] NodeType nodeType;

    public AICommand command;
    public Text nodeText;

    public int id;

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

    public void ConnectTo(CircuitNode toNode)
    {
        command.SetNext(toNode.command);
        toNode.command.SetPrev(command);
    }

    public override string ToString()
    {
        return gameObject.name + "{nodeType = " + nodeType + " id = " + id + "}";
    }
}

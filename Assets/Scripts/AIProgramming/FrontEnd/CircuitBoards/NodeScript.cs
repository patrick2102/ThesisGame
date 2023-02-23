using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private enum NodeType
    {
        InputNode, CircuitNode, OutputNode
    }

    public List<NodeScript> neighbours;
    public Queue<NodeScript> activeConnections;

    [SerializeField] NodeType nodeType;

    public AICommand command;
    public Text nodeText;

    public void Awake()
    {
        activeConnections = new Queue<NodeScript>();
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
            AIProgramFrontendManager.instance.LeftClickNode(this);
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
}

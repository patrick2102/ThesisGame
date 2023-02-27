using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircuitNode : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{


    public List<CircuitNode> neighbours;
    public Queue<CircuitNode> activeConnections;

    [SerializeField] NodeType nodeType;

    public AICommand command;
    public Text nodeText;

    public int id;

    public void Awake()
    {
        activeConnections = new Queue<CircuitNode>();
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

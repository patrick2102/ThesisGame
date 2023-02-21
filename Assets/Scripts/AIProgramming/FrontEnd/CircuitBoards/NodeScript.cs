using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeScript : MonoBehaviour, IPointerClickHandler
{
    private enum NodeType
    {
        InputNode, CircuitNode, OutputNode
    }

    public List<NodeScript> neighbours;
    public Queue<NodeScript> activeConnections;

    AIProgramFrontendManager frontEndManager;

    [SerializeField] NodeType nodeType;

    public IAICommand command;

    public void Awake()
    {
        activeConnections = new Queue<NodeScript>();
        frontEndManager = AIProgramFrontendManager.instance;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AIProgramFrontendManager.instance.LeftClickNode(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            AIProgramFrontendManager.instance.RightClickNode(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    public NodeScript startNode;
    private AIProgram program;

    public void Start()
    {
        program = new AIProgram(startNode.command);
        AIProgramBackendManager.instance.SetActiveProgram(program);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //AIProgramFrontendManager.instance.star
        AIProgramBackendManager.instance.StartProgram();
        Debug.Log("Run program");
    }
}

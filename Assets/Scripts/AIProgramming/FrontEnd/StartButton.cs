using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //AIProgramFrontendManager.instance.star
        AIProgramBackendManager.instance.StartProgram();
        Debug.Log("Run program");
    }
}

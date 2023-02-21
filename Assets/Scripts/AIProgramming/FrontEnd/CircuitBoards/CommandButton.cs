using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour, IPointerClickHandler
{
    public AICommand command;
    public Text commandButtonText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AIProgramFrontendManager.instance.SelectCommand(this);
            //AIProgramFrontendManager.instance.LeftClickNode(this);
            Debug.Log("Command clicked");
        }
    }
}

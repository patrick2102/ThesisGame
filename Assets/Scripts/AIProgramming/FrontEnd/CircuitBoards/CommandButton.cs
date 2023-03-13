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
            UIManager.instance.ChangeCommand(this);
            Debug.Log("Command clicked");
        }
    }
}

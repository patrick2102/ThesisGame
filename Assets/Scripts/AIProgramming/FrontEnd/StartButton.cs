using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        AIProgram.activeProgram.StartProgram();
        if (UIManager.instance.GetBoolForWithinInteractRange())
        {
            UIManager.instance.SetUI(UIManager.UIState.interactScreen);
        } else
        {
            UIManager.instance.SetUI(UIManager.UIState.closed);
        }
        Debug.Log("Run program");
    }
}

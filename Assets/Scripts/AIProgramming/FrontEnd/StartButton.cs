using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AIProgram.activeProgram.StartProgram();
        Debug.Log("Run program");
    }
}

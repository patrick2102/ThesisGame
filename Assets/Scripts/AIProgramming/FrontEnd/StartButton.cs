using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AIProgramBackendManager.instance.StartProgram();
        AIProgramBackendManager.instance.ResetActiveProgram();
        Debug.Log("Run program");
    }
}

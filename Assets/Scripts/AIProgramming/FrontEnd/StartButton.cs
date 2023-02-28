using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AIProgramBackendManager.instance.StartProgram();
        Debug.Log("Run program");
    }
}

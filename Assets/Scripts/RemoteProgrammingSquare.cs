using System.Runtime.CompilerServices;
using UnityEngine;
using static Interactable;

/*
 * Robot controller used by an AI program to control the behavior of the robot.
 */
public class RemoteProgrammingSquare : MonoBehaviour
{
    private bool programmingPanelOpen = false;
    [SerializeField] Interactable interactable;

    private void Update()
    {
        if (interactable.Interacted())
        {
            OpenProgrammingPanel();
        }
        else if (interactable.FinishedInteraction())
        {
            CloseProgrammingPanel();
        }
    }

    public void OpenProgrammingPanel()
    {
        UIManager.instance.SetUI(UIManager.UIState.nodeScreen);
        programmingPanelOpen = true;
    }

    public void CloseProgrammingPanel()
    {
        if(interactable.CanInteract())
            UIManager.instance.SetUI(UIManager.UIState.interactScreen);
        else
            UIManager.instance.SetUI(UIManager.UIState.closed);
        programmingPanelOpen = false;
    }
}

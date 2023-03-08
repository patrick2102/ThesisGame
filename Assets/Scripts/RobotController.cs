using System.Runtime.CompilerServices;
using UnityEngine;
using static Interactable;

/*
 * Robot controller used by an AI program to control the behavior of the robot.
 */
public class RobotController : MonoBehaviour
{
    public float speed = 50;
    private bool programmingPanelOpen = false;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Interactable interactable;

    public void MoveDirection(Vector3 direction)
    {
        direction = direction.normalized;

        var force = speed * direction;
        rb.AddForce(force);
    }

    private void Update()
    {
        //if (interactable.GetInteracted())
        //{
        //    ToogleProgrammingPanel();
        //    interactable.SetInteraction(false);
        //}

        if (Input.GetKeyUp(interactable.GetInteractKey()) && interactable.GetInteractionState() == InteractionState.canInteract)
        {
            ToggleProgrammingPanel();
            interactable.SetInteraction(InteractionState.interacting);
        }
        else if (Input.GetKeyUp(interactable.GetInteractKey()) && interactable.GetInteractionState() == InteractionState.interacting)
        {
            ToggleProgrammingPanel();
            interactable.SetInteraction(InteractionState.cannotInteract);
        }

        //if (interactable.GetInteracted() == Interactable.InteractionState.startedInteracting)
        //{
        //    OpenProgrammingPanel();
        //}
        //if (interactable.GetInteracted() == Interactable.InteractionState.stoppedInteracting)
        //    CloseProgrammingPanel();
    }

    private void ToggleProgrammingPanel()
    {
        if (programmingPanelOpen)
            CloseProgrammingPanel();
        else
            OpenProgrammingPanel();

    }

    public void OpenProgrammingPanel()
    {
        UIManager.instance.SetUI(UIManager.UIState.nodeScreen);
        programmingPanelOpen = true;
    }

    public void CloseProgrammingPanel()
    {
        UIManager.instance.SetUI(UIManager.UIState.closed);
        programmingPanelOpen = false;
    }
}

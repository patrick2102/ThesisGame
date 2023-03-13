using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public enum InteractionState
    { 
        canInteract, interacting, cannotInteract
    }

    private KeyCode interactKey = KeyCode.E;
    private InteractionState interactionState = InteractionState.cannotInteract;
    private bool withinRange;

    private void Start()
    {
        withinRange = false;   
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameObjectTags.Player.ToString())
        {
            interactionState = InteractionState.canInteract;
            UIManager.instance.SetUI(UIManager.UIState.interactScreen);
            withinRange = true;
        }
        //UIManager.instance.SetInteractScreen(withinRange);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameObjectTags.Player.ToString())
        {
            interactionState = InteractionState.cannotInteract;
            UIManager.instance.SetUI(UIManager.UIState.closed);
            withinRange = false;
        }
        //UIManager.instance.SetInteractScreen(withinRange);
    }

    public bool CanInteract()
    {
        return interactionState == InteractionState.canInteract;
    }

    public bool Interacted()
    {
        if (Input.GetKeyUp(GetInteractKey()) && interactionState == InteractionState.canInteract)
        {
            SetInteraction(InteractionState.interacting);
            return true;
        }
        else
            return false;
    }

    public bool FinishedInteraction()
    {
        if (Input.GetKeyUp(GetInteractKey()) && GetInteractionState() == InteractionState.interacting)
        {
            if (withinRange)
            {
                SetInteraction(InteractionState.canInteract);
            }
            else
            {
                SetInteraction(InteractionState.cannotInteract);
            }
            return true;
        }
        else
            return false;
    }

    public KeyCode GetInteractKey()
    {
        return interactKey;
    }

    private void SetInteraction(InteractionState newInteractionState)
    {
        interactionState = newInteractionState;
    }

    private InteractionState GetInteractionState()
    {
        return interactionState;
    }
}

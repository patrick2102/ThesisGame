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
        interactionState = InteractionState.canInteract;
        UIManager.instance.SetUI(UIManager.UIState.interactScreen);
        //UIManager.instance.SetInteractScreen(withinRange);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        interactionState = InteractionState.cannotInteract;
        UIManager.instance.SetUI(UIManager.UIState.closed);
        //UIManager.instance.SetInteractScreen(withinRange);
    }

    private void Update()
    {



        //if (interacting == InteractionState.stoppedInteracting)
        //    interacting = InteractionState.notInteracting;

        //if (Input.GetKeyUp(interactKey) && withinRange && interacting == InteractionState.notInteracting)
        //{
        //    SetInteraction(InteractionState.startedInteracting);
        //}
        //else if (Input.GetKeyUp(interactKey) && interacting == InteractionState.currentlyInteracting)
        //{
        //    SetInteraction(InteractionState.stoppedInteracting);
        //    UIManager.instance.SetUI(UIManager.UIState.interactScreen);
        //}


        //if (!withinRange)
        //{
        //    SetInteraction(InteractionState.stoppedInteracting);
        //    UIManager.instance.SetUI(UIManager.UIState.closed);
        //}
    }

    public KeyCode GetInteractKey()
    {
        return interactKey;
    }

    public void SetInteraction(InteractionState inter)
    {
        interactionState = inter;
    }

    public InteractionState GetInteractionState()
    {
        return interactionState;
    }
}

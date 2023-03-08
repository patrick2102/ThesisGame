using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public enum InteractionState
    { 
        notInteracting, startedInteracting, currentlyInteracting, stoppedInteracting
    }

    private KeyCode interactKey = KeyCode.E;
    private InteractionState interacting = InteractionState.notInteracting;
    private bool withinRange;

    private void Start()
    {
        withinRange = false;   
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        withinRange = true;
        UIManager.instance.SetInteractScreen(withinRange);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        withinRange = false;
        UIManager.instance.SetInteractScreen(withinRange);
    }

    private void Update()
    {
        if (interacting == InteractionState.stoppedInteracting)
            interacting = InteractionState.notInteracting;

        if (Input.GetKeyUp(interactKey) && withinRange && interacting == InteractionState.notInteracting)
        {
            SetInteraction(InteractionState.startedInteracting);
        }
        else if (Input.GetKeyUp(interactKey) && interacting == InteractionState.currentlyInteracting)
        {
            SetInteraction(InteractionState.stoppedInteracting);
            UIManager.instance.SetInteractScreen(true);
        }


        if (!withinRange)
        {
            SetInteraction(InteractionState.stoppedInteracting);
        }
    }

    public void SetInteraction(InteractionState inter)
    {
        interacting = inter;
    }

    public void FinishInteraction()
    {
        interacting = InteractionState.notInteracting;
    }



    public InteractionState GetInteracted()
    {
        return interacting;
    }
}

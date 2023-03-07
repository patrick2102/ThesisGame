using UnityEngine;

/*
 * Robot controller used by an AI program to control the behavior of the robot.
 */
public class RobotController : MonoBehaviour
{
    public float speed = 50;
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
        if (interactable.GetInteracted() == Interactable.InteractionState.startedInteracting)
        {
            OpenProgrammingPanel();
        }
        if (interactable.GetInteracted() == Interactable.InteractionState.stoppedInteracting)
            CloseProgrammingPanel();
    }

    public void OpenProgrammingPanel()
    {
        UIManager.instance.SetUI(UIManager.UIState.nodeScreen);
        interactable.SetInteraction(Interactable.InteractionState.currentlyInteracting);

    }

    public void CloseProgrammingPanel()
    {
        UIManager.instance.SetUI(UIManager.UIState.closed);
        interactable.SetInteraction(Interactable.InteractionState.notInteracting);
    }
}

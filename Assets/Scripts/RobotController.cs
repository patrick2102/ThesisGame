using System.Runtime.CompilerServices;
using UnityEngine;
using static Interactable;

/*
 * Robot controller used by an AI program to control the behavior of the robot.
 */
public class RobotController : MonoBehaviour
{
    public enum RobotBehaviourState
    {
        none, distracting
    }

    public RobotBehaviourState behaviorState = RobotBehaviourState.none;
    public static RobotController instance;
    public float speed = 50;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Interactable interactable;
    public Pickupable pickedUpObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }


    public void MoveDirection(Vector3 direction)
    {
        direction = direction.normalized;

        var force = speed * direction;
        rb.AddForce(force);
    }

    public void SetBehaviorState(RobotBehaviourState rbs)
    {
        behaviorState = rbs;
    }

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

    public void PickUp(Pickupable objectToPickup)
    {
        pickedUpObject = objectToPickup;
        objectToPickup.PickUp();
    }

    public void PutDown()
    {
        SetBehaviorState(RobotBehaviourState.none);
        pickedUpObject.PutDown();
        pickedUpObject = null;
    }

    public void OpenProgrammingPanel()
    {
        UIManager.instance.SetUI(UIManager.UIState.nodeScreen);
    }

    public void CloseProgrammingPanel()
    {
        if(interactable.CanInteract())
            UIManager.instance.SetUI(UIManager.UIState.interactScreen);
        else
            UIManager.instance.SetUI(UIManager.UIState.closed);
    }
}

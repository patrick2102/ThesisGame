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
        none, distracting, pickup, putdown, following
    }

    public RobotBehaviourState behaviorState = RobotBehaviourState.none;
    public static RobotController instance;
    public float speed = 50;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Interactable interactable;
    public Transform objectToPickup;
    private float distanceToWater;
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

    public void StartFollowing()
    {
        SetBehaviorState(RobotBehaviourState.following);
        RaycastHit2D[] results = new RaycastHit2D[10]; // Figure out how to make raycasting work here xD
        gameObject.GetComponent<CircleCollider2D>().Raycast(Vector2.down, results);
        foreach (RaycastHit2D currentHit in results)
        {
            if(currentHit.collider != null && currentHit.collider.gameObject.CompareTag("Water"))
            {
                distanceToWater = currentHit.distance;
            }
        }

    }

    public void StopFollowing()
    {
        SetBehaviorState(RobotBehaviourState.none);
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

    public float GetCurrentWaterDistance()
    {
        return distanceToWater;
    }
}

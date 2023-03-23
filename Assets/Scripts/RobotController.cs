using System.Runtime.CompilerServices;
using UnityEngine;
using static Interactable;

/*
 * Robot controller used by an AI program to control the behavior of the robot.
 */
public class RobotController : MonoBehaviour
{
    public static RobotController instance;
    public float speed = 50;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Interactable interactable;
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
    }

    public void CloseProgrammingPanel()
    {
        if(interactable.CanInteract())
            UIManager.instance.SetUI(UIManager.UIState.interactScreen);
        else
            UIManager.instance.SetUI(UIManager.UIState.closed);
    }
}

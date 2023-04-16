using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressurePlateTrigger : TriggerBase
{
    public GameObject door;
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private Transform moveToPosition;
    [SerializeField] private GameObject objectToRemove;
    [SerializeField] private CheckpointTrigger checkpoint;
    public bool moveOverTime;
    public float moveDuration;
    public bool resetPositionAtRestart;
    private Vector3 originalDoorPosition;
    private RobotEmotionStateHandler robotEmotionStateHandler;


    public void Awake()
    {
        originalDoorPosition= door.transform.position;
        robotEmotionStateHandler = GameObject.FindGameObjectWithTag(GameObjectTags.Robot.ToString()).GetComponent<RobotEmotionStateHandler>();
    }

    public override void HandleTriggerEnter(string tag)
    {
        switch (tag)
        {
            case nameof(GameObjectTags.Player):
                if(doorCollider != null)
                    doorCollider.enabled = false;
                StartCoroutine(MoveDoorToPosition());
                if (objectToRemove != null)
                    objectToRemove.SetActive(false);
                break;
            case nameof(GameObjectTags.Robot):
                if (doorCollider != null && !moveOverTime)
                    doorCollider.enabled = false;
                StartCoroutine(MoveDoorToPosition());
                if (objectToRemove != null)
                    objectToRemove.SetActive(false);
                robotEmotionStateHandler.SwitchRobotEmotionState(RobotEmotionStateHandler.EmotionState.happy);
                
                // Create a temporary reference to the current scene.
                Scene currentScene = SceneManager.GetActiveScene();

                // Retrieve the name of this scene.
                string sceneName = currentScene.name;

                if (sceneName == "RobotDeath")
                {
                    GameManager.instance.ChangeView(GameManager.CameraState.monsterCamera);
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator MoveDoorToPosition()
    {
        if (moveOverTime)
        {
            var currentPos = door.transform.position;
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / moveDuration;
                door.transform.position = Vector3.Lerp(currentPos, moveToPosition.position, t);
                yield return null;
            }
        } else
        {
            door.transform.position = moveToPosition.position;
        }
    }

    public override void HandleTriggerExit(string tag)
    {
        return;
    }

    public Vector3 GetOriginalDoorPosition()
    {
        return originalDoorPosition;
    }

    public void ResetTrigger()
    {
        if (checkpoint != null && !checkpoint.checkpointTriggered)
        {
            if (doorCollider != null)
                doorCollider.enabled = true;

            if (objectToRemove != null)
                objectToRemove.SetActive(true);

            door.transform.position = originalDoorPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    private bool triggerOnlyOnce = true;
    public bool resetPositionAtRestart;
    private Vector3 originalDoorPosition;
    private RobotEmotionStateHandler robotEmotionStateHandler;

    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private AudioSource audioSource;

    public void Awake()
    {
        originalDoorPosition= door.transform.position;
        robotEmotionStateHandler = GameObject.FindGameObjectWithTag(GameObjectTags.Robot.ToString()).GetComponent<RobotEmotionStateHandler>();
        audioSource.clip = triggerSound;
    }

    public override void HandleTriggerEnter(string tag)
    {
        if (triggerOnlyOnce)
        {
            switch (tag)
            {
                case nameof(GameObjectTags.Player):
                    if (doorCollider != null && !moveOverTime)
                        doorCollider.enabled = false;
                    StartCoroutine(MoveDoorToPosition());
                    if (objectToRemove != null)
                        objectToRemove.SetActive(false);
                    audioSource.Play();
                    break;
                case nameof(GameObjectTags.Robot):
                    if (doorCollider != null && !moveOverTime)
                        doorCollider.enabled = false;
                    StartCoroutine(MoveDoorToPosition());
                    if (objectToRemove != null)
                        objectToRemove.SetActive(false);
                    robotEmotionStateHandler.SwitchRobotEmotionState(RobotEmotionStateHandler.EmotionState.happy);
                    audioSource.Play();
                    break;
                case nameof(GameObjectTags.Monster):
                    // Temporary reference to the current scene.
                    Scene currentScene = SceneManager.GetActiveScene();

                    // Retrieve the name of this scene.
                    string sceneName = currentScene.name;

                    // This is just to ensure only the RobotDeath scene is affected by this code. Other scenes should not have monsters triggering pressure plates
                    if (sceneName == "RobotDeath")
                    {
                        if (doorCollider != null && !moveOverTime)
                            doorCollider.enabled = false;
                        StartCoroutine(MoveDoorToPosition());
                        if (objectToRemove != null)
                            objectToRemove.SetActive(false);
                        audioSource.Play();
                    }
                    break;
                default:
                    break;
            }
            triggerOnlyOnce = false;
        }
    }

    private IEnumerator MoveDoorToPosition()
    {
        doorCollider.enabled = true;
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
        var doorScript = door.GetComponent<Door>();

        if (doorScript != null)
            doorScript.PlayAudio();
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
        //if (checkpoint != null && !checkpoint.checkpointTriggered)
        //{
        if (doorCollider != null)
            doorCollider.enabled = true;

        if (objectToRemove != null)
            objectToRemove.SetActive(true);

        StopAllCoroutines();

        door.transform.position = originalDoorPosition;
        //}
    }
}

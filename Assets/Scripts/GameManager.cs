using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public enum CameraState
    { 
        playerCamera, pathCamera, monsterCamera
    }

    public static CameraState currentCameraState = CameraState.playerCamera;

    public static GameManager instance;

    [SerializeField] private AIProgram aiProgramPrefab;
    private GameObject player;
    private GameObject robot;
    private GameObject[] monsters;
    private GameObject[] pressurePlates;
    private (Vector3, Quaternion)[] monsterSpawns;

    //If they spawn in seperate places
    public CheckpointTrigger lastPlayerSeperateSpawn;
    public CheckpointTrigger lastRobotSeperateSpawn;

    //If they spawn in the same place
    public CheckpointTrigger lastCheckPoint;

    //Cameras
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera pathCamera;

    //TargetGroups
    [SerializeField] private CinemachineTargetGroup playerTargetGroup;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("Awake called");
    }

    private void Start()
    {
        SetupScene();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
    }

    public void ChangeView(CameraState newCameraState)
    {
        currentCameraState = newCameraState;

        if (currentCameraState == CameraState.playerCamera)
        {
            playerCamera.Priority = 10;
            pathCamera.Priority = 0;
        }
        else if (currentCameraState == CameraState.pathCamera)
        {
            playerCamera.Priority = 0;
            pathCamera.Priority = 10;
        }
    }

    public void Restart()
    {
        //Spawn player and robot back at checkpoint:
        if (player != null)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody2D>().angularVelocity = 0;
            if (lastPlayerSeperateSpawn != null)
            {
                player.transform.SetPositionAndRotation(lastPlayerSeperateSpawn.GetSpawnPoint().position, lastPlayerSeperateSpawn.GetSpawnPoint().rotation);
            }
            else
            {
                player.transform.SetPositionAndRotation(lastCheckPoint.GetSpawnPoint().position, lastCheckPoint.GetSpawnPoint().rotation);
            }
        }
        if (robot != null)
        {
            robot.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            robot.GetComponent<Rigidbody2D>().angularVelocity = 0;
            if (lastRobotSeperateSpawn != null)
            {
                robot.transform.SetPositionAndRotation(lastRobotSeperateSpawn.GetSpawnPoint().position, lastRobotSeperateSpawn.GetSpawnPoint().rotation);
            }
            else
            {
                robot.transform.SetPositionAndRotation(lastCheckPoint.GetSpawnPoint().position + new Vector3(1, 0, 0), lastCheckPoint.GetSpawnPoint().rotation);
            }
        }

        //Reset doors opened using pressureplates, if they are marked to reset:
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            pressurePlates[i].GetComponent<PressurePlateTrigger>().ResetTrigger();
            //if (pressurePlates[i].GetComponent<PressurePlateTrigger>().resetPositionAtRestart)
            //{
            //    pressurePlates[i].GetComponent<PressurePlateTrigger>().ResetTrigger();
            //}
        }

        //Spawn monsters back at start point:
        for (int i = 0; i < monsters.Length; i++)
        {
            var spawnPos = monsterSpawns[i];
            monsters[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            monsters[i].GetComponent<Rigidbody2D>().angularVelocity = 0;
            monsters[i].transform.SetPositionAndRotation(spawnPos.Item1, spawnPos.Item2);
        }
        if(AIProgram.activeProgram != null)
            AIProgram.activeProgram.ResetProgram();
            //aiProgramBackendManager.ResetProgram();
    }

    public void PlayerDeath()
    {
        Debug.Log("Player died");
        Restart();
    }

    public void RobotDeath()
    {
        Debug.Log("Robot should break down?");
        //Restart();
    }

    public void TriggerCheckpoint(CheckpointTrigger newCheckpoint)
    {
        if (lastCheckPoint == null)
        {
            lastCheckPoint = newCheckpoint;
            return;
        }

        if (lastCheckPoint.checkpointOrder < newCheckpoint.checkpointOrder)
        {
            Debug.Log("Checkpoint triggered");
            lastCheckPoint = newCheckpoint;
        }
    }

    private void SetupScene()
    {
        player = GameObject.FindGameObjectWithTag(GameObjectTags.Player.ToString());

        robot = GameObject.FindGameObjectWithTag(GameObjectTags.Robot.ToString());

        if (robot != null)
        {
            var program = Instantiate(aiProgramPrefab);
            program.SetupProgram(robot.GetComponent<RobotController>(), NodesScreen.instance.inputNode);
        }

        monsters = GameObject.FindGameObjectsWithTag(GameObjectTags.Monster.ToString());
        if (monsters != null)
            monsterSpawns = monsters.Select(x => (x.transform.position, x.transform.rotation)).ToArray();

        pressurePlates = GameObject.FindGameObjectsWithTag(GameObjectTags.PressurePlate.ToString());

        if (playerTargetGroup.FindMember(player.transform) == -1 || playerTargetGroup.FindMember(robot.transform) == -1)
        {
            for (int i = 0; i < playerTargetGroup.m_Targets.Length; i++)
            {
                playerTargetGroup.RemoveMember(playerTargetGroup.m_Targets[i].target);
            }
            playerTargetGroup.AddMember(player.transform, 4, 3);
            playerTargetGroup.AddMember(robot.transform, 4, 5);
        }
        Restart();
    }

    public void ChangeLevel(int buildIndex)
    {
        StartCoroutine(LoadScene(buildIndex));
    }

    private IEnumerator LoadScene(int buildIndex)
    {
        var load = SceneManager.LoadSceneAsync(buildIndex);

        yield return new WaitUntil(() => load.isDone);

        //SetupScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

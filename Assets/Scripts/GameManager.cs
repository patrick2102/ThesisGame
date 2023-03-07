using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private AIProgram aiProgramPrefab;
    public GameObject player;
    public GameObject robot;
    private GameObject[] monsters;
    private (Vector3, Quaternion)[] monsterSpawns;


    public CheckpointTrigger lastCheckPoint;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (player == null)
            player = GameObject.FindGameObjectWithTag(GameObjectTags.Player.ToString());

        if (robot == null)
            robot = GameObject.FindGameObjectWithTag(GameObjectTags.Robot.ToString());

        monsters = GameObject.FindGameObjectsWithTag(GameObjectTags.Monster.ToString());
        monsterSpawns = GameObject.FindGameObjectsWithTag(GameObjectTags.Monster.ToString()).Select(x => (x.transform.position, x.transform.rotation)).ToArray();

        Restart();
    }

    private void Start()
    {
        var program = Instantiate(aiProgramPrefab);
        program.SetupProgram(robot.GetComponent<RobotController>(), NodesScreen.instance.inputNode);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
            AIProgram.activeProgram.ResetProgram();
        }
    }

    private void Restart()
    {
        //Spawn player and robot back at checkpoint:
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        player.transform.SetPositionAndRotation(lastCheckPoint.GetSpawnPoint().position, lastCheckPoint.GetSpawnPoint().rotation);

        robot.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        robot.GetComponent<Rigidbody2D>().angularVelocity = 0;
        robot.transform.SetPositionAndRotation(lastCheckPoint.GetSpawnPoint().position + new Vector3(1, 0,0), lastCheckPoint.GetSpawnPoint().rotation);

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
        Restart();
    }

    public void TriggerCheckpoint(CheckpointTrigger newCheckpoint)
    {
        if (lastCheckPoint.checkpointOrder < newCheckpoint.checkpointOrder)
        {
            Debug.Log("Checkpoint triggered");
            lastCheckPoint = newCheckpoint;
        }
    }
}

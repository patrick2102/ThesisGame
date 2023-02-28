using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    }

    public void PlayerDeath()
    {
        Debug.Log("Player should die");
    }

    public void RobotDeath()
    {
        Debug.Log("Robot should break down?");
    }
}

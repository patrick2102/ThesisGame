using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementVisualiser : MonoBehaviour
{
    [SerializeField] private RobotController robotSimulation;
    [SerializeField] private LineRenderer robotPath;

    private void FixedUpdate()
    {
        //Show robot path


    }

    private void UpdatePath()
    {
        var simulation = AIProgramBackendManager.instance.GetActiveProgram();

        if (simulation != null)
        {
            var node = simulation.currentNode;

            while (node != null)
            {
                var command = node.GetCommand();


            }

        }
    }

}

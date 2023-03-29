using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCommand : AICommand
{
    float timer; // Timer to that counts up to maxTimer to control time before going to the next command
    [SerializeField] public float maxTimer;
    private Transform playerTransform;
    public override ProgramStatus Step(RobotController rbc)
    {
        if (maxTimer > timer)
        {
            if(playerTransform == null)
                playerTransform = Player_controller.instance.transform;

            var fromRobotToPlayer = playerTransform.position - rbc.transform.position;
            var dist = fromRobotToPlayer.magnitude;
            fromRobotToPlayer = fromRobotToPlayer.normalized;

            if (dist > 2.0f)
            {
                rbc.MoveDirection(fromRobotToPlayer);
            }
            return ProgramStatus.running;
        }
        else
        {
            timer = 0;
            return ProgramStatus.stopped;
        }
    }
}

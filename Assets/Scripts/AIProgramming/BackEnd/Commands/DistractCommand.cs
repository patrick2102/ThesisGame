using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractCommand : AICommand
{
    float timer; // Timer to that counts up to maxTimer to control time before going to the next command
    [SerializeField] public float maxTimer;
    public override ProgramStatus Step(RobotController rbc)
    {
        if (maxTimer > timer)
        {
            rbc.SetBehaviorState(RobotController.RobotBehaviourState.distracting);
            timer += Time.deltaTime;
            return ProgramStatus.running;
        }
        else
        {
            rbc.SetBehaviorState(RobotController.RobotBehaviourState.none);
            timer = 0;
            return ProgramStatus.stopped;
        }
    }
}

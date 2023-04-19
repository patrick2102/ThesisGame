using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractCommand : AICommand
{
    float timer; // Timer to that counts up to maxTimer to control time before going to the next command
    AnimationControllerScript animationController;

    public override ProgramStatus Step(RobotController rbc)
    {
        if (rbc != null)
        {
            if (maxTimer > timer)
            {
                rbc.SetBehaviorState(RobotController.RobotBehaviourState.distracting);
                timer += Time.deltaTime;
                if (animationController == null)
                {
                    animationController = rbc.gameObject.GetComponent<AnimationControllerScript>();
                    animationController.TriggerDistractAnimation(maxTimer);
                }
                return ProgramStatus.running;
            }
            else
            {
                rbc.SetBehaviorState(RobotController.RobotBehaviourState.none);
                timer = 0;
                return ProgramStatus.stopped;
            }
        }
        return ProgramStatus.stopped;
    }
}

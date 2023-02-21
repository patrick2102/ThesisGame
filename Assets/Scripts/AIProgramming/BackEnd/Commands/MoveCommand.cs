using System;
using UnityEngine;

public class MoveCommand : IAICommand
{
    Func<Vector2> target;
    RobotController robotController;
    float stopDistance;
    public IAICommand next;

    public MoveCommand(Func<Vector2> targetPosition, float stopDistance = 0.1f)
    {
        this.stopDistance = stopDistance;
        target = targetPosition;
        robotController = AIProgramBackendManager.instance.robotController;
    }
    public IAICommand Next()
    {
        return next;
    }

    /*
     * Make a step with the command, and if the timer reaches its limit, then reset the time and return stopped status. Resetting the timer is done to allow for loops.
     */
    public ProgramStatus Step()
    {
        var direction = (target() - (Vector2)robotController.transform.position);
        var distToTarget = (direction).magnitude;
        if (distToTarget > stopDistance)
        {
            robotController.MoveDirection(direction);
            return ProgramStatus.running;
        }
        else
        {
            return ProgramStatus.stopped;
        }
    }


    /*
     * Various predefined versions of the Move commands. 
     */

    #region Predefined Move Commands

    public static MoveCommand MoveTo(Func<Vector2> target, float stopDist = 0.1f)
    {
        return new MoveCommand(target, stopDist);
    }

    public static MoveCommand MoveFrom(Func<Vector2> target, float stopDist = 0.1f)
    {
        Func<Vector2> oppositeTarget = () => -target();
        return new MoveCommand(oppositeTarget, stopDist);
    }
    #endregion
}

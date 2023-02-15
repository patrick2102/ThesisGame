using System;
using UnityEngine;

/*
 * Commands to do with moving the robot. The commands use a Func<Vector2> to control the directions that the robot should move. 
 */
public class MoveCommand : IAICommand
{
    float timer; // Timer to that counts up to maxTimer to control time before going to the next command
    float maxTimer;
    Func<Vector2> direction; // A function that returns a vector. The reason behind the Function is to allow dynamic directions
                             // that can change during runtime. Otherwise the direction would have to be manually changed outside of the class if necessary.
                             // Example in the case where the robot has to follow a moving target. 
    RobotController robotController;
    public IAICommand next;

    public MoveCommand(Func<Vector2> direction, float maxTimer)
    {
        timer = 0.0f;
        this.maxTimer = maxTimer;
        this.direction = direction;
        robotController = AIProgramManager.instance.robotController;
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

        if (maxTimer > timer)
        {
            robotController.MoveDirection(direction());
            timer += Time.deltaTime;
            return ProgramStatus.running;
        }
        else
        {
            timer = 0;
            return ProgramStatus.stopped;
        }
    }


    /*
     * Various predefined versions of the Move commands. 
     */

    #region Predefined Move Commands
    public static MoveCommand MoveUpCommand(float timer)
    {
        Func<Vector2> direction = () => new Vector2(0, 1);

        return new MoveCommand(direction, timer);
    }

    public static MoveCommand MoveDownCommand(float timer)
    {
        Func<Vector2> direction = () => new Vector2(0, -1);
        return new MoveCommand(direction, timer);
    }

    public static MoveCommand MoveLeftCommand(float timer)
    {
        Func<Vector2> direction = () => new Vector2(-1, 0);
        return new MoveCommand(direction, timer);
    }

    public static MoveCommand MoveRightCommand(float timer)
    {
        Func<Vector2> direction = () => new Vector2(1, 0);
        return new MoveCommand(direction, timer);
    }

    public static MoveCommand MoveTo(Func<Vector2> direction, float timer)
    {
        return new MoveCommand(direction, timer);
    }

    public static MoveCommand MoveFrom(Func<Vector2> direction, float timer)
    {
        Func<Vector2> oppositeDirection = () => -direction();
        return new MoveCommand(oppositeDirection, timer);
    }
    #endregion
}

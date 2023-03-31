using UnityEngine;

/*
 * Commands to do with moving the robot. The commands use a Func<Vector2> to control the directions that the robot should move. 
 */
public class DirectionCommand : AICommand
{
    float timer; // Timer to that counts up to maxTimer to control time before going to the next command
    public float maxTimer;
    public Vector2 direction;

    public DirectionCommand(Vector2 direction, float maxTimer)
    {
        timer = 0.0f;
        this.maxTimer = maxTimer;
        this.direction = direction;
    }

    /*
     * Make a step with the command, and if the timer reaches its limit, then reset the time and return stopped status. Resetting the timer is done to allow for loops.
     */
    public override ProgramStatus Step(RobotController rbc)
    {

        if (maxTimer > timer)
        {
            rbc.MoveDirection(direction);
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
    #region Predefined Direction Commands

    //public static DirectionCommand DirectionUpCommand(float timer)
    //{
    //    Func<Vector2> direction = () => new Vector2(0, 1);

    //    return new DirectionCommand(direction, timer);
    //}

    //public static DirectionCommand DirectionDownCommand(float timer)
    //{
    //    Func<Vector2> direction = () => new Vector2(0, -1);
    //    return new DirectionCommand(direction, timer);
    //}

    //public static DirectionCommand DirectionLeftCommand(float timer)
    //{
    //    Func<Vector2> direction = () => new Vector2(-1, 0);
    //    return new DirectionCommand(direction, timer);
    //}

    //public static DirectionCommand DirectionRightCommand(float timer)
    //{
    //    Func<Vector2> direction = () => new Vector2(1, 0);
    //    return new DirectionCommand(direction, timer);
    //}

    //public static DirectionCommand DirectionTo(Func<Vector2> direction, float timer)
    //{
    //    return new DirectionCommand(direction, timer);
    //}

    //public static DirectionCommand DirectionFrom(Func<Vector2> direction, float timer)
    //{
    //    Func<Vector2> oppositeDirection = () => -direction();
    //    return new DirectionCommand(oppositeDirection, timer);
    //}

    #endregion
}

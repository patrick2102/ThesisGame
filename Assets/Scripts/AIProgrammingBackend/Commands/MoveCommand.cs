using System;
using System.Threading;
using UnityEngine;

public class MoveCommand : IAICommand
{
    float timer;
    float maxTimer;
    Func<Vector2> direction;
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

    public ProgramStatus Run()
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

}

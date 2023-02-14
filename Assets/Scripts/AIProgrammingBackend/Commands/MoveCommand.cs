using System.Threading;
using UnityEngine;

public class MoveCommand : IAICommand
{
    float timer;
    float maxTimer;
    Vector3 direction;
    RobotController robotController;
    public IAICommand next;

    public MoveCommand(Vector2 direction, float maxTimer)
    {
        timer = 0.0f;
        this.maxTimer = maxTimer;
        this.direction = direction.normalized;
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
            robotController.MoveDirection(direction);
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
        return new MoveCommand(new Vector2(0, 1), timer);
    }
    public static MoveCommand MoveDownCommand(float timer)
    {
        return new MoveCommand(new Vector2(0, -1), timer);
    }
    public static MoveCommand MoveLeftCommand(float timer)
    {
        return new MoveCommand(new Vector2(-1, 0), timer);
    }
    public static MoveCommand MoveRightCommand(float timer)
    {
        return new MoveCommand(new Vector2(1, 0), timer);
    }

}

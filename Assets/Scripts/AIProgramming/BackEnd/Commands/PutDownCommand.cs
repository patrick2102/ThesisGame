using UnityEngine;

public class PutDownCommand : AICommand
{

    public override ProgramStatus Step(RobotController robotController)
    {
        robotController.PutDown();
        return ProgramStatus.stopped;
    }
}

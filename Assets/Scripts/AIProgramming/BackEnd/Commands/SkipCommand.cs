using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCommand : AICommand
{

    public override ProgramStatus Step(RobotController rbc)
    {
        return ProgramStatus.stopped;
    }
}

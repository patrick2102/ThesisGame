using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICommand : MonoBehaviour
{
    public abstract ProgramStatus Step(RobotController rbc);
}

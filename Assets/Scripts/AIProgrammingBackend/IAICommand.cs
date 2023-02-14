using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAICommand
{
    ProgramStatus Run();

    IAICommand Next();
}

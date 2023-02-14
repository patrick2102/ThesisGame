using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProgram
{
    IAICommand firstCommand;
    IAICommand currentCommand;
    ProgramStatus status;


    public AIProgram(IAICommand initialCommand)
    {
        firstCommand = initialCommand;
        currentCommand = initialCommand;
        status = ProgramStatus.stopped;
    }

    public ProgramStatus StepProgram()
    {
        if (currentCommand == null)
            return ProgramStatus.stopped;

        var status = currentCommand.Run();
        if (status == ProgramStatus.stopped)
            currentCommand = currentCommand.Next();

        return ProgramStatus.running;
    }
}

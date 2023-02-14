using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfElseCommand<T> : IAICommand
{
    IAICommand ifCommand;
    IAICommand elseCommand;
    IAICommand next;

    RobotController robotController;
    T condition;
    Func<T, bool> condFunc;

    public IfElseCommand(Func<T, bool> condFunc, T condition, IAICommand ifCommand, IAICommand elseCommand) 
    {
        robotController = AIProgramManager.instance.robotController;
        this.condition = condition;
        this.condFunc = condFunc;
        this.ifCommand = ifCommand;
        this.elseCommand = elseCommand;
    }


    public IAICommand Next()
    {
        return next;
    }

    public ProgramStatus Run()
    {
        var res = condFunc(condition);

        if (res)
            next = ifCommand;
        else
            next = elseCommand;

        return ProgramStatus.stopped;
    }
}

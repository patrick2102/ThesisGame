using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICommand : MonoBehaviour, IAICommand
{
    public IAICommand next;
    public IAICommand prev;
    public IAICommand Next()
    {
        return next;
    }

    public IAICommand Prev()
    {
        return prev;
    }

    public void ConnectCommands(IAICommand nextCommand)
    {
        SetNext(nextCommand);
        nextCommand.SetPrev(this);
    }

    public void SetNext(IAICommand command)
    {
        next = command;
    }

    public void SetPrev(IAICommand command)
    {
        prev = command;
    }

    public abstract ProgramStatus Step();
}

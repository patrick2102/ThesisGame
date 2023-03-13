using System;

public class ContinueCommand : IAICommand
{
    IAICommand continueCommand; // The command that next is set to if the condition is true.
    IAICommand altCommand; // The command that next is set to if the condition is false.

    Func<bool> condition; // The function that is used to check for the condition. Has to be manually set outside of the class.
    public IAICommand next;
    IAICommand prev;

    public ContinueCommand(Func<bool> condition, IAICommand continueCommand, IAICommand altCommand)
    {
        this.condition = condition;
        this.continueCommand = continueCommand;
        this.altCommand = altCommand;
    }

    public IAICommand Next()
    {
        return next;
    }

    public IAICommand Prev()
    {
        return Prev();
    }

    /*
     * Calculates the condition and sets next based on the results.
     */
    public ProgramStatus Step()
    {
        ProgramStatus status;
        var cond = condition();

        if (cond)
        {
            status = continueCommand.Step();
            return status;
        }
        else
        {
            status = altCommand.Step();
            return ProgramStatus.running;
        }

    }

    public void SetNext(IAICommand command)
    {
        throw new NotImplementedException();
    }
    public void SetPrev(IAICommand command)
    {
        prev = command;
    }

    public void ConnectCommands(IAICommand command)
    {
        throw new NotImplementedException();
    }
}

using System;

/*
 * Command used for conditional logic.
 */
public class IfElseCommand : IAICommand
{
    IAICommand ifCommand; // The command that next is set to if the condition is true.
    IAICommand elseCommand; // The command that next is set to if the condition is false.
    IAICommand next;
    IAICommand prev;

    Func<bool> condition; // The function that is used to check for the condition. Has to be manually set outside of the class.

    public IfElseCommand(Func<bool> condition, IAICommand ifCommand, IAICommand elseCommand) 
    {
        this.condition = condition;
        this.ifCommand = ifCommand;
        this.elseCommand = elseCommand;
    }


    public IAICommand Next()
    {
        return next;
    }

    public IAICommand Prev()
    {
        return prev;
    }

    public void SetNext(IAICommand command)
    {
        throw new NotImplementedException();
    }

    public void SetPrev(IAICommand command)
    {
        prev = command;
    }

    /*
     * Calculates the condition and sets next based on the results.
     */
    public ProgramStatus Step()
    {
        var cond = condition();

        if (cond)
            next = ifCommand;
        else
            next = elseCommand;

        return ProgramStatus.stopped;
    }
}

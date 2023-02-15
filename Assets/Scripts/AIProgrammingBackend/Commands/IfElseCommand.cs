using System;

/*
 * Command used for conditional logic.
 */
public class IfElseCommand<T> : IAICommand
{
    IAICommand ifCommand; // The command that next is set to if the condition is true.
    IAICommand elseCommand; // The command that next is set to if the condition is false.
    IAICommand next;

    T variable; // The variable used in the condition function.
    Func<T, bool> condition; // The function that is used to check for the condition. Has to be manually set outside of the class.

    public IfElseCommand(Func<T, bool> condition, T variable, IAICommand ifCommand, IAICommand elseCommand) 
    {
        this.variable = variable;
        this.condition = condition;
        this.ifCommand = ifCommand;
        this.elseCommand = elseCommand;
    }


    public IAICommand Next()
    {
        return next;
    }

    /*
     * Calculates the condition and sets next based on the results.
     */
    public ProgramStatus Step()
    {
        var cond = condition(variable);

        if (cond)
            next = ifCommand;
        else
            next = elseCommand;

        return ProgramStatus.stopped;
    }
}

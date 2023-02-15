using System;

public class ContinueCommand<T> : IAICommand
{
    IAICommand continueCommand; // The command that next is set to if the condition is true.
    IAICommand altCommand; // The command that next is set to if the condition is false.

    T variable; // The variable used in the condition function.
    Func<T, bool> condition; // The function that is used to check for the condition. Has to be manually set outside of the class.
    public IAICommand next;

    public ContinueCommand(Func<T, bool> condition, T variable, IAICommand continueCommand, IAICommand altCommand)
    {
        this.variable = variable;
        this.condition = condition;
        this.continueCommand = continueCommand;
        this.altCommand = altCommand;
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
        ProgramStatus status;
        var cond = condition(variable);

        if (cond)
        {
            status = continueCommand.Step();
            next = continueCommand.Next();
        }
        else
        {
            status = altCommand.Step();
            next = altCommand.Next();
        }

        return status;
    }
}

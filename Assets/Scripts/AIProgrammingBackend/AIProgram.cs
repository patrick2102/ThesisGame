/*
 * A single AI program controls the flow between commands and gives statuses back to the AI Program.
 */
public class AIProgram
{
    IAICommand currentCommand;

    public AIProgram(IAICommand initialCommand)
    {
        currentCommand = initialCommand;
    }

    public ProgramStatus StepProgram()
    {
        if (currentCommand == null)
            return ProgramStatus.stopped;

        var status = currentCommand.Step();
        if (status == ProgramStatus.stopped)
            currentCommand = currentCommand.Next();

        return ProgramStatus.running;
    }
}

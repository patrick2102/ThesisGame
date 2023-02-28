/*
 * A single AI program controls the flow between commands and gives statuses back to the AI Program.
 */
using UnityEngine;

public class AIProgram
{
    public CircuitNode currentNode;

    public AIProgram(CircuitNode initialNode)
    {
        currentNode = initialNode;
    }

    public ProgramStatus StepProgram()
    {
        if (currentNode == null)
            return ProgramStatus.stopped;

        var status = currentNode.GetCommand().Step();
        if (status == ProgramStatus.stopped)
            currentNode = currentNode.GetNextNode();

        return ProgramStatus.running;
    }
}

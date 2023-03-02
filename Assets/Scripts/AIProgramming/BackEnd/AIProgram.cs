/*
 * A single AI program controls the flow between commands and gives statuses back to the AI Program.
 */
using UnityEngine;

public class AIProgram
{
    public CircuitNode firstNode;
    public CircuitNode currentNode;
    public CircuitNode initialNodeForResetting;

    public AIProgram(CircuitNode initialNode)
    {
        firstNode = initialNode;
        currentNode = initialNode;
        initialNodeForResetting = initialNode;
    }

    public void Reset()
    {
        currentNode = firstNode;
    }

    public ProgramStatus StepProgram(RobotController rbc)
    {
        if (currentNode == null)
        {
            Reset();
            return ProgramStatus.stopped;
        }

        var status = currentNode.GetCommand().Step(rbc);
        if (status == ProgramStatus.stopped)
            currentNode = currentNode.GetNextNode();

        return ProgramStatus.running;
    }

    public void ResetProgram()
    {
        currentNode = initialNodeForResetting;
    }
}

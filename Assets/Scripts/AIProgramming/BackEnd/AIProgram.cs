/*
 * A single AI program controls the flow between commands and gives statuses back to the AI Program.
 */
using UnityEngine;

public class AIProgram : MonoBehaviour
{
    public CircuitNode firstNode;
    public CircuitNode currentNode;
    public CircuitNode initialNodeForResetting;
    public static AIProgram activeProgram;
    public RobotController robotController;
    public int maxCommands;
    private int currentCommandCount;
    ProgramStatus status;

    private void Awake()
    {
        if (activeProgram == null)
        {
            activeProgram = this;
        }
        else if (activeProgram != this)
            Destroy(gameObject);
    }

    public void SetupProgram(RobotController rbc, CircuitNode initialNode, int maxCommands = 20)
    {
        robotController = rbc;
        firstNode = initialNode;
        currentNode = initialNode;
        initialNodeForResetting = initialNode;
        status = ProgramStatus.stopped;
        this.maxCommands = maxCommands;
    }

    public void FixedUpdate()
    {
        if (status == ProgramStatus.running)
        {
            status = activeProgram.StepProgram(robotController);
            RobotMovementVisualiser.instance.RenderPath(false);
        }
        else
        {
            RobotMovementVisualiser.instance.RenderPath(true);
        }
    }

    public ProgramStatus StartProgram()
    {
        if (status == ProgramStatus.stopped)
        {
            status = ProgramStatus.running;
        }
        return status;
    }

    public ProgramStatus StepProgram(RobotController rbc)
    {
        if (currentCommandCount >= maxCommands)
        {
            ResetProgram();
            return ProgramStatus.stopped;
        }

        if (currentNode == null)
        {
            ResetProgram();
            return ProgramStatus.stopped;
        }

        var status = currentNode.GetCommand().Step(rbc);
        if (status == ProgramStatus.stopped)    
        {
            currentCommandCount++;
            currentNode = currentNode.GetNextNode();
        }

        return ProgramStatus.running;
    }

    public void ResetProgram()
    {
        currentNode = initialNodeForResetting;
        currentCommandCount = 0;
        status = ProgramStatus.stopped;
    }
}

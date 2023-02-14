using UnityEngine;

public class AIProgramManager : MonoBehaviour
{
    AIProgram activeProgram;
    public RobotController robotController;
    private bool runningProgram;
    public static AIProgramManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void SetActiveProgram(AIProgram program)
    {
        activeProgram = program;
    }

    public void StartProgram()
    {
        runningProgram = true;
    }

    void Update()
    {
        if (activeProgram != null && runningProgram)
        {
            activeProgram.StepProgram();
        }
    }
}

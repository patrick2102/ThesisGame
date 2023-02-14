using UnityEngine;

/*
 * The AI Program Manager handles the changing of programs and running the various AI programs. 
 * For AI programs to work, this is the only script that is needed on a game object in a scene.
 */
public class AIProgramManager : MonoBehaviour
{
    AIProgram activeProgram; // Program currently set to run if runningProgram = true.
    bool runningProgram; // Bool for stopping and starting programs. 
    public RobotController robotController; // Robot controller that can be accessed by the various commands.

    public static AIProgramManager instance; // Instance used to ensure singleton behavior.

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    //Set the active program, which will run in Update()
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
        // If activeProgram is set and runningProgram=true, then step program each frame.
        if (activeProgram != null && runningProgram)
        {
            activeProgram.StepProgram();
        }
    }
}

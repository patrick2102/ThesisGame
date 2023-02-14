using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIProgram : MonoBehaviour
{
    [SerializeField] RobotController robotController;
    [SerializeField] AIProgramManager AIProgramManager;

    void Start()
    {
        var moveUp1 = MoveCommand.MoveUpCommand(2);
        var moveLeft1 = MoveCommand.MoveLeftCommand(2);
        var moveDown1 = MoveCommand.MoveDownCommand(2);
        var moveRight1 = MoveCommand.MoveRightCommand(2);

        moveUp1.next = moveLeft1;
        moveLeft1.next = moveDown1;
        moveDown1.next = moveRight1;
        moveRight1.next = moveUp1;



        var program = new AIProgram(moveUp1);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }
}

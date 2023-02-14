using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIProgram : MonoBehaviour
{
    [SerializeField] RobotController robotController;
    [SerializeField] AIProgramManager AIProgramManager;

    void Start()
    {
        IfElseCommandTest();
    }

    void MovementTest()
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
    void IfElseCommandTest()
    {
        var moveUp = MoveCommand.MoveUpCommand(0.1f);
        var moveDown = MoveCommand.MoveDownCommand(0.1f);

        Func<Transform, bool> above = v => (v.position.y > 5);
        Func<Transform, bool> below = v => (v.position.y < -5);

        var upCondition = new IfElseCommand<Transform>(above, robotController.transform, moveDown, moveUp);
        var downCondition = new IfElseCommand<Transform>(below, robotController.transform, moveUp, moveDown);


        moveUp.next = upCondition;
        moveDown.next = downCondition;

        var program = new AIProgram(moveUp);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }

}

using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestAIProgram : MonoBehaviour
{
    [SerializeField] RobotController robotController;
    [SerializeField] AIProgramManager AIProgramManager;

    void Start()
    {
        //IfElseCommandTest();
        MoveToCommandTest();
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

    void MoveToCommandTest()
    {
        //Func<Transform, bool> above = v => (v.position.y > 5);
        Func<Vector2> mousePosition = () => Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Func<Vector2> mouseDirection = () => Camera.main.ScreenToWorldPoint(Input.mousePosition) - robotController.transform.position;

        Func<Vector2> goalPosition = () => new Vector3(0,5,0) - robotController.transform.position;


        var moveTo = MoveCommand.MoveTo(goalPosition, 0.1f);
        var moveFrom = MoveCommand.MoveFrom(mouseDirection, 0.1f);

        float closestAllowed = 2.0f;
        Func<Transform, bool> tooClose = v => (v.position - (Vector3)mousePosition()).magnitude < closestAllowed;

        var mouseTooCloseCondition = new IfElseCommand<Transform>(tooClose, robotController.transform, moveFrom, moveTo);

        moveTo.next = mouseTooCloseCondition;
        moveFrom.next = mouseTooCloseCondition;

        var program = new AIProgram(moveTo);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }


}

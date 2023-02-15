using System;
using UnityEditor.Tilemaps;
using UnityEngine;


/*
 * Class used for testing various AI Programs and commands.
 */
public class TestAIProgram : MonoBehaviour
{
    [SerializeField] RobotController robotController;
    [SerializeField] AIProgramManager AIProgramManager;

    void Start()
    {
        //DirectionTest();
        //IfElseCommandTest();
        //MoveToCommandTest();
        ContinueCommandTest();
    }

    /*
     * Simple test for moving in a square and looping around.
     */
    void DirectionTest()
    {
        var moveUp1 = DirectionCommand.DirectionUpCommand(2);
        var moveLeft1 = DirectionCommand.DirectionLeftCommand(2);
        var moveDown1 = DirectionCommand.DirectionDownCommand(2);
        var moveRight1 = DirectionCommand.DirectionRightCommand(2);

        moveUp1.next = moveLeft1;
        moveLeft1.next = moveDown1;
        moveDown1.next = moveRight1;
        moveRight1.next = moveUp1;



        var program = new AIProgram(moveUp1);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }

    /*
     * Test for the IfElseCommand, where the robot moves up and down within a range.
     */
    void IfElseCommandTest()
    {
        var moveUp = DirectionCommand.DirectionUpCommand(0.1f);
        var moveDown = DirectionCommand.DirectionDownCommand(0.1f);

        Func<bool> above = () => (robotController.transform.position.y > 5);
        Func<bool> below = () => (robotController.transform.position.y < -5);

        var upCondition = new IfElseCommand(above, moveDown, moveUp);
        var downCondition = new IfElseCommand(below, moveUp, moveDown);


        moveUp.next = upCondition;
        moveDown.next = downCondition;

        var program = new AIProgram(moveUp);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }

    /*
     * More complicated test for changing direction vectors and conditionals.
     */
    void MoveToCommandTest()
    {
        //Func<Transform, bool> above = v => (v.position.y > 5);
        Func<Vector2> mousePosition = () => Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Func<Vector2> mouseDirection = () => Camera.main.ScreenToWorldPoint(Input.mousePosition) - robotController.transform.position;

        Func<Vector2> goalPosition = () => new Vector3(0, 5, 0) - robotController.transform.position;


        var moveTo = DirectionCommand.DirectionTo(goalPosition, 0.1f);
        var moveFrom = DirectionCommand.DirectionFrom(mouseDirection, 0.1f);

        float closestAllowed = 2.0f;
        Func<bool> tooClose = () => (robotController.transform.position - (Vector3)mousePosition()).magnitude < closestAllowed;

        var mouseTooCloseCondition = new IfElseCommand(tooClose, moveFrom, moveTo);

        moveTo.next = mouseTooCloseCondition;
        moveFrom.next = mouseTooCloseCondition;

        var program = new AIProgram(moveTo);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }

    /*
    * More complicated test for changing direction vectors and conditionals.
    */
    void ContinueCommandTest()
    {
        Func<Vector2> mousePosition = () => Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Func<Vector2> mouseDirection = () => Camera.main.ScreenToWorldPoint(Input.mousePosition) - robotController.transform.position;

        Func<Vector2> goalPosition = () => new Vector3(0, 5, 0) - robotController.transform.position;

        var moveFrom = DirectionCommand.DirectionFrom(mouseDirection, 0.1f);

        var topRight = new Vector2(5, 5);
        var topLeft = new Vector2(-5, 5);
        var bottomLeft = new Vector2(-5, -5);
        var bottomRight = new Vector2(5, -5);

        var stopDist = 0.5f;

        var moveUp1 = MoveCommand.MoveTo(() => topRight, stopDist);
        var moveLeft1 = MoveCommand.MoveTo(() => topLeft, stopDist);
        var moveDown1 = MoveCommand.MoveTo(() => bottomLeft, stopDist);
        var moveRight1 = MoveCommand.MoveTo(() => bottomRight, stopDist);

        float closestAllowed = 2.0f;
        Func<bool> tooClose = () => (robotController.transform.position - (Vector3)mousePosition()).magnitude > closestAllowed;

        var moveUp1Continue = new ContinueCommand(tooClose, moveUp1, moveFrom);
        var moveLeft1Continue = new ContinueCommand(tooClose, moveLeft1, moveFrom);
        var moveDown1Continue = new ContinueCommand(tooClose, moveDown1, moveFrom);
        var moveRight1Continue = new ContinueCommand(tooClose, moveRight1, moveFrom);

        moveUp1Continue.next = moveLeft1Continue;
        moveLeft1Continue.next = moveDown1Continue;
        moveDown1Continue.next = moveRight1Continue;
        moveRight1Continue.next = moveUp1Continue;

        var program = new AIProgram(moveUp1Continue);

        AIProgramManager.SetActiveProgram(program);
        AIProgramManager.StartProgram();
    }

}

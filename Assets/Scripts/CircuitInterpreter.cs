using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitInterpreter : MonoBehaviour
{
    public static void WriteString()
    {
        string path = Application.dataPath + "/txt-level-files/test.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    public static NodeType[,] ReadString()
    {
        string path = Application.dataPath + "/txt-level-files/test.txt";
        //Read the text directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        
        String txtLevel = reader.ReadToEnd();

        var array = txtLevel.Split('\n');

        var curcuitBoardColumns = array[0].Length - 1; //Minus 1 to not count the "\n" character
        var curcuitBoardRows = array.Length;

        NodeType[,] gridRepresentation;
        //Vector2 grideSize;

        gridRepresentation = new NodeType[curcuitBoardColumns, curcuitBoardRows]; //TODO replace with tool that can generate gridnodes
        //var circuitNodes = new CircuitNode[(int)grideSize.x, (int)grideSize.y];

        for (int i = 0; i < curcuitBoardColumns; i++)
        {
            for (int j = 0; j < curcuitBoardRows; j++)
            {
                char c = array[i][j];

                switch (c)
                {
                    case '@':
                        //Blank space
                        break;
                    case 'X':
                        //Unusable Node
                        break;
                    case 'I':
                        //Input node
                        gridRepresentation[i, j] = NodeType.InputNode;
                        break;
                    case 'O':
                        //Output node
                        gridRepresentation[i, j] = NodeType.OutputNode;
                        break;
                    case 'N':
                        //Normal "Circuit" node
                        gridRepresentation[i, j] = NodeType.CircuitNode;
                        break;
                }
            }
        }

        reader.Close();

        return gridRepresentation;
    }
}

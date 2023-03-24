using System;
using System.IO;
using UnityEngine;

public class CircuitInterpreter : MonoBehaviour
{
    public static void WriteString()
    {
        string path = Application.dataPath + "/txt-level-files/4x4.txt";
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
        string path = Application.dataPath + "/txt-level-files/4x4.txt";
        //Read the text directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        
        String txtLevel = reader.ReadToEnd();

        // This does not function on Unix systems, as they have different line endings
        var array = txtLevel.Split('\n');

        var xLength = array[0].Length -1; //Minus 1 to not count the "\r" character

        var yLength = array.Length;

        NodeType[,] gridRepresentation;

        gridRepresentation = new NodeType[xLength, yLength]; //TODO replace with tool that can generate gridnodes
        //var circuitNodes = new CircuitNode[(int)grideSize.x, (int)grideSize.y];

        for (int i = 0; i < yLength; i++)
        {
            for (int j = 0; j < xLength; j++)
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
                        gridRepresentation[j, i] = NodeType.InputNode;
                        break;
                    case 'O':
                        //Output node
                        gridRepresentation[j, i] = NodeType.OutputNode;
                        break;
                    case 'N':
                        //Normal "Circuit" node
                        gridRepresentation[j, i] = NodeType.CircuitNode;
                        break;
                }
            }
        }

        reader.Close();

        return gridRepresentation;
    }
}

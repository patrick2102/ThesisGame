using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandList : MonoBehaviour
{
    public List<AICommand> commands;
    public CommandButton buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var command in commands)
        {
            var button = Instantiate(buttonPrefab);
            button.command = command;
            button.commandButtonText.text = command.name;
            button.transform.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

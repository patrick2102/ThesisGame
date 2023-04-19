using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandScreen : MonoBehaviour
{
    public List<AICommand> commands;
    public CommandButton buttonPrefab;
    public Transform content;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var command in commands)
        {
            var button = Instantiate(buttonPrefab);
            button.command = command;
            button.commandButtonText.text = command.name;
            button.commandButtonImage.sprite = command.commandImage.sprite;
            button.transform.SetParent(content, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

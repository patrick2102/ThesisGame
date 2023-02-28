using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Interface for the AICommands.
 */
public interface IAICommand
{
    // The method for 
    ProgramStatus Step();

    IAICommand Next();

    IAICommand Prev();

    void SetNext(IAICommand command);
    void SetPrev(IAICommand command);

    void ConnectCommands(IAICommand command);
}

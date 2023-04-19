using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AICommand : MonoBehaviour
{
    public float maxTimer;
    public Image commandImage;

    private void Start()
    {
        commandImage = GetComponent<Image>();
    }

    public abstract ProgramStatus Step(RobotController rbc);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerTutorialText : MonoBehaviour
{
    [SerializeField] private string TutorialtextForUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == GameObjectTags.Player.ToString())
        {
            UIManager.instance.SetUI(UIManager.UIState.tutorialScreen);
            UIManager.instance.SetTutorialText(TutorialtextForUI);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == GameObjectTags.Player.ToString())
        {
            UIManager.instance.SetUI(UIManager.UIState.closed);
        }
        //UIManager.instance.SetInteractScreen(withinRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Transform mousePosition;

    // Update is called once per frame
    void Update()
    {
        mousePosition.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

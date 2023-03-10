using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerBase : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTriggerEnter(collision.gameObject.tag);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTriggerEnter(collision.tag);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleTriggerExit(collision.gameObject.tag);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleTriggerExit(collision.tag);
    }

    public abstract void HandleTriggerEnter(string tag);
    public abstract void HandleTriggerExit(string tag);
}

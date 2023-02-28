using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerBase : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTrigger(collision.gameObject.tag);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTrigger(collision.tag);
    }

    public abstract void HandleTrigger(string tag);
}

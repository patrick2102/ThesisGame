using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnMonsterAI : MonoBehaviour
{
    public float aggroRangeRadius;
    public float rotateSpeed;
    public float alignAngleRange;

    // The multiplier determining "thrust"-speed
    public int moveMultiplier;

    // Saving the player-tagged and robot-tagged gameObjects to only have to look for them once
    // private GameObject playerObject;
    // provate GameObject robotObject;

    private bool FacingTarget;

    private Rigidbody2D turnMonsterRB2D;
    private Transform childSprite;

    // This could inherit from a more general sort of monster class, but since we have no other yet
    // i'll just make it non-general for now and adapt it should we find a good case for generality

    private void Awake()
    {
        // playerObject = GameObject.FindWithTag("Player");
        turnMonsterRB2D = gameObject.GetComponent<Rigidbody2D>();
        childSprite = gameObject.GetComponent<Transform>().GetChild(0);
        // No idea why + 1.5f is necessary, but it makes the circle line up perfectly...
        childSprite.localScale = childSprite.localScale * (aggroRangeRadius + 1.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        CheckAggroRange();
        if (FacingTarget)
        {
            MoveForward();
        }
    }

    void CheckAggroRange()
    {
        var fun = Physics2D.OverlapCircleAll(turnMonsterRB2D.position, aggroRangeRadius);

        float closestDistance = 99;
        GameObject objectWithClosestDistance = null;
        foreach(Collider2D col in fun)
        {
            float temp = CalcDistanceToTarget(col.gameObject.transform.position);
            switch (col.gameObject.tag)
            {
                case "Player":
                    if (temp < closestDistance)
                    {
                        closestDistance = temp;
                        objectWithClosestDistance = col.gameObject;
                    }
                    break;
                case "Robot":
                    if (temp < closestDistance)
                    {
                        closestDistance = temp;
                        objectWithClosestDistance = col.gameObject;
                    }
                    break;
            }
        }

        FacingTarget = false;

        if (closestDistance < 99)
        {
            TurnToTargetThenMove(objectWithClosestDistance.transform.position);
        }
    }

    float CalcDistanceToTarget(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, turnMonsterRB2D.position);
    }

    void TurnToTargetThenMove(Vector3 targetPosition)
    {
        float radianAngle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x);
        float degreeAngle = Mathf.Rad2Deg * radianAngle;
        
        // Added - 90 here as it gives the correct result. Might be problematic though, lets keep it in mind
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, degreeAngle - 90));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < alignAngleRange)
        {
            FacingTarget = true;
        }
        else
        {
            FacingTarget = false;
        }
    }

    void MoveForward()
    {
        turnMonsterRB2D.velocity = (transform.up).normalized * moveMultiplier;
    }
}

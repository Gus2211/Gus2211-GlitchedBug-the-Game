using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider))]
public class OneWayBoxCollider : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;
    [SerializeField, Range(1.0f, 2.0f)] private float triggerScale = 1.25f;
    private new BoxCollider invisWall = null;

    private BoxCollider collisionCheckTrigger = null;

    private void Awake() 
    {
        invisWall = GetComponent<BoxCollider>();
        invisWall.isTrigger = false;

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = invisWall.size * triggerScale;
        collisionCheckTrigger.center = invisWall.center;
        collisionCheckTrigger.isTrigger = true;
    }

    private void OnTriggerStay(Collider other) 
    {
        if (Physics.ComputePenetration(
            collisionCheckTrigger, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth
            ))
        {
            Vector3 direction;
            if(localDirection)
            {
                direction = transform.TransformDirection(entryDirection.normalized);
            }
            else
            {
                direction = entryDirection;
            }
            float dot = Vector3.Dot(direction, collisionDirection);
            if (dot < 0)
            {
                Physics.IgnoreCollision(invisWall, other, false);
            }
            else
            {
                Physics.IgnoreCollision(invisWall, other, true);
            }
        }
    }
    private void OnDrawGizmosSelected() 
    {

        Vector3 direction;
        if(localDirection)
        {
            direction = transform.TransformDirection(entryDirection.normalized);
        }
        else
        {
            direction = entryDirection;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -direction);
    }
}
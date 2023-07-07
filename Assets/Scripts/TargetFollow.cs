using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offsetVector;
    [SerializeField] float smoothSpeed;
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 targetPosition = target.position;
            if (offsetVector != Vector3.zero)
            {
                targetPosition += offsetVector;
            }
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
    
    public Vector3 GetOffsetVector()
    {
        return offsetVector;
    }
    
    public void SetOffsetVector(Vector3 newVector)
    {
        offsetVector = newVector;
    }
}

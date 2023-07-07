using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offsetVector;
    
    void LateUpdate()
    {
        if (target)
        {
            Vector3 targetPosition = target.position;
            if (offsetVector != Vector3.zero)
            {
                targetPosition.x += offsetVector.x;
                targetPosition.y += offsetVector.y;
                targetPosition.z += offsetVector.z;
            }
            transform.position = targetPosition;
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

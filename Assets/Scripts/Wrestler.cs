using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public abstract class Wrestler : MonoBehaviour
{
    public int Score { get; set; }
    public int ForceFactor { get; set; }
    public float MovementSpeed { get; set; }
    public float WeakPivotRotationSpeed { get; set; }
    
    [Header("Starting Values")]
    [SerializeField] protected int initForceFactor;
    [SerializeField] protected float initMovementSpeed;
    [SerializeField] protected float initWeakPivotRotationSpeed;
    
    protected abstract void InitValues(); // Abstract method
    
    protected virtual void PushAttack(Rigidbody enemyRigidbody)
    {
        Vector3 forceDir = enemyRigidbody.transform.position - transform.position;
        enemyRigidbody.AddForce(forceDir * ForceFactor, ForceMode.Impulse);
    }
}

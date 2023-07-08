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
    public bool IsBeingPushed { get; set; }
    
    [Header("Starting Values")]
    [SerializeField] protected int initForceFactor;
    [SerializeField] protected float initMovementSpeed;
    [SerializeField] protected float initWeakPivotRotationSpeed;

    [SerializeField] private float pushedTime;
    protected abstract void InitValues(); // Abstract method
    
    protected virtual void PushAttack(Wrestler wrestler)
    {
        Rigidbody enemyRigidbody = wrestler.GetComponent<Rigidbody>();
        Vector3 forceDir = enemyRigidbody.transform.position - transform.position;
        enemyRigidbody.AddForce(forceDir.normalized * ForceFactor, ForceMode.Impulse);
        wrestler.StartPushedCor();
    }

    private void StartPushedCor()
    {
        StartCoroutine(StartBeingPush());
    }
    
    IEnumerator StartBeingPush()
    {
        IsBeingPushed = true;
        yield return new WaitForSeconds(pushedTime);
        IsBeingPushed = false;
    }   
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Wrestler : MonoBehaviour
{
    public int Score { get; set; }
    public float ForceFactor { get; set; }
    public float MovementSpeed { get; set; }
    public float WeakPivotRotationSpeed { get; set; }
    public bool IsBeingPushed { get; set; }
    
    [Header("Starting Values")]
    [SerializeField] protected float initForceFactor;
    [SerializeField] protected float initMovementSpeed;
    [SerializeField] protected float initWeakPivotRotationSpeed;
    [Space(20)]
    [SerializeField] private float pushedTime;
    [SerializeField] protected Transform weakPivotTransform;
    protected UnityEvent onDie;
    protected Rigidbody wrestlerRigidbody;

    #region Abstract Methods
    protected abstract void CollectPowerUp(PowerUp powerUp);
    protected abstract void RotateWeakPivot();
    
    #endregion
    
    #region Virtual Methods
    protected virtual void InitValues()
    { 
        MovementSpeed = initMovementSpeed;
        ForceFactor = initForceFactor;
        WeakPivotRotationSpeed = initWeakPivotRotationSpeed;
        Score = 0;
        onDie = new UnityEvent();
    }
    protected virtual void PushAttack(Wrestler wrestler)
    {
        Rigidbody enemyRigidbody = wrestler.GetComponent<Rigidbody>();
        Vector3 forceDir = enemyRigidbody.transform.position - transform.position;
        enemyRigidbody.AddForce(forceDir.normalized * ForceFactor, ForceMode.Impulse);
        wrestler.StartPushedCor();
    }
    
    protected virtual void UpdateForceFactor(float forcePoint)
    {
        ForceFactor += forcePoint;
    }
    protected virtual void UpdateWrestlerScore(int point)
    {
        Score += point;
    }
    protected virtual void UpdateWrestlerScale(float multiplier)
    {
        transform.localScale *= multiplier;
    }
    
    #endregion
    
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

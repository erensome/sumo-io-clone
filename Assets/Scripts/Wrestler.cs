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
    public virtual void PushAttack(Wrestler wrestler)
    {
        Rigidbody enemyRigidbody = wrestler.GetComponent<Rigidbody>();
        Vector3 forceDir = enemyRigidbody.transform.position - transform.position;
        enemyRigidbody.AddForce(forceDir.normalized * ForceFactor, ForceMode.Impulse);
        wrestler.StartPushedCor();
    }
    // Rotates the Weak Point Pivot object in the Y-axis according to the player's movement direction.
    // Thus, Weak Point object will always stay at back of the player.
    protected virtual void RotateWeakPivot(Vector2 movementDir)
    {
        float angle = Mathf.Atan2(movementDir.x, movementDir.y) * Mathf.Rad2Deg;
        var rotation = weakPivotTransform.rotation;
        rotation = Quaternion.Slerp(rotation, Quaternion.Euler(0f, angle, 0f),
            WeakPivotRotationSpeed * Time.deltaTime);
        weakPivotTransform.rotation = rotation;
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

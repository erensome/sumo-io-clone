using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Wrestler
{
    private Transform _targetTransform;
    private Vector3 _targetPos;
    public float RoamingSpeed;
    
    #region Wrestler Overrides
    protected override void InitValues()
    {
        base.InitValues();
        onDie.AddListener(DeleteEnemyOnTheList);
        onDie.AddListener(GameManager.Instance.CheckEnemyCount);
    }
    
    protected override void CollectPowerUp(PowerUp powerUp)
    {
        base.UpdateWrestlerScore(powerUp.scorePoint);
        base.UpdateForceFactor(powerUp.forcePoint);
        base.UpdateWrestlerScale(powerUp.scaleMultiplier);
        SpawnManager.Instance.DecreaseAndCheckPowerUps();
    }
    // Rotates the Weak Point Pivot object in the Y-axis according to the player's movement direction.
    // Thus, Weak Point object will always stay at back of the player.
    protected override void RotateWeakPivot()
    {
        Vector3 dir = _targetPos - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        var rotation = weakPivotTransform.rotation;
        rotation = Quaternion.Slerp(rotation, Quaternion.Euler(0f, angle, 0f),
            WeakPivotRotationSpeed * Time.deltaTime);
        weakPivotTransform.rotation = rotation;
    }
    
    #endregion
    
    private void Awake()
    {
        InitValues();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _targetPos = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-10f, 10f));
        wrestlerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateWeakPivot();
    }

    private void FixedUpdate()
    {
        Vector3 dir = _targetPos - transform.position;
        wrestlerRigidbody.AddForce(dir.normalized * RoamingSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Wrestler wrestler = other.gameObject.GetComponent<Wrestler>();
            if (ForceFactor > wrestler.ForceFactor)
            {
                base.PushAttack(wrestler);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            PowerUp powerUpRef = other.GetComponent<PowerUp>();
            CollectPowerUp(powerUpRef);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Death Zone"))
        {
            onDie?.Invoke();
            Destroy(gameObject);
        }
    }

    private void DeleteEnemyOnTheList()
    {
        GameManager.Instance.allWrestlers.Remove(this);
    }
    
    private void Roaming()
    {
        
    }

    private Transform GetClosestWrestler(Transform[] wrestlers)
    {
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform wrestlerTransform in wrestlers)
        {
            Vector3 distanceToTarget = wrestlerTransform.position - currentPosition;
            float distanceSqr = distanceToTarget.sqrMagnitude;
            if (closestDistanceSqr > distanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestTarget = wrestlerTransform;
            }
        }
        
        return closestTarget;
    }
}

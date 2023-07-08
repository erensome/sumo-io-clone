using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Wrestler
{
    private Transform _targetTransform;
    
    protected override void InitValues()
    {
        this.MovementSpeed = initMovementSpeed;
        this.ForceFactor = initForceFactor;
        this.WeakPivotRotationSpeed = initWeakPivotRotationSpeed;
        this.Score = 0;
    }
    
    private void Awake()
    {
        InitValues();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            base.PushAttack(playerRb);
        }
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

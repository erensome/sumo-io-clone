using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Wrestler
{
    private Transform _targetTransform;
    private Vector3 _targetPos;
    private Vector3 _movementDir;
    
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
        _movementDir = _targetPos - transform.position;
        // Convert 3D direction vector to 2D direction vector 
        Vector2 inputDir = new Vector2(_movementDir.x, _movementDir.z);
        RotateWeakPivot(inputDir);
    }

    private void FixedUpdate()
    {
        wrestlerRigidbody.AddForce(_movementDir.normalized * MovementSpeed);
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

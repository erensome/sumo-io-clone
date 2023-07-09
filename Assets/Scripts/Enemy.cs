using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Wrestler
{
    private Transform _targetTransform;
    private Vector3 _movementDir;
    private Vector3 _roamingPosition;
    private State _enemyState;
    private Wrestler _lastHit;
    
    private enum State
    {
        Roaming,        // Roaming around randomly
        Chase,          // Chase another random AI
        AttackPlayer    // Chase Player (to avoid AI clustering bug)
    }
    
    #region Wrestler Overrides
    protected override void InitValues()
    {
        base.InitValues();
        onDie.AddListener(DeleteEnemyOnTheList);
        onDie.AddListener(AddScoreToPlayer);
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
        ChangeToRoaming();
        wrestlerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_enemyState)
        {
            case State.Roaming:
                _movementDir = _roamingPosition - transform.position;
                float reachedDistanceSqr = 10f;
                if (Vector3.Distance(_roamingPosition,transform.position) < reachedDistanceSqr)
                {
                    ChangeToChase();
                }
                break;
            case State.Chase:
                _movementDir = _targetTransform.position - transform.position;
                break;
            case State.AttackPlayer:
                _movementDir = _targetTransform.position - transform.position;
                break;
        }
        // Convert 3D direction vector to 2D direction vector 
        Vector2 inputDir = new Vector2(_movementDir.normalized.x, _movementDir.normalized.z);
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
            _lastHit = wrestler;
            if (ForceFactor > wrestler.ForceFactor)
            {
                base.PushAttack(wrestler);
            }
            if (_enemyState == State.Chase)
            {
                ChangeToAttackPlayer();
            }
            else if (_enemyState == State.AttackPlayer)
            {
                ChangeToRoaming();
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Wrestler wrestler = other.gameObject.GetComponent<Wrestler>();
            PreventClustering(wrestler, 2f);
            PreventClustering(this, 2f);
        }
        if (_enemyState == State.Chase)
        {
            ChangeToAttackPlayer();
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

    private void AddScoreToPlayer()
    {
        if (_lastHit == GameManager.Instance.playerController)
        {
            GameManager.Instance.playerController.UpdateWrestlerScore(300);
        }
    }
    
    private void DeleteEnemyOnTheList()
    {
        GameManager.Instance.allWrestlers.Remove(this);
    }

    private void PreventClustering(Wrestler wrestler, float preventForce)
    {
        Rigidbody enemyRigidbody = wrestler.GetComponent<Rigidbody>();
        Vector3 forceDir = enemyRigidbody.transform.position - transform.position;
        enemyRigidbody.AddForce(forceDir.normalized * preventForce, ForceMode.Impulse);
    }

    private Vector3 GetRoamingPosition()
    {
        float x = Random.Range(-GameManager.Instance.gameArenaRadius, GameManager.Instance.gameArenaRadius);
        float z = Random.Range(-GameManager.Instance.gameArenaRadius, GameManager.Instance.gameArenaRadius);
        return new Vector3(x, 0f, z);
    }

    private void ChangeToRoaming()
    {
        _enemyState = State.Roaming;
        _roamingPosition = GetRoamingPosition();
    }

    private void ChangeToChase()
    {
        _enemyState = State.Chase;
        _targetTransform = GetClosestWrestler(GameManager.Instance.allWrestlers);
    }

    private void ChangeToAttackPlayer()
    {
        _enemyState = State.AttackPlayer;
        _targetTransform = GameManager.Instance.playerController.transform;
    }

    private Transform GetClosestWrestler(List<Wrestler> wrestlers)
    {
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (var wrestler in wrestlers)
        {
            if (wrestler != this)
            {
                var wrestlerTransform = wrestler.transform;
                Vector3 distanceToTarget = wrestlerTransform.position - currentPosition;
                float distanceSqr = distanceToTarget.sqrMagnitude;
                if (closestDistanceSqr > distanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestTarget = wrestlerTransform;
                }
            }
        }
        return closestTarget;
    }
}

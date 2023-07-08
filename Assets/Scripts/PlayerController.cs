using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Wrestler
{
    private Rigidbody _playerRigidbody;
    private Vector2 _movementDir;
    [Space(20)]
    [SerializeField] Transform weakPivotTransform;
    [SerializeField] TargetFollow cameraTargetFollow;

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
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (!IsBeingPushed)
        {
            _movementDir = new Vector2(horizontalInput, verticalInput);
        }
        // If player doesn't move then don't rotate the object to start point.
        if (_movementDir != Vector2.zero) 
        {
            RotateWeakPivot();
        }
        if (transform.position.y < -2f)
        {
            Debug.Log("Game Over!");
        }
    }

    private void FixedUpdate()
    {
        if (_movementDir != Vector2.zero && !IsBeingPushed)
        {
            _playerRigidbody.velocity =
                new Vector3(_movementDir.x, -0.5f, _movementDir.y) * MovementSpeed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
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
            UpdateCameraOffset(powerUpRef.scaleMultiplier);
            UpdateForceFactor(powerUpRef.forcePoint);
            UpdatePlayerScale(powerUpRef.scaleMultiplier);
            UpdatePlayerScore(powerUpRef.scorePoint);
            Destroy(other.gameObject);
        }
    }

    // Rotates the Weak Point Pivot object in the Y-axis according to the player's movement direction.
    // Thus, Weak Point object will always stay at back of the player.
    private void RotateWeakPivot()
    {
        float angle = Mathf.Atan2(_movementDir.x, _movementDir.y) * Mathf.Rad2Deg;
        var rotation = weakPivotTransform.rotation;
        rotation = Quaternion.Slerp(rotation, Quaternion.Euler(0f, angle, 0f),
            WeakPivotRotationSpeed * Time.deltaTime);
        weakPivotTransform.rotation = rotation;
    }

    private void UpdateCameraOffset(float multiplier)
    {
        cameraTargetFollow.SetOffsetVector(cameraTargetFollow.GetOffsetVector() * multiplier);
    }

    private void UpdatePlayerScale(float multiplier)
    {
        transform.localScale *= multiplier;
    }
    private void UpdatePlayerScore(int point)
    {
        Score += point;
    }

    private void UpdateForceFactor(int forcePoint)
    {
        ForceFactor += forcePoint;
    }
}

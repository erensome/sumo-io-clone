using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerController : Wrestler
{
    private Vector2 _movementDir;
    [Space(20)]
    [SerializeField] TargetFollow cameraTargetFollow;
    [SerializeField] FixedJoystick fixedJoystick;
    public bool IsWin { get; private set; } = true;
    
    #region Wrestler Overrides
    protected override void InitValues()
    {
        base.InitValues();
        onDie.AddListener(GameManager.Instance.FinishGame);
    }
    protected override void CollectPowerUp(PowerUp powerUp)
    {
        base.UpdateForceFactor(powerUp.forcePoint);
        base.UpdateWrestlerScale(powerUp.scaleMultiplier);
        base.UpdateWrestlerScore(powerUp.scorePoint);
        UpdateCameraOffset(powerUp.scaleMultiplier);
        SpawnManager.Instance.DecreaseAndCheckPowerUps();
    }
    // Rotates the Weak Point Pivot object in the Y-axis according to the player's movement direction.
    // Thus, Weak Point object will always stay at back of the player.
    protected override void RotateWeakPivot()
    {
        float angle = Mathf.Atan2(_movementDir.x, _movementDir.y) * Mathf.Rad2Deg;
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
        wrestlerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //float verticalInput = Input.GetAxis("Vertical");
        //float horizontalInput = Input.GetAxis("Horizontal");
        
        if (!IsBeingPushed)
        {
            _movementDir = new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical);
        }
        // If player doesn't move then don't rotate the object to start point.
        if (_movementDir != Vector2.zero) 
        {
            RotateWeakPivot();
        }
    }

    private void FixedUpdate()
    {
        if (_movementDir != Vector2.zero && !IsBeingPushed)
        {
            wrestlerRigidbody.velocity =
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
            CollectPowerUp(powerUpRef);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Death Zone"))
        {
            IsWin = false;
            onDie?.Invoke();
        }
    }

    private void UpdateCameraOffset(float multiplier)
    {
        cameraTargetFollow.SetOffsetVector(cameraTargetFollow.GetOffsetVector() * multiplier);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Score { get; private set; } = 0;
    public int forceFactor;
    
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
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 forceDir = other.transform.position - transform.position;
            playerRb.AddForce(forceDir * forceFactor, ForceMode.Impulse);
        }
    }
}

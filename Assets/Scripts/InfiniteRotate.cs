using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f,15f,0f) * rotateSpeed * Time.deltaTime);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speedForward = 8f; 
    public float speedRight = 1.0f;

    //private Rigidbody rigidbody;
    private float dirRight;

    private float groundScale;

    private void Awake()
    {
        //rigidbody= GetComponent<Rigidbody>();
        groundScale = GameObject.FindWithTag("Ground").transform.localScale.x * 5;

    }

    private void Update()
    {
        dirRight = Input.GetAxis("Horizontal");

    }
    private void FixedUpdate()
    {
        float right = dirRight * speedRight;
        //rigidbody.velocity = new Vector3(right, 0, speedForward);
        //if (Math.Abs(transform.position.x)>=groundScale)
        //    rigidbody.velocity = new Vector3(-right, 0, speedForward);

        Vector3 translation = new Vector3(right, 0, speedForward) * Time.fixedDeltaTime;
        transform.Translate(translation);
        if (Math.Abs(transform.position.x) >= groundScale)
            transform.position = new Vector3(transform.position.x - translation.x , transform.position.y , transform.position.z);
    }
    
    
}

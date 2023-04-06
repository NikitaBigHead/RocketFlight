using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speedForward = 8f; 
    public float speedRight = 1.0f;

    //private Rigidbody rigidbody;
    private float dirRight;

    private float groundScale;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI highScore;
    private int countZone;
    private float distanceBetweenZones;

    private int count = 0;

    private void Awake()
    {
        //rigidbody= GetComponent<Rigidbody>();
        groundScale = GameObject.FindWithTag("Ground").transform.localScale.x * 5;

        scoreText = GameObject.Find("UI").transform.Find("UserUI").
           transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>();

        highScore = GameObject.Find("UI").transform.Find("UserUI").
           transform.Find("Record").GetComponentInChildren<TextMeshProUGUI>();

        highScore.text = "Record\n" + (PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore"):0);

        countZone = GameObject.FindWithTag("Manager").GetComponent<GeneratingZones>().countZone;

        distanceBetweenZones = GameObject.FindWithTag("Manager").GetComponent<GeneratingZones>().distance;

    }
    private void Start()
    {
        scoreText.text = string.Format("{0}/{1}", count, countZone);
        StartCoroutine(justMoving());
    }
    IEnumerator justMoving()
    {
        while (transform.position.z < 0)
        {
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(checkingCountZones());
    }
    IEnumerator checkingCountZones() {
        float distance = 0 - distanceBetweenZones;
        while (count!=countZone)
        {
            if (transform.position.z >= (distance + distanceBetweenZones))
            {
                distance = transform.position.z;
                count += 1;
            }
            
            scoreText.text = string.Format("{0}/{1}", count, countZone);
            yield return new WaitForFixedUpdate();
        }
    }
    private void Update()
    {
        dirRight = Input.GetAxis("Horizontal");

    }
    private void FixedUpdate()
    {
       

        float right = dirRight * speedRight;
     
        Vector3 translation = new Vector3(right, 0, speedForward) * Time.fixedDeltaTime;
        transform.Translate(translation);
        if (Math.Abs(transform.position.x) >= groundScale)
            transform.position = new Vector3(transform.position.x - translation.x , transform.position.y , transform.position.z);
    }
    
    
}

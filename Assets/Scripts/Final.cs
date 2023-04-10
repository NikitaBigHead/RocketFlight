using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{
    private RocketTrigger rocketTrigger;
    private void Awake()
    {
        rocketTrigger = GameObject.FindWithTag("Player").GetComponent<RocketTrigger>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy( other.gameObject.GetComponent<Move>());

            other.gameObject.AddComponent<EndRocket>();

            if (PlayerPrefs.GetInt("HighScore") < rocketTrigger.score)
                PlayerPrefs.SetInt("HighScore", rocketTrigger.score);

        }

        
    }
}

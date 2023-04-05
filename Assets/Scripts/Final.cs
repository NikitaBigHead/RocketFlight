using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy( other.gameObject.GetComponent<Move>());
            other.gameObject.AddComponent<EndRocket>();
        }

    }
}

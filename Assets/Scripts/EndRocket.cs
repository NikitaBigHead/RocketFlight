using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EndRocket : MonoBehaviour
{
    public float speedForward = 20f;
    public float speedRight = 40;

    public float speedTransfromCamera = 0.5f;

    public float endDist = 40f;

    private float right;
    private float symbol;

    private Vector3 camRot = new Vector3(50f,0f,0f);
    private Vector3 camPos = new Vector3(0,6.4f,-4.7f);

    private RocketTrigger rocketTrigger;

    private float finalBeginZ;
    private float distBetweenPoints = 4f;

    private void Awake()
    {
        rocketTrigger = GetComponent<RocketTrigger>();
        finalBeginZ = GameObject.FindWithTag("Finish").transform.position.z;
    }
    private void Start()
    {
        //Camera.main.transform.localPosition = camPos;
        //Camera.main.transform.localRotation = Quaternion.Euler(camRot);

        StartCoroutine(transformCamera());
        StartCoroutine(moveRocket());

        symbol = transform.position.x / Mathf.Abs(transform.position.x);
        symbol = -symbol;

        right = symbol * speedRight * Time.fixedDeltaTime;
    }
    IEnumerator transformCamera()
    {

        while (Camera.main.transform.localPosition - camPos!= Vector3.zero &&
            Camera.main.transform.localRotation.ToEuler() - camRot != Vector3.zero)
        {
            Camera.main.transform.localPosition 
                = Vector3.Lerp(Camera.main.transform.localPosition, camPos, Time.fixedDeltaTime * speedTransfromCamera );
            Camera.main.transform.localRotation  
                = Quaternion.Slerp(Camera.main.transform.localRotation, Quaternion.Euler( camRot), Time.fixedDeltaTime * speedTransfromCamera);
           yield return new FixedUpdate();
        }

    }

    IEnumerator moveRocket()
    {
        float currentPosZ = finalBeginZ;
        rocketTrigger.removeCondom();

        Debug.Log(rocketTrigger.condomsLen);

        while (rocketTrigger.condomsLen != 0)
        {
            if(Mathf.Abs(transform.position.x) < 0.2f)
                right = 0f;

            Vector3 translation = new Vector3(right, 0, speedForward) * Time.fixedDeltaTime;
            transform.Translate(translation);

            if (transform.position.z >= currentPosZ + distBetweenPoints)
            {
                currentPosZ = transform.position.z;
                rocketTrigger.removeCondom();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EggCell")
        {
            StopCoroutine(moveRocket());
        }
    }
}

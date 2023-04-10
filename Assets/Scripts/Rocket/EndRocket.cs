using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EndRocket : MonoBehaviour
{
    private AudioSource audio;
    private AudioClip winMenuClip;
    private AudioClip resumeMenuClip;

    public float speedForward = 20f;
    public float speedRight = 40;

    public float speedTransfromCamera = 0.5f;

    public float endDist = 40f;

    private float right;
    private float symbol;

    private Vector3 camRot = new Vector3(54.195f, 0f,0f);
    private Vector3 camPos = new Vector3(0, 7.65f, -6.96f);

    private RocketTrigger rocketTrigger;

    private float finalBeginZ;
    private float distBetweenPoints = 3.989f;

    private Coroutine moving;

    private GameObject winMenu;
    private TextMeshProUGUI score;
    private TextMeshProUGUI winner;
    private void Awake()
    {
        rocketTrigger = GetComponent<RocketTrigger>();
        finalBeginZ = GameObject.FindWithTag("Finish").transform.position.z;

        winMenu = GameObject.FindWithTag("UI").transform.Find("WinMenu").gameObject;
        score = GameObject.FindWithTag("UI").transform.Find("WinMenu").GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        winner = GameObject.FindWithTag("UI").transform.Find("WinMenu").GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        GameObject manager = GameObject.FindWithTag("Manager");
        audio = manager.GetComponent<HolderMusic>().audio;
        winMenuClip = manager.GetComponent<HolderMusic>().winMenuClip;
        resumeMenuClip = manager.GetComponent<HolderMusic>().resumeMenuClip;
    }
    private void Start()
    {
        symbol = transform.position.x / Mathf.Abs(transform.position.x);
        symbol = -symbol;

        right = symbol * speedRight * Time.fixedDeltaTime;

        StartCoroutine(transformCamera());
        moving = StartCoroutine(moveRocket());

    }
    IEnumerator transformCamera()
    {

        //while (Camera.main.transform.localPosition - camPos!= Vector3.zero &&
        //    Camera.main.transform.localRotation.ToEuler() - camRot != Vector3.zero)
        while(Vector3.Dot(Camera.main.transform.localPosition.normalized , camPos.normalized) < (1 - 0.00001f) &&
            (Camera.main.transform.localRotation.ToEuler() - camRot != Vector3.zero ))
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
        if (rocketTrigger.condomsLen == 0) {

            winner.text = "";
            setMenu();

             yield  break;
        }

        rocketTrigger.removeCondomInFinal();
        

        while (rocketTrigger.condomsLen != 0)
        {
            if(Mathf.Abs(transform.position.x) < 0.2f)
                right = 0f;

            Vector3 translation = new Vector3(right, 0, speedForward) * Time.fixedDeltaTime;
            transform.Translate(translation);

            if (transform.position.z >= (currentPosZ + distBetweenPoints))
            {
                //currentPosZ = transform.position.z;
                currentPosZ = currentPosZ + distBetweenPoints;
                rocketTrigger.removeCondomInFinal();
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);

        winner.text = "";
        setMenu();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EggCell")
        {
            setMenu();
            StopCoroutine(moving);
        }
    }

    private void setMenu()
    {
        score.text = "Score: " + rocketTrigger.score;
        winMenu.SetActive(true);

        if (winner.text.Length != 0)
        {
            audio.PlayOneShot(winMenuClip);
        }
        else
        {
            audio.PlayOneShot(resumeMenuClip);
        }
    }
}

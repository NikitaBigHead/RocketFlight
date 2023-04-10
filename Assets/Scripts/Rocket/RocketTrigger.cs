using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class RocketTrigger : MonoBehaviour
{
    [Header("Параметры изменения темпа игры")]
    public float initialSpeed = 12f;
    public float finalSpeed = 20f;
    public int lenBuffer = 10;

    [Header("Параметры для настройки отображения зон")]
    public Material red;
    public Material green;
    public float runningTime = 1f;

    [Header("Звуки")]
    public List<AudioClip> positive;
    public List<AudioClip> negative;
    public AudioSource audio;

    [Space]
    private List<bool> listAddedCond = new List<bool>();

    public int score = 0;

    private Move playerMove;

    public GameObject condomPrefab;

    public List<Material> materials = new List<Material>();

    public float DistanceCondoms = 0.1f;
    private Vector3 beginVolume;

    private int numMaterial;
    private int aMaterial = 170;

    private Stack<GameObject> condoms = new Stack<GameObject>();

    private GeneratingZones generatingZones;

    public int condomsLen
    {
        get
        {
            return condoms.Count;
        }
    }

    private void Awake()
    {
        beginVolume = condomPrefab.transform.localScale;
        playerMove = GetComponent<Move>();
        generatingZones = GameObject.FindWithTag("Manager").GetComponent<GeneratingZones>();
    }
    public void addCondom()
    {
        GameObject condom = Instantiate(condomPrefab, condomPrefab.transform.position, Quaternion.identity);
        condom.transform.SetParent(condomPrefab.transform.parent);

        beginVolume = new Vector3(beginVolume.x + DistanceCondoms, beginVolume.y + DistanceCondoms, beginVolume.z);
        condom.transform.localScale = beginVolume;

        StartCoroutine(moveCamera(1));

        Renderer renderer = condom.GetComponent<Renderer>();
        renderer.material = materials[numMaterial];
        renderer.enabled = true;

        numMaterial++;


        if(condoms.Count!=0)
            channgeColor(255);
        
        condoms.Push(condom);
        if (numMaterial == materials.Count || numMaterial < 0)
               numMaterial = 0;
    }
    public void removeCondom()
    {
        if (condoms.Count > 0)
        {
            beginVolume = new Vector3(beginVolume.x - DistanceCondoms, beginVolume.y - DistanceCondoms, beginVolume.z);

            StartCoroutine(moveCamera(-1));

            Destroy(condoms.Pop());
            
            if(condoms.Count != 0)
                channgeColor(170);
            
            numMaterial--;

        }
        if (numMaterial == materials.Count || numMaterial < 0)
            numMaterial = 0;
    }
    public void removeCondomInFinal()
    {

            beginVolume = new Vector3(beginVolume.x - DistanceCondoms, beginVolume.y - DistanceCondoms, beginVolume.z);

            Destroy(condoms.Pop());

            if(condoms.Count != 0)
                channgeColor(170);
            
            numMaterial--;

        
        if (numMaterial == materials.Count || numMaterial < 0)
            numMaterial = 0;
    }
    IEnumerator changeMaterial(Renderer renderer, Material newColor, float runningTime)
    {
        Material oldMaterial = renderer.material;
        renderer.material = newColor;

        yield return new WaitForSeconds(runningTime);

        renderer.material = oldMaterial;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zone")
        {
            ZoneData data = other.gameObject.GetComponent<ZoneData>();
            Renderer renderer = other.gameObject.GetComponentInChildren<Renderer>();

            score += data.score;
            if (score < 0)
            {
                score = 0;
            }


            if (data.isTrue)
            {
                StartCoroutine(changeMaterial(renderer, green, runningTime));

                audio.PlayOneShot(positive[Random.Range(0, positive.Count)]);

                for (int i = 0; i < data.score; i++)
                {
                    addCondom();
                    addTolistCondoms(data.isTrue);
                }
            }
            else
            {
                StartCoroutine(changeMaterial(renderer, red, runningTime));

                audio.PlayOneShot(negative[Random.Range(0, negative.Count)]);

                for (int i = 0; i < -data.score; i++)
                {
                    removeCondom();
                    addTolistCondoms(data.isTrue);
                }
            }

        }
        if (other.tag == "AreaZone" && (playerMove.count < (generatingZones.countZone - generatingZones.dynamicZoneCount)))
        {
            generatingZones.generateZone();
            StartCoroutine(destroyZone(other.gameObject.transform.parent.gameObject, 3f));
        }
    }

    IEnumerator destroyZone(GameObject zone, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(zone);

        try
        {
            Destroy(zone.transform.parent.gameObject);
        }
        catch(Exception ex){}
    }

    private void channgeColor(int a) {
        Material material = condoms.Peek().GetComponent<Renderer>().material;
        material.color = new Color(material.color.r, material.color.g, material.color.b, a);
        condoms.Peek().GetComponent<Renderer>().material = material;
    }
    private void addTolistCondoms(bool isTrue)
    {
        if (listAddedCond.Count != lenBuffer)
        {
            listAddedCond.Add(isTrue);
            return;
        }

        int countTrue = getCountEl(true);
        float percenTrue = countTrue / listAddedCond.Count;

        if (countTrue >=  0.7f && countTrue<=0.85)
        {
            changeSpeed(2f);
        }
        else if (countTrue >= -0.85)
        {
            changeSpeed(3f);
        }
        else if ( countTrue<=0.6 && countTrue > 0.5f){
            changeSpeed(-1f);
        }
        else if (countTrue<=0.5 && countTrue >= 0.35)
        {
            changeSpeed(-2f);
        }

        listAddedCond.Clear();

    }
    private int getCountEl(bool el)
    {
        int count = 0; 
        for(int i = 0;i< listAddedCond.Count;i++)
        {
            if (listAddedCond[i] == el)
            {
                count++;
            }
        }
        return count;
    }
    private void changeSpeed(float speed)
    {
        float playerSpeed = playerMove.speedForward;
        if((playerSpeed + speed) > finalSpeed)
        {
            playerMove.speedForward = finalSpeed;
            return;
        }
        else if ((playerSpeed+speed) < initialSpeed)
        {
            playerMove.speedForward = initialSpeed;
            return;
        }
        playerMove.speedForward = playerSpeed + speed;
    }

    IEnumerator moveCamera(float smooth)
    {
        Vector3 camPos = new Vector3(Camera.main.transform.localPosition.x,
            Camera.main.transform.localPosition.y + DistanceCondoms * smooth, Camera.main.transform.localPosition.z);
        //while (Camera.main.transform.localPosition - camPos != Vector3.zero)
        while (Vector3.Dot(Camera.main.transform.localPosition.normalized, camPos.normalized) < (1 - 0.00001f))
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition,camPos,0.1f);
            yield return new WaitForFixedUpdate();
        }
    }
}

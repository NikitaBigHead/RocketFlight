using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Stack<GameObject> condoms = new Stack<GameObject>();

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
    }
    public void addCondom()
    {
        GameObject condom = Instantiate(condomPrefab, condomPrefab.transform.position, Quaternion.identity);
        condom.transform.SetParent(condomPrefab.transform.parent);

        beginVolume = new Vector3(beginVolume.x + DistanceCondoms, beginVolume.y + DistanceCondoms, beginVolume.z);
        condom.transform.localScale = beginVolume;

        Camera.main.transform.localPosition = 
            new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y + DistanceCondoms * 0.3f, Camera.main.transform.localPosition.z);

        Renderer renderer = condom.GetComponent<Renderer>();
        renderer.material = materials[numMaterial];
        renderer.enabled = true;

        numMaterial++;

        condoms.Push(condom);
        if (numMaterial == materials.Count || numMaterial < 0)
               numMaterial = 0;
    }
    public void removeCondom()
    {
        if (condoms.Count > 0)
        {
            beginVolume = new Vector3(beginVolume.x - DistanceCondoms, beginVolume.y - DistanceCondoms, beginVolume.z);

            Camera.main.transform.localPosition =
           new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y - DistanceCondoms * 0.3f, Camera.main.transform.localPosition.z);

            Destroy(condoms.Pop());
            numMaterial--;

        }
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
            if (score <0)
            {
                score = 0;
            }
            

            if (data.isTrue)
            {
                StartCoroutine(changeMaterial(renderer,green,runningTime));

                audio.PlayOneShot(positive[Random.Range(0,positive.Count)]);

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
            //if (numMaterial == materials.Count || numMaterial < 0) 
            //    numMaterial = 0;
        }
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
}

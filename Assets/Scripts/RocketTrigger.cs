using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RocketTrigger : MonoBehaviour
{
    public int score = 0;

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

    }
    public void addCondom()
    {
        GameObject condom = Instantiate(condomPrefab, condomPrefab.transform.position, Quaternion.identity);
        condom.transform.SetParent(condomPrefab.transform.parent);

        beginVolume = new Vector3(beginVolume.x + DistanceCondoms, beginVolume.y + DistanceCondoms, beginVolume.z);
        condom.transform.localScale = beginVolume;

        Renderer renderer = condom.GetComponent<Renderer>();
        renderer.material = materials[numMaterial];
        renderer.enabled = true;

        numMaterial++;

        condoms.Push(condom);
    }
    public void removeCondom()
    {
        if (condoms.Count > 0)
        {
            Destroy(condoms.Pop());
            numMaterial--;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zone")
        {
            ZoneData data = other.gameObject.GetComponent<ZoneData>();
            score += data.score;
            if (score <0)
            {
                score = 0;
            }

            if (data.isTrue)
            {
                addCondom();
               
            }
            else
            {
                removeCondom();
               
            }
            if (numMaterial == materials.Count || numMaterial < 0) 
                numMaterial = 0;
        }
    }
}

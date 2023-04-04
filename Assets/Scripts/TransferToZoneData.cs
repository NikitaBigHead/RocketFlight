using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferToZoneData : MonoBehaviour
{
    public string equation;
    public bool isTrue;
    public int score;

    public GameObject trueObject;
    public GameObject falseObject;

    private ZoneData zoneTrue;
    private ZoneData zoneFalse;

    private void Awake()
    {
        zoneTrue = trueObject.GetComponent<ZoneData>();
        zoneFalse= falseObject.GetComponent<ZoneData>();
    }
    public void launch(string equation, bool isTrue, int score)
    {
        this.equation = equation;
        this.isTrue = isTrue;
        this.score = score;

        zoneTrue.launchDoubleZone(equation, isTrue, score);
        zoneFalse.launchDoubleZone(equation, !isTrue, score);
        
    }
}

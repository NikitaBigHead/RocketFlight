using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using File = System.IO.File;
using System.Data;
using System;
using Random = UnityEngine.Random;

public class GeneratingZones : MonoBehaviour
{
    [Space]
    public float distance = 7f;
    public int countZone = 50;

    [Header("Проценты генерации зон и их вероятность правильного ответа")]
    [Range(0,100)]   public float percentOfDoubleZone = 50f;
    [Range(0, 100)]  public float percentOfDoubleZoneCorrect = 50f;
    [Range(0, 100)]  public float percentOfZoneCorrect = 50f;

    [Header("Максимальное отклонение уравнения")]
    public int MaxOffsetUncorrectEquation = 3;

    [Header("Префабы зон")]
    public GameObject prefabZone;
    public GameObject prefabDoubleZone;

    [Header("Базовые кординаты расположения зон")]
    public float ZoneCordX = 5f;
    public float ZoneCordY = 2.5f;

    [Header("Файлы с уравнениями")]
    public TextAsset hardEquationsText;
    public TextAsset simpleEquationsText;

    [Header("Очки")]
    public int scoreForHard = 1;
    public int scoreForSimple = 1;

    private List<string> hardEquations = new List<string>();
    private List<string> simpleEquations = new List<string>();

    private string[,] hardEquationsWithAnswers;
    private string[,] SimpleEquationsWithAnswers;


    private void Awake()
    {

        //string parentDir = Directory.GetCurrentDirectory();
        //string dir = parentDir.ToString() + "/Assets/Equations/HardEquations.txt";
        //equations = File.ReadAllLines(dir);

        getList(hardEquationsText,ref hardEquations);
        getList(simpleEquationsText,ref simpleEquations);

        hardEquationsWithAnswers = new string[hardEquations.Count,2];
        SimpleEquationsWithAnswers = new string[simpleEquations.Count,2];

        setDoubleArray(hardEquations,ref hardEquationsWithAnswers);
        setDoubleArray(simpleEquations, ref SimpleEquationsWithAnswers);

        Vector3 newPos = new Vector3(0,0,0);

        for (int i = 0; i < countZone; i++)
        {
            bool isDoubleZone = random(percentOfDoubleZone);
            

            if(isDoubleZone)
            {
                TransferToZoneData zone = Instantiate(prefabDoubleZone, newPos,Quaternion.identity).GetComponent<TransferToZoneData>();

                bool isTrue = random(percentOfDoubleZoneCorrect);

                int index = Random.Range(0, simpleEquations.Count);

                string result = SimpleEquationsWithAnswers[index, 1];
                if (!isTrue) 
                    result = (Convert.ToInt32(result) + Random.Range(1, MaxOffsetUncorrectEquation)).ToString() ;
                
                string equation = SimpleEquationsWithAnswers[index, 0]+ " = " + result;

                zone.launch(equation,isTrue,scoreForSimple);
            }
            else
            {
                bool isRight = random(50f);
                int oper = isRight? 1 : -1;
                ZoneData zone = Instantiate(prefabZone, new Vector3(oper * ZoneCordX,ZoneCordY,newPos.z), Quaternion.identity)
                    .GetComponent<ZoneData>();

                bool isTrue = random(percentOfZoneCorrect);

                int index = Random.Range(0, hardEquations.Count);
                string result = hardEquationsWithAnswers[index, 1];
                if (!isTrue)
                    result = (Convert.ToInt32(result) + Random.Range(1, MaxOffsetUncorrectEquation)).ToString();

                string equation = hardEquationsWithAnswers[index, 0]+ " = " + result;

                zone.launch(equation, isTrue, scoreForHard);

            }
            
            newPos.z += distance;
        }

    }
    private void getList(TextAsset EquationsText,ref List<string> equations) {
        string str = EquationsText.ToString();
        string raw = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].ToString() == "\n")
            {
                equations.Add(raw);
                raw = "";
                continue;
            }
            raw += str[i].ToString();
        }
    }
    private void setDoubleArray(List<string> list,ref string [,] array)
    {
        for(int i = 0;i<list.Count;i++)
        {
            string str = list[i];
            str = str.Substring(0, list[i].Length -1);

            array[i,0] = str;
            array[i, 1] = new DataTable().Compute(list[i],"").ToString();        
        }
    }
    private bool random(float precent)
    {
        return Random.Range(precent - 100f, precent) >0 ? true : false; 
    }
}



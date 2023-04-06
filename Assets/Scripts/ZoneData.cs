using System;
using TMPro;
using UnityEngine;

public class ZoneData:MonoBehaviour
{
    public TextMeshProUGUI textEquation;
    public TextMeshProUGUI textRight;
    public TextMeshProUGUI textWrong;
    public TextMeshProUGUI scores;

    public string equation;
    public bool isTrue;
    public int score;

    public void launch(string equation, bool isTrue, int score) {
        this.equation = equation;
        this.isTrue = isTrue;

        textEquation.text = equation;
        textRight.text = string.Format("+{0}",score) ;
        textWrong.text = string.Format("-{0}", score);   
        
        this.score = isTrue?score:-score;
    }

    public void launchDoubleZone(string equation, bool isTrue, int score)
    {
        this.equation = equation;
        this.isTrue = isTrue;

        textEquation.text = equation;

        this.score = isTrue ? score : -score;

        scores.text = Mathf.Abs(score).ToString();
    }

}
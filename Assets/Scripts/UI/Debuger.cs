using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Debuger : MonoBehaviour
{
    public GameObject debugMenu;
    public TextMeshProUGUI fps;

    private GameObject debug;

    private void Awake()
    {
        debug = GameObject.FindWithTag("Debug");
#if DEBUG
        debugMenu.SetActive(true);
#else
     debugMenu.SetActive(false);
#endif
    }
    private void Update()
    {
        fps.text = "FPS: " + Convert.ToInt32( (Time.frameCount / Time.time)).ToString() ;
    }
    public void toogleDebugMenu()
    {
        debug.SetActive(!debug.active);
    }
}

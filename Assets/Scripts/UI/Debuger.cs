using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Debuger : MonoBehaviour
{
    public GameObject debugMenu;
    GameObject debug;
    private void Awake()
    {
        debug = GameObject.FindWithTag("Debug");
#if DEBUG
        debugMenu.SetActive(true);
#else
     debugMenu.SetActive(false);
#endif
    }
    public void toogleDebugMenu()
    {
        debug.SetActive(!debug.active);
    }
}

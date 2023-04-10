using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour
{
    private Move playerMove;
    private RaycastHit hit;

    private Dictionary<string, Action> nameHandlers;
    private void Awake()
    {
        /*nameHandlers  = new Dictionary<string, Action> {
                    {"Right", right },
                    {"Left", left },
                };
        */
        playerMove = GameObject.FindWithTag("Player").GetComponent<Move>();
    }

    private void Update()
    {

        if (Input.touchCount <= 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        if(touch.phase== TouchPhase.Began)
        {
            touchHandler(touch);
        }
        if(touch.phase == TouchPhase.Moved)
        {
            touchHandler(touch);
        }
        else if (touch.phase == TouchPhase.Ended) {
            playerMove.dirRight = 0f;
        }
    }

    private void touchHandler(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z));

        if (Physics.Raycast(ray, out hit))
        {
            
            string name = hit.collider.name;
            /*
            try
            {
                var handler = nameHandlers[name];
                handler();
            }
            catch(Exception e)
            {
                return;
            }
            */
            if (name == "Right")
            {
                right();
            }
            else if (name == "Left")
            {
                left();
            }
        }
    }
    public void right()
    {
        playerMove.dirRight = 1f;
    }
    public void left()
    {
        playerMove.dirRight = -1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Volker : MonoBehaviour
{


    private Camera mainCam;
    private GameObject controller;
    private Controller c;

    private GazeAware gaze;
    private float dwellTime = 0;
    public bool go = true;
    private bool lookedAway = true;
    public bool standard_input;
    // Use this for initialization
    void Start()
    {
        mainCam = Camera.main;

        controller = GameObject.Find("Controller");
        c = controller.GetComponent<Controller>();
        gaze = this.transform.GetChild(0).GetComponent<GazeAware>();
      //  gaze = this.GetComponent<GazeAware>();

    }

    // Update is called once per frame
    void OnMouseOver()
    {
        if (standard_input)
        {

            go = false;
        }
    }

    void OnMouseExit()
    {
        if (standard_input)
        {
            go = true;
        }
    }
    void FixedUpdate()
    {
        standard_input = c.standard_input;

        float game_speed;
        if (go)
        {
            game_speed = c.game_speed;
        }
        else
        {
            game_speed = 0;
        }
        this.transform.position = new Vector3(this.transform.position.x + 0.022f * game_speed, this.transform.position.y, this.transform.position.z);
        mainCam.transform.position = new Vector3(this.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
        //c.player_position = this.transform.position.x;

        if (!standard_input)
        {
            if (gaze.HasGazeFocus)
            {
                if (dwellTime != 0)
                {
                    if (Time.time - dwellTime > 0.1)
                    {
                        dwellTime = 0;
                        lookedAway = false;
                        if (go)
                        {
                            go = false;
                        }
                        else
                        {
                            go = true;
                        }
                    }
                }
                if (dwellTime == 0)
                {
                    dwellTime = Time.time;
                }
                if (!lookedAway)
                {
                    dwellTime = 0;
                }
            }
            else
            {
                dwellTime = 0;
                lookedAway = true;
            }
        }
    }
}

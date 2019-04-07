using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Eye : MonoBehaviour {

    private GameObject cube;
    private Rigidbody rb;
    private GazeAware gaze;
    private bool physics = false;

    private GameObject floor;
    private bool floorPresent = true;

    private UserPresence up;

    private HeadPose hp;

    private float dwellTime = 0;

	// Use this for initialization
	void Start () {
        cube = GameObject.Find("Cube");
        floor = GameObject.Find("Floor");
        gaze = cube.GetComponent<GazeAware>();
        up = TobiiAPI.GetUserPresence();
    }
	
	// Update is called once per frame
	void Update () {
        up = TobiiAPI.GetUserPresence();
        if (physics == true)
        {
            if (up == UserPresence.Present)
            {
                hp = TobiiAPI.GetHeadPose();
                if (hp.Rotation.eulerAngles.y > 180)
                {
                    if (hp.Rotation.eulerAngles.y < 330)
                    {
                        rb.AddForce((360 - hp.Rotation.eulerAngles.y) * (-1) / 5, 0, 0, ForceMode.Force);
                    }
                }
                if (hp.Rotation.eulerAngles.y < 180)
                {
                    if (hp.Rotation.eulerAngles.y > 20)
                    {
                        rb.AddForce(hp.Rotation.eulerAngles.y / 5, 0, 0, ForceMode.Force);
                    }
                }
            }
        }
        if (floorPresent == true)
        {
            if (up == UserPresence.NotPresent)
            {
                floor.active = false;
                floorPresent = false;
            }
        }
        if (gaze.HasGazeFocus)
        {
            if (physics == false)
            {
                if (dwellTime == 0)
                {
                    dwellTime = Time.time;
                }
                if (Time.time - dwellTime >= 0.4)
                {
                    cube.AddComponent<Rigidbody>();
                    rb = cube.GetComponent<Rigidbody>();
                    physics = true;
                }
            }
        }
        if (!gaze.HasGazeFocus)
        {
            dwellTime = 0;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Lift : MonoBehaviour
{

    private GameObject lift;
    private Rigidbody rb;
    private GazeAware gaze;
    private bool physics = false;

    private HeadPose hp;

    private float dwellTime = 0;

    // Use this for initialization
    void Start()
    {
        lift = GameObject.Find("Lift");
        gaze = lift.GetComponent<GazeAware>();
        rb = lift.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (physics == true)
        {
            hp = TobiiAPI.GetHeadPose();
            if (hp.Rotation.eulerAngles.x > 180)
            {
                if (hp.Rotation.eulerAngles.x < 345)
                {
                    rb.AddForce(0, (360 - hp.Rotation.eulerAngles.x) / 2, 0, ForceMode.Force);
                }
            }
            if (hp.Rotation.eulerAngles.x < 180)
            {
                if (hp.Rotation.eulerAngles.x > 30)
                {
                    rb.AddForce(0, ((hp.Rotation.eulerAngles.x) * (-1)) / 10, 0, ForceMode.Force);
                }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Boat : MonoBehaviour
{

    private GameObject boat;
    private Rigidbody rb;
    private GazeAware gaze;
    public bool physics = false;
    public float input_degree = 0;
    public bool standard_input;
    private HeadPose hp;

    private float dwellTime = 0;
    public GameObject controller;
    public Controller c;

    public bool focus = false;

    // Use this for initialization
    void Start()
    {
        boat = gameObject;
        gaze = boat.GetComponent<GazeAware>();
        rb = boat.GetComponent<Rigidbody>();

        controller = GameObject.Find("Controller");
        c = controller.GetComponent<Controller>();
        standard_input = c.standard_input;

    }

    // Update is called once per frame
    //Maybe fixed update?
    void FixedUpdate()
    {
        if (focus == true)
        {
            affect_object();
        }
        if (gaze.HasGazeFocus)
        {

            if (dwellTime == 0)
            {
                dwellTime = Time.time;
            }
            if (Time.time - dwellTime >= 0.4)
            {
                activate_physics(true);
                focus = true;
            }


        }
        if (!gaze.HasGazeFocus)
        {
            dwellTime = 0;
        }
    }

    void OnMouseOver()
    {
        activate_physics(true);
        focus = true;
    }

    void OnMouseExit()
    {
        focus = false;
    }
    private void activate_physics(bool activate)
    {
        if (activate)
        {
            if (rb == null)
            {
                boat.AddComponent<Rigidbody>();
                rb = boat.GetComponent<Rigidbody>();
                physics = true;
            }
        }
        else
        {
            if (rb != null)
            {
                physics = false;
                Destroy(rb);
                rb = null;
            }

        }
    }
    private void get_degree()
    {
        if (!standard_input)
        {
            hp = TobiiAPI.GetHeadPose();
            input_degree = hp.Rotation.eulerAngles.y;
        }
        else
        {
            input_degree = Input.GetAxis("Horizontal") * (-1) * 21;
            input_degree = input_degree + 180;
        }
    }
    private void affect_object()
    {
        /* Debug.Log(input_degree);
         get_degree();
         Vector3 v = rigidbody.velocity;
         v.x = (input_degree - 180) / 100;
         rigidbody.velocity = v;
         /*
         if (input_degree > 180)
         {
             if (input_degree < 330)
             {
                 rb.AddForce((360 - input_degree) * (-1) / 5, 0, 0, ForceMode.Force);
             }
         }
         if (input_degree < 180)
         {
             if (input_degree > 20)
             {
                 rb.AddForce(input_degree / 5, 0, 0, ForceMode.Force);
             }
         }
     }*/
    }
}
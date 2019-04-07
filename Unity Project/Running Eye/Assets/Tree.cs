using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Tree : MonoBehaviour
{

    private GameObject @object;
    private Rigidbody rb;
    private GazeAware gaze;
    private bool physics = false;
    public float input_degree = 0;
    private bool standard_input;
    private HeadPose hp;
    public float speedX = 1;
    public float speedY = 0;
    public float speedZ = 0;
    private float dwellTime = 0;
    public GameObject controller;
    public Controller c;

    public bool focus = false;
    // Use this for initialization
    void Start()
    {
        controller = GameObject.Find("Controller");
        c = controller.GetComponent<Controller>();

        //Tree object with this script, the tree=this
        @object = gameObject;
        gaze = @object.GetComponent<GazeAware>();

        //   activate_physics(true);
    }

    // Update is called once per frame
    //Maybe fixed update?
    void FixedUpdate()
    {
        standard_input = c.standard_input;
        if (standard_input)
        {

            get_degree_std();
        }
        else
        {
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
            get_degree_head();
        }

        if (focus == true)
        {
            affect_object();
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
                @object.AddComponent<Rigidbody>();
                rb = @object.GetComponent<Rigidbody>();
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
    private void get_degree_std()
    {
        input_degree = Input.GetAxis("Horizontal") * (-1) * 21;
        input_degree = input_degree + 180;
    }

    private void get_degree_head()
    {
        hp = TobiiAPI.GetHeadPose();
        input_degree = hp.Rotation.eulerAngles.y;
    }
    private void affect_object()
    {

        if (input_degree > 180)
        {
            if (input_degree < 330)
            {
                rb.AddForce((360 - input_degree) * (-1) / 5 * speedX, (360 - input_degree) * (-1) / 5 * speedY, (360 - input_degree) * (-1) / 5 * speedZ, ForceMode.Force);
            }
        }
        if (input_degree < 180)
        {
            if (input_degree > 20)
            {
                rb.AddForce(input_degree / 5 * speedX, input_degree / 5 * speedY, input_degree / 5 * speedZ, ForceMode.Force);
            }
        }
    }
}

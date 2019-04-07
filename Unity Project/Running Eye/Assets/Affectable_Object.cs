using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
/*
- focus light added
- safe point fixed
- 2 obstacles added (do not view style)
- focused objects light up
- Obstacle 3 and 4 optimiced
- Game Exit implemented
- Level Exit implemented
- Tutorial Scene implemented
- Menue Song: http://freemusicarchive.org/music/Monplaisir/American_Dreams_Soundtrack/Monplaisir_-_American_Dreams_Original_Soundtrack_-_12_A_Good_Start
- Game Song: http://freemusicarchive.org/music/Monplaisir/B_R_B_R_K_R_K_R/Monplaisir_-_B_R_B_R_K_R_K_R_-_12_Weird_serious_jingle_of_death
- Water Shader Changed, Light working

    
 */
public class Affectable_Object : MonoBehaviour
{

    private GameObject @object;
    private Rigidbody rb;
    private GazeAware gaze;
    private bool physics = false;
    public float input_degree_X = 0;
    public float input_degree_Z = 0;
    private bool standard_input;
    private HeadPose hp;
    public float speedX = 1;
    public float speedY = 0;
    public float speedZ = 0;
    public int force_type = 0;
    private float dwellTime = 0;
    private float dwellNoFocus = 0;
    private GameObject controller;
    private Controller c;
    public int deactivate_time;

    private int deactivate_in = -1;
    private float nextTime = 0.0f;
  



    public bool constrain_rot_X = false;
    public bool constrain_rot_Y = false;
    public bool constrain_rot_Z = false;

    public bool x_input = true;
    public bool z_input = false;
    public bool focus = false;
    Light focus_light;
    Affectable_Object[] AOs;
    // Use this for initialization
    void Start()
    {
        controller = GameObject.Find("Controller");
        c = controller.GetComponent<Controller>();
        //todo disable mlouse if eye enabled and otherwise
        //Tree object with this script, the tree=this
        @object = gameObject;
        gaze = @object.GetComponent<GazeAware>();
     

        standard_input = c.standard_input;
        deactivate_time = c.deactivate_time;
        gameObject.AddComponent<Light>();
        focus_light = gameObject.GetComponent<Light>();
        focus_light.type = LightType.Point;
        focus_light.range=10;
        focus_light.color = Color.magenta;
        focus_light.intensity = 3;
        focus_light.enabled = false;
      AOs = Object.FindObjectsOfType<Affectable_Object>();
    }

    // Update is called once per frame
    //Maybe fixed update?
    void FixedUpdate()
    {
        check_old();
        if (focus)
        {
            focus_light.enabled = true;
        }
        else
        {
            focus_light.enabled = false;
        }

        //standard_input = false;
        if (standard_input)
        {

            get_degree_std();
            if (Time.time > nextTime)
            {
                nextTime += 1f;
                check_deactivate();
            }
        }
        else
        {
            if (gaze.HasGazeFocus)
            {
                if (focus == false)
                {
                    dwellNoFocus = 0;

                    if (dwellTime == 0)
                    {
                        dwellTime = Time.time;
                    }
                    if (Time.time - dwellTime >= 0.1)
                    {
                      
                        foreach (Affectable_Object AO in AOs)
                        {
                            AO.focus = false;
                        }

                        activate_physics(true);
                        focus = true;
                        dwellTime = 0;
                    }



                }
            }
            if (!gaze.HasGazeFocus)
            {
                dwellTime = 0;
                if (dwellNoFocus == 0)
                {
                    dwellNoFocus = Time.time;
                }
                if (Time.time - dwellNoFocus >= 5)
                {
                    focus = false;
                }
            }
            get_degree_head();
        }

        if (focus == true)
        {
            affect_object();
        }
    }
    private void check_deactivate()
    {

        if(deactivate_in>=0)
        {
            
                // execute block of code here

                if (deactivate_in == 0)
                {
                    focus = false;
                }
                else
                {
                    deactivate_in = deactivate_in - 1;
                }
            }
        
       
    }
    void OnMouseOver()
    {
        if (standard_input)
        {
            activate_physics(true);
            focus = true;
            deactivate_in = -1;
        }
    }
    private void check_old()
    {
if((c.player.transform.position.x-gameObject.transform.position.x)>20)
        {
            activate_physics(false);
        }
    }
    void OnMouseExit()
    {
        if (standard_input)
        {
            if (deactivate_time < 0)
            {
                focus = false;
            }
            else
            {
                // 
                deactivate_in = deactivate_time;
            }
           
        }
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

                if (constrain_rot_X)
                {
                    rb.constraints |= RigidbodyConstraints.FreezeRotationX;

                }
                else
                {
                    rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;

                }
                if (constrain_rot_Y)
                {
                    rb.constraints |= RigidbodyConstraints.FreezeRotationY;

                }
                else
                {
                    rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;

                }

                if (constrain_rot_Z)
                {
                    rb.constraints |= RigidbodyConstraints.FreezeRotationZ;

                }
                else
                {
                    rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;

                }
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
       
        input_degree_X = Input.GetAxis("Horizontal") * (-1) * 21;
        input_degree_X = input_degree_X + 180;

        input_degree_Z = Input.GetAxis("Vertical") * (-1) * 21;
        input_degree_Z = input_degree_Z + 180;
    }

    private void get_degree_head()
    {
        hp = TobiiAPI.GetHeadPose();
        float pose_x = hp.Rotation.eulerAngles.y;
        float pose_z = hp.Rotation.eulerAngles.x;
        float f = 0.05f;

        bool z = false;
        bool reverse = false;
        float direction = 20;
        bool do_nothing = false;

        if (pose_x >= 180)
        {
            float amount = 360 - pose_x;
            if (amount > direction)
            {
                direction = amount;
                z = false;
                reverse = false;
            }
            //input_degree_X = 1 / f + 180;
        }
        if (pose_x < 180)
        {
            float amount = pose_x;
            if (amount > direction)
            {
                direction = amount;
                z = false;
                reverse = true;
            }
            //input_degree_X = -1 / f + 180;
        }

        if (direction == 20)
        {
            if (pose_z >= 180)
            {
                float amount = 360 - pose_z;
                if (amount > 5)
                {
                    direction = amount;
                    z = true;
                    reverse = true;
                    //input_degree_Z = -1 / f + 180;
                }
            }
            if (pose_z < 180)
            {
                float amount = pose_z;
                if (amount > 5)
                {
                    direction = amount;
                    z = true;
                    reverse = false;
                    //input_degree_Z = 1 / f + 180;
                }
            }
        }

        if (direction == 20)
        {
            do_nothing = true;
        }

        // input manipulation
        if (z)
        {
            input_degree_X = 180;
            input_degree_Z = 1 / f;
            if (reverse)
            {
                input_degree_Z = input_degree_Z * (-1);
            }
            input_degree_Z = input_degree_Z + 180;
        }
        if (!z)
        {
            input_degree_Z = 180;
            input_degree_X = 1 / f;
            if (reverse)
            {
                input_degree_X = input_degree_X * (-1);
            }
            input_degree_X = input_degree_X + 180;
        }
       
        if (do_nothing)
        {
            input_degree_X = 180;
            input_degree_Z = 180;
        }
    }
    private void affect_object()
    {
        if (force_type == 0)
        {
            Vector3 v = rb.velocity;
            if (speedX != 0)
            {
                v.x = (input_degree_X - 180) / 10 * (-1) * speedX;
            }
            if (speedY != 0)
            {
                v.y = (input_degree_X - 180) / 10 * (-1) * speedY;
            }
            if (speedZ != 0)
            {
                v.z = (input_degree_Z - 180) / 10 * (-1) * speedZ;
            }



            rb.velocity = v;
        }
        //old, don't use
        if (force_type == 1)
        {

            if (input_degree_X > 180)
            {
                if (input_degree_X < 330)
                {
                    rb.AddForce((360 - input_degree_X) * (-1) / 5 * speedX, (360 - input_degree_X) * (-1) / 5 * speedY, (360 - input_degree_X) * (-1) / 5 * speedZ, ForceMode.Force);
                }
            }
            if (input_degree_X < 180)
            {
                if (input_degree_X > 20)
                {
                    rb.AddForce(input_degree_X / 5 * speedX, input_degree_X / 5 * speedY, input_degree_X / 5 * speedZ, ForceMode.Force);
                }
            }
        }
    }
}

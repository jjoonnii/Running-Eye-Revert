using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud : MonoBehaviour
{

    // Use this for initialization
    private GameObject controller;
    private Controller c;
    void Start()
    {
        controller = GameObject.Find("Controller");
        c = controller.GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {


        int hight_movement = 0;




        this.transform.position = new Vector3(this.transform.position.x + 0.022f * c.game_speed, this.transform.position.y + hight_movement, this.transform.position.z);
    }
}
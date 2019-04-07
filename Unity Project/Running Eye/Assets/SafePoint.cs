using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class SafePoint : MonoBehaviour {

    private GameObject safe_point;
    private GazeAware gaze;

    private bool active = false;

	// Use this for initialization
	void Start () {
        safe_point = GameObject.Find("SafePoint");
        gaze = safe_point.GetComponent<GazeAware>();
    }

    // Update is called once per frame
    void Update() {
        if (gaze.HasGazeFocus)
        {
            if (!active)
            {
                // Deactivates all affectable objects
                Affectable_Object[] AOs = Object.FindObjectsOfType<Affectable_Object>();
                foreach (Affectable_Object AO in AOs)
                {
                    AO.focus = false;
                }

                // Activates Volker
                Volker[] vs = Object.FindObjectsOfType<Volker>();
                foreach (Volker v in vs)
                {
                    v.go = true;
                }

                active = true;
            }
        }
        if(!gaze.HasGazeFocus)
        {
            if (active)
            {
                active = false;
            }
        }
    }
}

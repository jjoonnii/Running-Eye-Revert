using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class info : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {


        Button back = GameObject.Find("Button").GetComponent<Button>();
        back.onClick.AddListener(go_back);
    }
    public void go_back()
    {
        SceneManager.LoadScene("menue");
    }
    // Update is called once per frame
    void Update()
    {

    }
}

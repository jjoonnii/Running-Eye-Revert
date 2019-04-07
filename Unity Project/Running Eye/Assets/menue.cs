using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menue : MonoBehaviour
{
    public int level_length = 5;
    public bool standard_input = false;
    public float game_speed = 0.85f;
    public int progress = 0;
    public int target = 0;
    public int points = 0;
    public int high_points = 0;
    Slider speed;
    Slider lenght;
    Toggle sinput;

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Text lg3 = GameObject.Find("Last_Game_3").GetComponent<Text>();
        Text lg1 = GameObject.Find("Last_Game_1").GetComponent<Text>();
        Text highscore = GameObject.Find("Points").GetComponent<Text>();



        Button start = GameObject.Find("Button").GetComponent<Button>();
        Button settings = GameObject.Find("Settings").GetComponent<Button>();
        Button end = GameObject.Find("End").GetComponent<Button>();
        Button info = GameObject.Find("Info").GetComponent<Button>();






        if (PlayerPrefs.HasKey("Game Percentage"))
        {
            progress = PlayerPrefs.GetInt("Game Percentage");

        }
        else
        {

        }

        if (PlayerPrefs.HasKey("Target"))
        {
            target = PlayerPrefs.GetInt("Target");
            if (target <= 0)
            {
                lg1.text = "YOU WON";
            }
            else
            {
                lg1.text = "YOU LOST";
            }
        }
        else
        {
            lg1.text = "YOU DIDN'T PLAYED";
        }

        if (PlayerPrefs.HasKey("Points"))
        {
            points = PlayerPrefs.GetInt("Points");
        }
        if (PlayerPrefs.HasKey("HighPoints"))
        {
            high_points = PlayerPrefs.GetInt("HighPoints");
        }



        lg3.text = points + " POINTS";
        highscore.text = high_points + "";


        start.onClick.AddListener(start_game);
        settings.onClick.AddListener(start_settings);
        end.onClick.AddListener(end_game);
        info.onClick.AddListener(start_info);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void end_game()
    {
#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif

    }
    public void start_game()
    {


        SceneManager.LoadScene("one");
    }
    public void start_settings()
    {


        SceneManager.LoadScene("settings");
    }
    public void start_info()
    {


        SceneManager.LoadScene("info");
    }
}

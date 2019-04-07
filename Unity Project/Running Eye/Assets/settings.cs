using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    public int level_length = 5;
    public bool standard_input = false;
    public float game_speed = 0.85f;
    private int deactivate_time=1;
    Slider speed;
    Slider lenght;
    Slider time;
    Toggle sinput;
    Text lenght_text;
    Text speed_text;
    Text time_text;
    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        lenght_text = GameObject.Find("Settings_1_Out").GetComponent<Text>();
        speed_text = GameObject.Find("Settings_2_Out").GetComponent<Text>();
       time_text = GameObject.Find("Settings_3_Out").GetComponent<Text>();

        speed = GameObject.Find("Slider_Speed").GetComponent<Slider>();
        lenght = GameObject.Find("Slider_Lenght").GetComponent<Slider>();
        time = GameObject.Find("Slider_Time").GetComponent<Slider>();


        sinput = GameObject.Find("Input").GetComponent<Toggle>();

        Button start = GameObject.Find("Button").GetComponent<Button>();

        if (PlayerPrefs.HasKey("Speed"))
        {
            game_speed = PlayerPrefs.GetFloat("Speed");
        }
        if (PlayerPrefs.HasKey("Input"))
        {
            standard_input = PlayerPrefs.GetInt("Input") != 0;
        }
        if (PlayerPrefs.HasKey("Lenght"))
        {
            level_length = PlayerPrefs.GetInt("Lenght");
        }
        if (PlayerPrefs.HasKey("Deactivate_Time"))
        {
            deactivate_time = PlayerPrefs.GetInt("Deactivate_Time")+1;
        }


        speed.value = game_speed;
        lenght.value = level_length;
        time.value = deactivate_time;

        SetSliderValue_Time(deactivate_time);
        SetSliderValue_Speed(game_speed);
        SetSliderValue_Lenght(level_length);

        sinput.isOn = standard_input;

        start.onClick.AddListener(back);
    }
    public void SetSliderValue_Speed(float sliderValue)
    {
        speed_text.text = Math.Round(sliderValue * 1, 1).ToString();
    }
    public void SetSliderValue_Lenght(float sliderValue)
    {
        lenght_text.text = Mathf.Round(sliderValue * 1).ToString();
    }

    public void SetSliderValue_Time(float sliderValue)
    {
        time_text.text = Mathf.Round(sliderValue * 1).ToString();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void back()
    {
        game_speed = speed.value;
        level_length = (int)lenght.value;
        deactivate_time = (int)time.value;
        standard_input = sinput.isOn;

        PlayerPrefs.SetFloat("Speed", game_speed);
        PlayerPrefs.SetInt("Lenght", level_length);
        int input = 0;
        if (standard_input)
        {
            input = 1;
        }
        PlayerPrefs.SetInt("Input", input);
        PlayerPrefs.SetInt("Deactivate_Time", deactivate_time-1);
      
        SceneManager.LoadScene("menue");
    }
}

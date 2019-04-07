using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public bool standard_input = false;
    public float game_speed = 1;
    public GameObject[] prefabs;
    public int current_build_position = 0;
    public double game_percentage = 0;
    public int level_length = 5;
    public List<GameObject> instantiated_objects;
    public GameObject target;
    public GameObject start;
    public GameObject player;
    public GameObject cloud;
    public Text target_meters_display;
    public Text cloud_meters_display;
    public Text progress_meters_display;
    public Color c1;
    private Light sun;

    public int deactivate_time;

    private int progress_meters;
    private int target_meters;
    private int cloud_meters;

    private GameObject safe_point;
    //to do get set usw
    void Start()
    {
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
            deactivate_time = PlayerPrefs.GetInt("Deactivate_Time");
        }

        if (standard_input)
        {
            GameObject.Find("SafePoint").SetActive(false);
        }


        sun = GameObject.Find("Sun").GetComponent<Light>();
        target_meters_display = GameObject.Find("Target_Meters").GetComponent<Text>();
        cloud_meters_display = GameObject.Find("Cloud_Meters").GetComponent<Text>();
        progress_meters_display = GameObject.Find("Progress_Meters").GetComponent<Text>();

        System.Random r = new System.Random();



        for (int i = 0; i < 10; i++)
        {
            GameObject c1 = Instantiate(cloud, new Vector3(r.Next(-20, 20), 2, r.Next(5, 15)), Quaternion.Euler(-90f, 0f, 0f));
            //GameObject c2 = Instantiate(cloud, new Vector3(i * 5, 2, i * 3 * (-1) - 5), Quaternion.Euler(-90f, 0f, 0f));

            c1 = c1.transform.GetChild(0).gameObject;
            c1.SetActive(false);
            //  c2 = c2.transform.GetChild(0).gameObject;
            //c2.SetActive(false);
        }

        cloud = (GameObject)Instantiate(cloud, new Vector3(-10, 2, 0), Quaternion.Euler(-90f, 0f, 0f));
        player = (GameObject)Instantiate(player, new Vector3(0, 2, 0), Quaternion.identity);
        Instantiate(start, new Vector3(0, 0, 0), Quaternion.identity);
        current_build_position = current_build_position + start.GetComponent<obstacle>().obstacle_size;
        for (int i = 0; i < level_length; i++)
        {
            instantiate_part(r.Next(prefabs.Length));
        }



        Instantiate(target, new Vector3(current_build_position, 0, 0), Quaternion.identity);


        Button back = GameObject.Find("Button").GetComponent<Button>();
        back.onClick.AddListener(go_back);
    }
    public void go_back()
    {
        SceneManager.LoadScene("menue");
    }
    private void instantiate_part(int index)
    {
        Debug.Log(index);
        Vector3 pos = new Vector3(current_build_position, 0, 0);
        instantiated_objects.Add((GameObject)Instantiate(prefabs[index], pos, Quaternion.identity));
        current_build_position = current_build_position + prefabs[index].GetComponent<obstacle>().obstacle_size;
    }
    public void FixedUpdate()
    {

        //game_percentage = Math.Round(game_progress / (current_build_position / 100));
        game_percentage = Math.Round(player.transform.position.x / current_build_position * 100, MidpointRounding.AwayFromZero);
        if (game_percentage < 0)
        {
            game_percentage = 0;
        }
        progress_meters = (int)Math.Round(game_percentage / 100 * current_build_position);
        target_meters = current_build_position - progress_meters;

        cloud_meters = (int)Math.Round(player.transform.position.x - cloud.transform.position.x, MidpointRounding.AwayFromZero);

        int points = (int)(Math.Round(current_build_position * game_speed * (game_percentage / 100)) + cloud_meters);
        int hight_points = 0;


        PlayerPrefs.SetInt("Game Percentage", Convert.ToInt32(game_percentage));
        PlayerPrefs.SetInt("Target", target_meters);
        PlayerPrefs.SetInt("Points", points);

        if (PlayerPrefs.HasKey("HighPoints"))
        {
            hight_points = PlayerPrefs.GetInt("HighPoints");
        }
        if (points >= hight_points)
        {
            hight_points = points;
            PlayerPrefs.SetInt("HighPoints", hight_points);
        }

        if (target_meters <= 0)
        {
            SceneManager.LoadScene("menue");
        }
        if (cloud_meters <= 0)
        {
            SceneManager.LoadScene("menue");

        }




    }
    public void Update()
    {
        int round_percentage = Convert.ToInt32(game_percentage);
        c1 = new Color(150 - round_percentage, 30 + round_percentage, 30 + round_percentage, 0.5f);
        Color color0 = new Color32(90, 0, 10, 128);
        Color color1 = new Color32(0, 150, 20, 128);
        float color_percentage = (float)game_percentage / (float)100;
        sun.color = Color.Lerp(color0, color1, color_percentage);



        progress_meters_display.text = progress_meters + " Meters";
        target_meters_display.text = target_meters + " Meters";
        cloud_meters_display.text = cloud_meters + " Meters";

    }

}
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
    public float game_progress = 0;
    public int current_build_position = 0;
    public double game_percentage = 0;
    public int level_length = 5;
    public List<GameObject> instantiated_objects;
    public GameObject target;
    public Text target_meters;
    public Text cloud_meters;
    public Text progress_meters;
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




        target_meters = GameObject.Find("Target_Meters").GetComponent<Text>();
        cloud_meters = GameObject.Find("Cloud_Meters").GetComponent<Text>();
        progress_meters = GameObject.Find("Progress_Meters").GetComponent<Text>();

        System.Random r = new System.Random();
        for (int i = 0; i < level_length; i++)
        {
            instantiate_part(r.Next(prefabs.Length));
        }


        Vector3 pos = new Vector3(current_build_position, 0, 0);
        Instantiate(target, pos, Quaternion.identity);



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
        game_percentage = Math.Round(game_progress / current_build_position * 100, MidpointRounding.AwayFromZero);
        if (game_percentage < 0)
        {
            game_percentage = 0;
        }
        int progress = (int)Math.Round(game_percentage / 100 * current_build_position);
        int target = current_build_position - progress;
        int points = (int)(Math.Round(current_build_position * game_speed * (game_percentage / 100)));
        int hight_points = 0;
        progress_meters.text = progress + " Meters";
        target_meters.text = target + " Meters";


        PlayerPrefs.SetInt("Game Percentage", Convert.ToInt32(game_percentage));
        PlayerPrefs.SetInt("Target", target);
        PlayerPrefs.SetInt("Points", points);

        if (PlayerPrefs.HasKey("HighPoints"))
        {
            hight_points = PlayerPrefs.GetInt("HighPoints");
        }
        if (points > hight_points)
        {
            hight_points = points;
            PlayerPrefs.SetInt("HighPoints", hight_points);
        }

        if (target < 2)
        {
            SceneManager.LoadScene("menue");
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveFileCheckerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = Path.GetFullPath(".");
        string date = DateTime.Now.ToString("dd-MM-yyyy");

        /* CHECK IF FOLDERS EXIST */
        if (!Directory.Exists(path + @"/UserData"))
        {
            Directory.CreateDirectory(path + @"/UserData");
            Directory.CreateDirectory(path + @"/UserData/Yearly");
            Directory.CreateDirectory(path + @"/UserData/Daily");
            Directory.CreateDirectory(path + @"/UserData/Statistics");
        }
        if(!Directory.Exists(path + @"/UserData/Yearly"))
        {
            Directory.CreateDirectory(path + @"/UserData/Yearly");
        }
        if (!Directory.Exists(path + @"/UserData/Daily"))
        {
            Directory.CreateDirectory(path + @"/UserData/Daily");
        }
        if (!Directory.Exists(path + @"/UserData/Statistics"))
        {
            Directory.CreateDirectory(path + @"/UserData/Statistics");
        }

        /* CHECK IF DATA EXISTS */
        for (int i = 1; i < 4; i++)
        {
            if (!File.Exists(path + @"/UserData/Daily/" + date + "-level" + i + ".txt")) // check if daily data exists
            {
                PlayerPrefs.DeleteKey("DAILYhighscore" + i);
            }
            if (!File.Exists(path + @"/UserData/Yearly/" + DateTime.Now.ToString("yyyy") + "-level" + i + ".txt")) // check if yearly data exists
            {
                PlayerPrefs.DeleteKey("YEARLYhighscore" + i);
            }
        }

        if (!File.Exists(path + @"/UserData/Statistics/" + date + "-feedback" + ".txt")) // check if daily data exists
        {
            PlayerPrefs.DeleteKey("FeedbackStats");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

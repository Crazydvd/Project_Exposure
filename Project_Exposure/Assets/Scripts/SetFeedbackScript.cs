using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SetFeedbackScript : MonoBehaviour
{
    [System.Serializable]
    struct FeedbackEntry
    {
        public string Time;
        public string Name;
        public int Knowledge;
        public int Learned;
        public int Level1Points;
        public int Level2Points;
        public int Level3Points;
    }

    struct Feedback
    {
        public List<FeedbackEntry> FeedbackList;
    }

    [SerializeField] Slider _technologySlider;
    [SerializeField] Slider _knowledgeSlider;

    List<FeedbackEntry> _feedback;

    public void SetTechnologyKnowledge(){
        StatsTrackerScript.TechnologyKnowledge = (int) _technologySlider.value;
    }

    public void SetKnowledgeLearned()
    {
        StatsTrackerScript.KnowledgeLearned = (int) _knowledgeSlider.value;
    }

    static public void SetPointsLevel(int pLevel, int pAmount)
    {
        switch(pLevel){
            case 1:
                StatsTrackerScript.PointsLevel1 = pAmount;
                break;
            case 2:
                StatsTrackerScript.PointsLevel2 = pAmount;
                break;
            case 3:
                StatsTrackerScript.PointsLevel3 = pAmount;
                break;
        }
    }

    public void AddFeedBackEntry(){
        FeedbackEntry entry = new FeedbackEntry {Time = System.DateTime.UtcNow.ToString("HH:mm dd MMMM, yyyy"), Name = StatsTrackerScript.Name, Knowledge = StatsTrackerScript.TechnologyKnowledge, Learned = StatsTrackerScript.KnowledgeLearned, Level1Points = StatsTrackerScript.PointsLevel1, Level2Points = StatsTrackerScript.PointsLevel2, Level3Points = StatsTrackerScript.PointsLevel3 };
        loadFeedback(); // load it again to be sure

        _feedback.Add(entry);

        if (_feedback.Count > 500)
        {
            _feedback.RemoveRange(500, _feedback.Count - 500); // cut off at 500
        }

        saveFeedback();
    }

    public void AddFeedback(){
        StatsTrackerScript.FeedbackGiven = true;

    }

    void loadFeedback()
    {
        string json = PlayerPrefs.GetString("FeedbackStats");
        if (json.Length > 0)
        {
            Feedback feedback = JsonUtility.FromJson<Feedback>(json);
            _feedback = feedback.FeedbackList;
        }
        else
        {
            _feedback = new List<FeedbackEntry>();
        }
    }

    void saveFeedback()
    {
        Feedback highscore = new Feedback { FeedbackList = _feedback };
        string json = JsonUtility.ToJson(highscore);
        PlayerPrefs.SetString("FeedbackStats", json);
        PlayerPrefs.Save();
    }

    public void saveTextFile()
    {         
        AddFeedBackEntry();

        string path = Path.GetFullPath(".");
        string date = DateTime.Now.ToString("dd-MM-yyyy");

        TextWriter textWriter = new StreamWriter(path + @"/UserData/Statistics/" + date + "-feedback" + ".txt");
        for (int i = 0; i < _feedback.Count; i++)
        {
            FeedbackEntry entry = _feedback[i];
            textWriter.WriteLine(entry.Time
                                + " | Name: " + entry.Name
                                + " | Opinion game: " + (entry.Knowledge > -1 ? entry.Knowledge + "" : "X")
                                + " | How much learned: " + (entry.Learned > -1 ? entry.Learned + "" : "X")
                                + " | Points level 1: " + (entry.Level1Points > -1 ? entry.Level1Points + "" : "X")
                                + " | Points level 2: " + (entry.Level2Points > -1 ? entry.Level2Points + "" : "X")
                                + " | Points level 3: " + (entry.Level3Points > -1 ? entry.Level3Points + "" : "X")
                                + " | Points Total: " + ((entry.Level1Points < 0 ? 0 : entry.Level1Points) 
                                                    + (entry.Level2Points < 0 ? 0 : entry.Level2Points)
                                                    + (entry.Level3Points < 0 ? 0 : entry.Level3Points))
                                );
        }
        textWriter.Close();
    }
}

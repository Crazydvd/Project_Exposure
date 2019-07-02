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

    public void AddFeedback(){
        StatsTrackerScript.FeedbackGiven = true;
        FeedbackEntry entry = new FeedbackEntry { Knowledge = StatsTrackerScript.TechnologyKnowledge, Learned = StatsTrackerScript.KnowledgeLearned, Level1Points = StatsTrackerScript.PointsLevel1, Level2Points = StatsTrackerScript.PointsLevel2, Level3Points = StatsTrackerScript.PointsLevel3 };
        loadFeedback(); // load it again to be sure

        _feedback.Add(entry);

        if (_feedback.Count > 500)
        {
            _feedback.RemoveRange(500, _feedback.Count - 500); // cut off at 500
        }

        saveFeedback();
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
        loadFeedback(); // to be sure
        if(!StatsTrackerScript.FeedbackGiven){
            AddFeedback();
            StatsTrackerScript.FeedbackGiven = true;
        }

        string path = Path.GetFullPath(".");
        string date = DateTime.Now.ToString("dd-MM-yyyy");

        TextWriter textWriter = new StreamWriter(path + @"/UserData/Statistics/" + date + "-feedback" + ".txt");
        for (int i = 0; i < _feedback.Count; i++)
        {
            FeedbackEntry entry = _feedback[i];
            textWriter.WriteLine("Wat vindt je van technologie: " + entry.Knowledge
                                + "| Hoeveel heb je geleerd: " + entry.Learned 
                                + "| Punten level 1: " + (entry.Level1Points > -1 ? entry.Level1Points + "" : "X")
                                + "| Punten level 2: " + (entry.Level2Points > -1 ? entry.Level2Points + "" : "X")
                                + "| Punten level 3: " + (entry.Level3Points > -1 ? entry.Level3Points + "" : "X")
                                + "| Punten Totaal: " + ((entry.Level1Points < 0 ? 0 : entry.Level1Points) 
                                                    + (entry.Level2Points < 0 ? 0 : entry.Level2Points)
                                                    + (entry.Level3Points < 0 ? 0 : entry.Level3Points))
                                );
        }
        textWriter.Close();
    }
}

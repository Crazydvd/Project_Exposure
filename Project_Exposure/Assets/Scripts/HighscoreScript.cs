using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class HighscoreScript : MonoBehaviour
{
    [System.Serializable]
    struct HighscoreEntry
    {
        public string Name;
        public float Score;
    }

    struct Highscore
    {
        public List<HighscoreEntry> HighscoreList;
    }

    enum HighscoreType
    {
        DAILY,
        YEARLY
    }

    [SerializeField] bool _autoLoop = false;
    [SerializeField] GameObject _dailyTitle;
    [SerializeField] GameObject _yearlyTitle;

    [SerializeField] InputField _nameField;

    [SerializeField] GameObject _highscoreContainer;
    [SerializeField] GameObject _highscoreTemplate;

    [SerializeField] float _offset = 20f;

    List<HighscoreEntry> _leaderBoard = new List<HighscoreEntry>();
    HighscoreType _highscoreType = HighscoreType.DAILY; // default is daily
    float _level = 1;
    float _score = 0;
    bool _entryAdded = false;

    float _autoLoopTimer = 0f;
    float _autoLoopTimerMax = 5f;

    private void Start()
    {
        if (_autoLoop)
        {
            _autoLoopTimer = _autoLoopTimerMax;
        }
    }

    void OnEnable()
    {
        ReloadHighscore();
    }

    void Update()
    {
        if(_autoLoop){
            if(_autoLoopTimer > 0f){
                _autoLoopTimer -= Time.deltaTime;
            }else{
                switch (_highscoreType)
                {
                    case HighscoreType.DAILY:
                        SetHighscoreTypeYearly();
                        _yearlyTitle.SetActive(true);
                        _dailyTitle.SetActive(false);
                        break;
                    case HighscoreType.YEARLY:
                        SetHighscoreTypeDaily();
                        _yearlyTitle.SetActive(false);
                        _dailyTitle.SetActive(true);
                        break;
                }
                _autoLoopTimer = _autoLoopTimerMax;
                ReloadHighscore();
            }
        }    
    }

    public void ReloadHighscore(){
        loadHighscore();
        for (int i = _highscoreContainer.transform.childCount - 1; i > -1; i--)
        {
            Destroy(_highscoreContainer.transform.GetChild(i).gameObject);
        }

        bool currentPlayerFound = false;

        for (int i = 0; i < 10; i++)
        {

            GameObject score = Instantiate(_highscoreTemplate, _highscoreContainer.transform);
            score.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -_offset * i);

            score.transform.GetChild(0).GetComponent<Text>().text = (i + 1) + "."; // place

            if (i < _leaderBoard.Count)
            {
                score.transform.GetChild(1).GetComponent<Text>().text = "" + _leaderBoard[i].Score; // score

                Text name = score.transform.GetChild(2).GetComponent<Text>();
                if (_leaderBoard[i].Name == "Naamloos")
                {
                    switch (LanguageSettings.Language)
                    {
                        case Language.NL:
                            name.text = "Naamloos";
                            break;
                        case Language.EN:
                            name.text = "Anonymous";
                            break;
                        case Language.DE:
                            name.text = "Unbekannt";
                            break;
                    }
                }
                else
                {
                    name.text = "" + _leaderBoard[i].Name;
                }

                if ((_score == _leaderBoard[i].Score && !currentPlayerFound) && !_autoLoop)
                { // set current score as yellow
                    score.transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
                    score.transform.GetChild(1).GetComponent<Text>().color = Color.yellow;
                    score.transform.GetChild(2).GetComponent<Text>().color = Color.yellow;
                    currentPlayerFound = true;
                }
            }else{
                score.transform.GetChild(1).GetComponent<Text>().text = "-";
                score.transform.GetChild(2).GetComponent<Text>().text = "-";
            }
        }
    }

    public void AddEntry()
    {
        if(_entryAdded){
            return;
        }
        
        _entryAdded = true;
        HighscoreEntry entry = new HighscoreEntry { Name = StatsTrackerScript.Name, Score = _score };
        for (int loop = 0; loop < 2; loop++) // add it both to daily and yearly
        {
            loadHighscore(); // load it again to be sure
            bool inserted = false;

            // insert our new entry
            for (int i = 0; i < _leaderBoard.Count; i++)
            {
                if (entry.Score >= _leaderBoard[i].Score)
                {
                    _leaderBoard.Insert(i, entry);
                    inserted = true;
                    break;
                }
            }
            if (_leaderBoard.Count < 1 || !inserted)
            {
                _leaderBoard.Add(entry);
            }

            if (_leaderBoard.Count > 50)
            {
                _leaderBoard.RemoveRange(50, _leaderBoard.Count - 50); // cut off at 50
            }
            saveHighscore();
            saveTextFile();
            _highscoreType = _highscoreType == HighscoreType.DAILY ? HighscoreType.YEARLY : HighscoreType.DAILY; // toggle to the other
        }

    }

    public int CheckDailySpot(float pScore)
    {
        loadHighscore();

        for (int i = 0; i < _leaderBoard.Count; i++)
        {
            if (pScore >= _leaderBoard[i].Score)
            {
                return i + 1;
            }
        }
        return _leaderBoard.Count + 1; // last spot
    }

    public void SetName(){
        if (_nameField.text != "")
        {
            StatsTrackerScript.Name = _nameField.text;
        }else{
            StatsTrackerScript.Name = "Naamloos";
        }
    }

    public void SkipName(){
        StatsTrackerScript.Name = "Naamloos";
    }

    public void SetScore(float pScore){
        _score = pScore;
    }

    public void SetLevel(float pLevel){
        _level = pLevel;
    }

    public void SetHighscoreTypeDaily(){
        _highscoreType = HighscoreType.DAILY;
    }
    public void SetHighscoreTypeYearly()
    {
        _highscoreType = HighscoreType.YEARLY;
    }

    void loadHighscore()
    {
        string json = PlayerPrefs.GetString(_highscoreType.ToString() + "highscore" + _level);
        if (json.Length > 0)
        {
            Highscore highscore = JsonUtility.FromJson<Highscore>(json);
            _leaderBoard = highscore.HighscoreList;
        }
        else
        {
            _leaderBoard = new List<HighscoreEntry>();
        }
        _leaderBoard = _leaderBoard.OrderByDescending(x => x.Score).ToList();
    }

    void saveHighscore()
    {
        Highscore highscore = new Highscore { HighscoreList = _leaderBoard };
        string json = JsonUtility.ToJson(highscore);
        string type = _highscoreType.ToString();
        PlayerPrefs.SetString(_highscoreType.ToString() + "highscore" + _level, json);
        PlayerPrefs.Save();
    }

    void saveTextFile(){
        string path = Path.GetFullPath(".");
        string date = DateTime.Now.ToString("dd-MM-yyyy");

        if(_highscoreType == HighscoreType.DAILY){
            TextWriter textWriter = new StreamWriter(path + @"/UserData/Daily/" + date + "-level" + _level + ".txt");
            for (int i = 0; i < _leaderBoard.Count; i++)
            {
                HighscoreEntry entry = _leaderBoard[i];
                textWriter.WriteLine("#" + (i + 1) + " " + entry.Name + " " + entry.Score);
            }
            textWriter.Close();
        }
        else if(_highscoreType == HighscoreType.YEARLY)
        {
            TextWriter textWriter = new StreamWriter(path + @"/UserData/Yearly/" + DateTime.Now.ToString("yyyy") + "-level" + _level + ".txt");
            for (int i = 0; i < _leaderBoard.Count; i++)
            {
                HighscoreEntry entry = _leaderBoard[i];
                textWriter.WriteLine("#" + (i + 1) + " " + entry.Name + " " + entry.Score);
            }
            textWriter.Close();
        }
    }
}

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

    [SerializeField] GameObject _nameInputPanel;
    [SerializeField] InputField _nameField;

    [SerializeField] GameObject _highscoreContainer;
    [SerializeField] GameObject _highscoreTemplate;

    [SerializeField] float _offset = 20f;

    List<HighscoreEntry> _leaderBoard = new List<HighscoreEntry>();
    HighscoreType _highscoreType = HighscoreType.DAILY; // default is daily
    float _level = 1;
    float _score = 0;
    string _name = "Naamloos";
    bool _entryAdded = false;

    void OnEnable()
    {
        ReloadHighscore();
    }

    public void ReloadHighscore(){
        loadHighscore();
        for (int i = _highscoreContainer.transform.childCount - 1; i > -1; i--)
        {
            Destroy(_highscoreContainer.transform.GetChild(i).gameObject);
        }

        float count = _leaderBoard.Count < 10 ? _leaderBoard.Count : 10;
        for (int i = 0; i < count; i++)
        {
            GameObject score = Instantiate(_highscoreTemplate, _highscoreContainer.transform);
            score.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -_offset * i);

            score.transform.GetChild(0).GetComponent<Text>().text = "#" + (i + 1); // place
            score.transform.GetChild(1).GetComponent<Text>().text = "" + _leaderBoard[i].Score; // score
            score.transform.GetChild(2).GetComponent<Text>().text = "" + _leaderBoard[i].Name; // name
        }
    }

    public void AddEntry()
    {
        if(_entryAdded){
            return;
        }
        
        _entryAdded = true;
        HighscoreEntry entry = new HighscoreEntry { Name = _name, Score = _score };
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

    public void ShowPlayerInputPanel(){
        if(_entryAdded){
            gameObject.SetActive(true);
        }else{
            _nameInputPanel.SetActive(true);
        }
    }

    public void SetName(){
        if (_nameField.text != "")
        {
            _name = _nameField.text;
        }else{
            _name = "Naamloos";
        }
    }

    public void SkipName(){
        _name = "Naamloos";
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] GameObject _highscoreContainer;
    [SerializeField] GameObject _highscoreTemplate;

    [SerializeField] float _offset = 20f;

    List<HighscoreEntry> _leaderBoard = new List<HighscoreEntry>();
    float _level = 1;

    void OnEnable()
    {
        for (int i = _highscoreContainer.transform.childCount - 1; i > 0; i--)
        {
            Destroy(_highscoreContainer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < _leaderBoard.Count; i++)
        {
            GameObject score = Instantiate(_highscoreTemplate, _highscoreContainer.transform);
            score.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -_offset * i);

            score.transform.GetChild(0).GetComponent<Text>().text = "#" + (i + 1); // place
            score.transform.GetChild(1).GetComponent<Text>().text = "" + _leaderBoard[i].Score; // score
            score.transform.GetChild(2).GetComponent<Text>().text = "" + _leaderBoard[i].Name; // name
        }
    }

    public void AddEntry(string pName, float pScore, int pLevel)
    {
        _level = pLevel;
        HighscoreEntry entry = new HighscoreEntry { Name = pName, Score = pScore };
        loadHighscore();

        _leaderBoard = _leaderBoard.OrderByDescending(x => x.Score).ToList(); // sort list

        // insert our new entry
        for (int i = 0; i < _leaderBoard.Count; i++)
        {
            if (entry.Score >= _leaderBoard[i].Score)
            {
                _leaderBoard.Insert(i, entry);
                break;
            }
        }

        if (_leaderBoard.Count > 10)
        {
            _leaderBoard.RemoveRange(10, _leaderBoard.Count - 10); // cut off at 10
        }
        saveHighscore();
    }

    void loadHighscore()
    {
        string json = PlayerPrefs.GetString("highscore" + _level);
        if (json.Length > 0)
        {
            Highscore highscore = JsonUtility.FromJson<Highscore>(json);
            _leaderBoard = highscore.HighscoreList;
        }
        else
        {
            _leaderBoard = new List<HighscoreEntry>();
        }
    }

    void saveHighscore()
    {
        Highscore highscore = new Highscore { HighscoreList = _leaderBoard };
        string json = JsonUtility.ToJson(highscore);
        PlayerPrefs.SetString("highscore" + _level, json);
        PlayerPrefs.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighscoreScript : MonoBehaviour
{
	[System.Serializable]
	struct HighscoreEntry{
		public string name;
		public float score;
	}

	struct Highscore{
		public List<HighscoreEntry> HighscoreList;
	}

	[SerializeField] GameObject _highscoreContainer;
	[SerializeField] GameObject _highscoreTemplate;

	[SerializeField] float _offset = 20f;

	List<HighscoreEntry> _leaderBoard = new List<HighscoreEntry>();
	float _level = 1;

    void OnEnable()
    {
		for(int i = _highscoreContainer.transform.childCount - 1; i > 0; i--){
			Destroy(_highscoreContainer.transform.GetChild(i).gameObject);
		}

        for(int i = 0; i < _leaderBoard.Count; i++){
			GameObject score = Instantiate(_highscoreTemplate, _highscoreContainer.transform);
			score.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -_offset * i);

			score.transform.GetChild(0).GetComponent<Text>().text = "#" + (i + 1); // place
			score.transform.GetChild(1).GetComponent<Text>().text = "" + _leaderBoard[i].score; // score
			score.transform.GetChild(2).GetComponent<Text>().text = "" + _leaderBoard[i].name; // name
		}
    }

	public void AddEntry(string pName, float pScore, int pLevel){
		_level = pLevel;
		HighscoreEntry entry = new HighscoreEntry { name = pName, score = pScore };
		LoadHighscore();

		_leaderBoard = _leaderBoard.OrderByDescending(x => x.score).ToList(); // sort list

		// insert our new entry
		for (int i = 0; i < _leaderBoard.Count; i++)
		{
			if(entry.score >= _leaderBoard[i].score){
				_leaderBoard.Insert(i, entry);
				break;
			}
		}

		if (_leaderBoard.Count > 10){
			_leaderBoard.RemoveRange(10, _leaderBoard.Count - 10); // cut off at 10
		}
		SaveHighscore();
	}

	void LoadHighscore(){
		string json = PlayerPrefs.GetString("highscore" + _level);
		if (json.Length > 0)
		{
			Highscore highscore = JsonUtility.FromJson<Highscore>(json);
			_leaderBoard = highscore.HighscoreList;
		}else{
			_leaderBoard = new List<HighscoreEntry>();
		}
	}

	void SaveHighscore(){
		Highscore highscore = new Highscore { HighscoreList = _leaderBoard };
		string json = JsonUtility.ToJson(highscore);
		PlayerPrefs.SetString("highscore" + _level, json);
		PlayerPrefs.Save();
	}
}

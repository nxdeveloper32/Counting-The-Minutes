using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class VictoryScript : MonoBehaviour {
	public GameObject Victorypanel;
	public Text PlayerDeaths;
	public Text PlayerKills;
	public Text ScoreText;
	public UI UIScript;
	private int score;
	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			Time.timeScale = 0.0f;
			score = (PlayerPrefs.GetInt ("PlayerKills") * 15) - (PlayerPrefs.GetInt ("PlayerDeaths") * 5);
			PlayerDeaths.text = PlayerPrefs.GetInt ("PlayerDeaths").ToString();
			PlayerKills.text = PlayerPrefs.GetInt ("PlayerKills").ToString();
			ScoreText.text = score.ToString();
			UIScript.SaveGame ();
			Victorypanel.SetActive (true);
		}
	}
}

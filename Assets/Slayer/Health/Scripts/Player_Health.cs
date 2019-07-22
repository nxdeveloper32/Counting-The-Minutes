using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
public class Player_Health : MonoBehaviour {
	[SerializeField]
	private BarScript health;
	private string connectionString;
	public Text HealthText;
	public GameObject Damage;
	public GameObject DeadPanel;
	void Start () {
		connectionString = "URI=file:" + Application.dataPath + "/Plugins/CountingTheMinutes.sqlite";
	}
	public void Hit(float HitHP){
			health.CurrentHealth -= HitHP;
			Damage.SetActive (true);
			StartCoroutine (DisableDamage ());
			health.CurrentHealth = Mathf.Clamp (health.CurrentHealth, 0, health.MaxHealth);
			if (health.CurrentHealth == 0) {
				PlayerPrefs.SetInt ("PlayerDeaths", PlayerPrefs.GetInt ("PlayerDeaths") + 1);
				SaveDeaths(PlayerPrefs.GetInt ("PlayerDeaths"),PlayerPrefs.GetString("PlayerName"));
				Time.timeScale = 0.0f;
				DeadPanel.SetActive(true);
			}
			HealthText.text = health.CurrentHealth.ToString () + " / " + health.MaxHealth.ToString ();
	}
	IEnumerator DisableDamage (){
		yield return new WaitForSeconds (1);
		Damage.SetActive (false);
	}
	void Update(){
		if(Input.GetKeyDown("h") && PlayerPrefs.GetInt("PlayerHealthpack")!=0){
			health.CurrentHealth = 100;
			PlayerPrefs.SetInt ("PlayerHealthpack", PlayerPrefs.GetInt ("PlayerHealthpack") - 1);
			HealthText.text = health.CurrentHealth.ToString () + " / " + health.MaxHealth.ToString ();
		}
	}
	private void SaveDeaths(int deaths,string name){
		var score = (PlayerPrefs.GetInt("PlayerKills") * 15) -  (PlayerPrefs.GetInt("PlayerDeaths") * 5);
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = String.Format("UPDATE Users SET deaths = \"{0}\",Score = \"{1}\" WHERE name  = \"{2}\"",deaths,score,name);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}
}

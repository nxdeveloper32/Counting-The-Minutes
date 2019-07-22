using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System;
public class UI : MonoBehaviour {
	public GameObject PausePanel;
	public GameObject ControlPanel;
	public GameObject FreeCam;
	public GameObject PlayerHealthBar;
	public GameObject HealthPack;
	public GameObject AmmoPack;
	public GameObject PickUpNotification;
	public bool isPaused;
	public Transform Player;

	public GameObject IntroPanel;
	public GameObject HintPanel;

	private string connectionString;
	private bool pausePossible;

	public GameObject LowBulletsAmmoUI;
	public GameObject LowHealthPacksUI;

	void Start () {
		isPaused = false;
		pausePossible = true;
		connectionString = "URI=file:" + Application.dataPath + "/Plugins/CountingTheMinutes.sqlite";
		if (PlayerPrefs.GetInt ("NewGame") == 1 ) {
			SetPlayerPrefsForUser (PlayerPrefs.GetString("PlayerName"));
		} else {
			Player.position = new Vector3 (PlayerPrefs.GetFloat("PlayerPosx"),PlayerPrefs.GetFloat("PlayerPosy"),PlayerPrefs.GetFloat("PlayerPosz"));
			Player.eulerAngles = new Vector3(PlayerPrefs.GetFloat("PlayerRotx"),PlayerPrefs.GetFloat("PlayerRoty"),PlayerPrefs.GetFloat("PlayerRotz"));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			if(pausePossible){
				PauseGame(true);
				return;
			}
			IntroPanel.SetActive (false);
			HintPanel.SetActive (true);
			pausePossible = true;
			IntroPanel.SetActive (false);
			PlayerHealthBar.SetActive (true);
			HealthPack.SetActive (true);
			AmmoPack.SetActive (true);
			PickUpNotification.SetActive (false);
			Cursor.visible = false;
			Time.timeScale = 1.0f;
		}
		if(Input.GetKeyDown("c") && Time.timeScale == 1.0f){
			HintsButton ();
		}
		if(Input.GetKeyDown("`")){
			PlayerPrefs.SetInt ("PlayerBullets", (PlayerPrefs.GetInt ("PlayerBullets") + 999));
			PlayerPrefs.SetInt ("PlayerHealthpack", (PlayerPrefs.GetInt ("PlayerHealthpack") + 999));
		}
		if (PlayerPrefs.GetInt ("PlayerBullets") == 0) {
			LowBulletsAmmoUI.SetActive (true);
		} else {
			LowBulletsAmmoUI.SetActive (false);
		}
		if (PlayerPrefs.GetInt ("PlayerHealthpack") == 0) {
			LowHealthPacksUI.SetActive (true);
		} else {
			LowHealthPacksUI.SetActive (false);
		}
	}
	void PauseGame(bool state){
		if (state) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
		PausePanel.SetActive(state);
		ControlPanel.SetActive (false);
		FreeCam.SetActive (false);
		PlayerHealthBar.SetActive (!state);
		HealthPack.SetActive (!state);
		AmmoPack.SetActive (!state);
		PickUpNotification.SetActive (false);
		HintPanel.SetActive (!state);
		Cursor.visible = state;
	}
	public void ContinueGame(){
		PauseGame(false);
	}
	public void QuitGame(){
		Time.timeScale = 1.0f;
		PlayerPrefs.DeleteAll ();
		Application.LoadLevel ("Menu");
	}
	public void ControlBtn(){
		ControlPanel.SetActive (true);
		FreeCam.SetActive (true);
		PausePanel.SetActive(false);
		PlayerHealthBar.SetActive (false);
		HealthPack.SetActive (false);
		AmmoPack.SetActive (false);
		PickUpNotification.SetActive (false);
	}
	public void SaveBtn(){
		var score = (PlayerPrefs.GetInt("PlayerKills") * 15) -  (PlayerPrefs.GetInt("PlayerDeaths") * 5);
		UpdateUsersdb (PlayerPrefs.GetString("PlayerName"),PlayerPrefs.GetInt("PlayerKills"),PlayerPrefs.GetInt("PlayerDeaths"),PlayerPrefs.GetInt("PlayerHealth"),PlayerPrefs.GetInt("PlayerLevel"),Player.position.x,Player.position.y,Player.position.z,Player.rotation.x,Player.rotation.y,Player.rotation.z,PlayerPrefs.GetInt("PlayerBullets"),PlayerPrefs.GetInt("PlayerHealthpack"),score);
		SetPlayerPrefsForUser (PlayerPrefs.GetString("PlayerName"));
		PauseGame (false);
	}
	public void SaveGame(){
		var score = (PlayerPrefs.GetInt("PlayerKills") * 15) -  (PlayerPrefs.GetInt("PlayerDeaths") * 5);
		UpdateUsersdb (PlayerPrefs.GetString("PlayerName"),PlayerPrefs.GetInt("PlayerKills"),PlayerPrefs.GetInt("PlayerDeaths"),PlayerPrefs.GetInt("PlayerHealth"),PlayerPrefs.GetInt("PlayerLevel"),52.94f,9.21f,66.4f,0,0,0,PlayerPrefs.GetInt("PlayerBullets"),PlayerPrefs.GetInt("PlayerHealthpack"),score);
	}
	public void HintsButton(){
		Time.timeScale = 0.0f;
		pausePossible = false;
		IntroPanel.SetActive (true);
		Cursor.visible = true;
		HintPanel.SetActive (false);
		PlayerHealthBar.SetActive (false);
		HealthPack.SetActive (false);
		AmmoPack.SetActive (false);
		PickUpNotification.SetActive (false);
	}
	private void SetPlayerPrefsForUser(string name){
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = String.Format("SELECT * FROM Users WHERE name = \"{0}\" ",name);
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()){
					reader.Read();
					PlayerPrefs.SetInt("PlayerKills",reader.GetInt32(2));
					PlayerPrefs.SetInt("PlayerDeaths",reader.GetInt32(3));
					PlayerPrefs.SetInt("PlayerHealth",reader.GetInt32(4));
					PlayerPrefs.SetInt("PlayerLevel",reader.GetInt32(5));
					PlayerPrefs.SetFloat("PlayerPosx",reader.GetFloat(6));
					PlayerPrefs.SetFloat("PlayerPosy",reader.GetFloat(7));
					PlayerPrefs.SetFloat("PlayerPosz",reader.GetFloat(8));
					PlayerPrefs.SetFloat("PlayerRotx",reader.GetFloat(9));
					PlayerPrefs.SetFloat("PlayerRoty",reader.GetFloat(10));
					PlayerPrefs.SetFloat("PlayerRotz",reader.GetFloat(11));
					PlayerPrefs.SetInt("PlayerBullets",reader.GetInt32(12));
					PlayerPrefs.SetInt("PlayerHealthpack",reader.GetInt32(13));
					dbConnection.Close();
					reader.Close();
				}
			}
		}
	}
	private void UpdateUsersdb (string name,int kills ,int deaths , int health,int level,float posx,float posy, float posz,float rotx,float roty,float rotz, int bullets,int healthpacks,int score){
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = String.Format("UPDATE Users SET kills = \"{0}\",deaths = \"{1}\",health = \"{2}\",level = \"{3}\",posx = \"{4}\",posy = \"{5}\",posz = \"{6}\",rotx = \"{7}\",roty = \"{8}\",rotz = \"{9}\",bullets = \"{10}\",healthpacks = \"{11}\",Score = \"{12}\" WHERE name  = \"{13}\"",kills,deaths,health,level,posx,posy,posz,rotx,roty,rotz,bullets,healthpacks,score,name);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}
}
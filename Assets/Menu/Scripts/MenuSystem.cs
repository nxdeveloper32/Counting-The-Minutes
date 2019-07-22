using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class MenuSystem : MonoBehaviour {
	public GameObject ControlPanel;
	public GameObject Title;
	public GameObject NamePanel;
	public GameObject EnemyLusth;
	public GameObject ResumeGamePanel;
	public Text PlayerName;
	public GameObject HighScorePanel;
	public GameObject ResumeGameLoadContentPrefab;
	public GameObject HighScoreGameLoadContentPrefab;
	public GameObject LoadListPanel;
	public GameObject PlayerScoreList;
	public GameObject PlayerScreen;
	public Texture LoadingDesert;
	public Texture LoadingCity;

	bool newGame = false;
	bool highScore = false;
	bool Controls = false;
	bool resumeGame = false;
	bool Exit = false;

	private string connectionString;

	void Start(){
		connectionString = "URI=file:" + Application.dataPath + "/Plugins/CountingTheMinutes.sqlite";
	}
	void Update(){
		if(Input.GetButtonDown("Cancel")){
			ControlPanel.SetActive(false);
			Title.SetActive(true);
			newGame = false;
			resumeGame = false;
			highScore = false;
			Controls = false;
			Exit = false;
			NamePanel.SetActive(false);
			EnemyLusth.SetActive(false);
			
			GameObject[] HighScoreObjects = GameObject.FindGameObjectsWithTag("PlayerScoreEntry");
			foreach (GameObject target in HighScoreObjects) {
				GameObject.Destroy(target);
			}
			HighScorePanel.SetActive(false);
			GameObject[] LoadgameObjects = GameObject.FindGameObjectsWithTag("Load");
			foreach (GameObject target in LoadgameObjects) {
				GameObject.Destroy(target);
			}
			ResumeGamePanel.SetActive(false);
		}
	}
	private void InsertUser(string name){
		int kills = 0;
		int deaths = 0;
		int health = 100;
		int level = 1;
		float posx = 52.94f;
		float posy = 9.21f;
		float posz = 66.4f;
		float rotx = 0;
		float roty = 0;
		float rotz = 0;
		float bullets = 30;
		float healthpacks = 2;
		float score = 0;
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = String.Format("INSERT INTO Users(name,kills,deaths,health,level,posx,posy,posz,rotx,roty,rotz,bullets,healthpacks,Score) VALUES (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\")",name,kills,deaths,health,level,posx,posy,posz,rotx,roty,rotz,bullets,healthpacks,score);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}
    private void CreateTable() {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("CREATE TABLE if not exists Users (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , name VARCHAR NOT NULL , kills INTEGER NOT NULL , deaths INTEGER NOT NULL , health INTEGER NOT NULL , level INTEGER NOT NULL , posx FLOAT NOT NULL , posy FLOAT NOT NULL , posz FLOAT NOT NULL , rotx FLOAT NOT NULL , roty FLOAT NOT NULL , rotz FLOAT NOT NULL , bullets INTEGER NOT NULL , healthpacks INTEGER NOT NULL , Score INTEGER NOT NULL , Date DATETIME NOT NULL  DEFAULT CURRENT_DATE, Time DATETIME NOT NULL  DEFAULT CURRENT_TIME)");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
	public void StartGame(){
        CreateTable();
		InsertUser (PlayerName.text);
		PlayerPrefs.SetString ("PlayerName",PlayerName.text);
		PlayerPrefs.SetInt ("NewGame",1);
		PlayerName.text = "";
		Application.LoadLevel ( "Start" );
	}
	void LoadHighScore(){
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = "SELECT name,kills,deaths,Score FROM Users ORDER BY Score DESC";
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()){
					while(reader.Read()){
						GameObject tmpObj = Instantiate(HighScoreGameLoadContentPrefab);
						tmpObj.GetComponent<PlayerScorePanel>().SetPlayerScoreText(reader.GetString(0),reader.GetInt32(3).ToString(),reader.GetInt32(1).ToString(),reader.GetInt32(2).ToString());
						tmpObj.transform.SetParent(PlayerScoreList.transform);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
	}
	void LoadResumeGame(){
		PlayerPrefs.SetInt ("NewGame",0);
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = "SELECT * FROM Users";
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()){
					while(reader.Read()){
						GameObject tmpObj = Instantiate(ResumeGameLoadContentPrefab);
						tmpObj.GetComponent<LoadPanelSelect>().SetResGameText(reader.GetString(1),reader.GetString(15),reader.GetString(16),reader.GetInt32(5).ToString());
						tmpObj.transform.SetParent(LoadListPanel.transform);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
	}
	void OnGUI(){
		if (!newGame) {
			if (GUI.Button (new Rect (Screen.width / 1.4f, Screen.height / 7f, Screen.width / 5, Screen.height / 10) , "New Game")){
				newGame = true;
				resumeGame = true;
				highScore = true;
				Controls = true;
				Exit = true;
				Title.SetActive(false);
				NamePanel.SetActive(true);
				EnemyLusth.SetActive(true);
			}
		}
		if (!resumeGame) {
			if (GUI.Button (new Rect (Screen.width / 1.4f, Screen.height / 3.2f, Screen.width / 5, Screen.height / 10), "Resume Game")) {
				newGame = true;
				resumeGame = true;
				highScore = true;
				Controls = true;
				Exit = true;
				EnemyLusth.SetActive(true);
				ResumeGamePanel.SetActive(true);
				LoadResumeGame();
			}
		}

		if (!highScore) {
			if (GUI.Button (new Rect (Screen.width / 1.4f, Screen.height / 2.1f, Screen.width / 5, Screen.height / 10), "High Score")) {
				Title.SetActive(false);
				newGame = true;
				resumeGame = true;
				highScore = true;
				Controls = true;
				Exit = true;
				EnemyLusth.SetActive(true);
				HighScorePanel.SetActive(true);
				LoadHighScore();
			}
		}
		if (!Controls) {
			if (GUI.Button (new Rect (Screen.width / 1.4f, Screen.height / 1.55f, Screen.width / 5, Screen.height / 10), "Controls")) {
				Title.SetActive(false);
				ControlPanel.SetActive(true);
				newGame = true;
				resumeGame = true;
				highScore = true;
				Controls = true;
				Exit = true;
				EnemyLusth.SetActive(true);
			}
		}
		if (!Exit) {
			if (GUI.Button (new Rect (Screen.width / 1.4f, Screen.height / 1.23f, Screen.width / 5, Screen.height / 10), "Exit")) {
				Application.Quit();
			}
		}
	}
}
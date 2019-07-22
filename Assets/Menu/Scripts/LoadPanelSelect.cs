using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class LoadPanelSelect : MonoBehaviour {
	public Color SelectColor;
	public Color DeselectColor;
	public Text Name;
	public Text SaveDate;
	public Text SaveTime;
	public Text SaveLevel;

	private Image LoadBGColor;
	private string connectionString;
	private GameObject FindScript;

	void Start(){
		LoadBGColor = GetComponent<Image> ();
		connectionString = "URI=file:" + Application.dataPath + "/Plugins/CountingTheMinutes.sqlite";
		FindScript = GameObject.FindGameObjectWithTag ("PlayerScreen");
	}
	public void LoadSelect(){
		LoadBGColor.color = SelectColor;
	}
	public void LoadDeselect(){
		LoadBGColor.color = DeselectColor;
	}
	public void SetResGameText(string a,string b,string c,string d){
		Name.text = a;
		SaveDate.text = b;
		SaveTime.text = c;
		SaveLevel.text = d;
	}
	public void SetPlayerPrefsForResGame(){
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()){
				string sqlQuery = String.Format("SELECT * FROM Users WHERE name = \"{0}\"",Name.text);
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()){
					reader.Read();
					PlayerPrefs.SetString ("PlayerName",reader.GetString(1));
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
		if (PlayerPrefs.GetInt ("PlayerLevel") == 1) {
			FindScript.GetComponent<MenuSystem>().PlayerScreen.GetComponent<RawImage>().texture = FindScript.GetComponent<MenuSystem>().LoadingDesert;
			FindScript.GetComponent<MenuSystem>().PlayerScreen.SetActive(true);
			Application.LoadLevel("Desert");
		}else if(PlayerPrefs.GetInt ("PlayerLevel") == 2){
			FindScript.GetComponent<MenuSystem>().PlayerScreen.GetComponent<RawImage>().texture = FindScript.GetComponent<MenuSystem>().LoadingCity;
			FindScript.GetComponent<MenuSystem>().PlayerScreen.SetActive(true);
			Application.LoadLevel("City");
		}
	}
}

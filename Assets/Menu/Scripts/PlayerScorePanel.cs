using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerScorePanel : MonoBehaviour {

	new public Text name;
	public Text kills;
	public Text score;
	public Text deaths;
		
	public void SetPlayerScoreText(string a,string b,string c,string d){
		name.text = a;
		score.text = b;
		kills.text = c;
		deaths.text = d;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AmmoPackUI : MonoBehaviour {
	public Text AmmoPackText;

	private int AmmoPack;
	// Update is called once per frame
	void Update () {
		//Debug.Log (PlayerPrefs.GetInt ("PlayerBullets"));
		if (AmmoPack != PlayerPrefs.GetInt ("PlayerBullets")) {
			AmmoPack = PlayerPrefs.GetInt ("PlayerBullets");
			AmmoPackText.text = AmmoPack.ToString();
		}
	}
}

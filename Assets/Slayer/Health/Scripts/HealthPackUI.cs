using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HealthPackUI : MonoBehaviour {

	public Text HealthPackText;

	private int HealthPack;
	// Update is called once per frame
	void Update () {
		if (HealthPack != PlayerPrefs.GetInt ("PlayerHealthpack")) {
			HealthPack = PlayerPrefs.GetInt ("PlayerHealthpack");
			HealthPackText.text = HealthPack.ToString();
		}
	}
}

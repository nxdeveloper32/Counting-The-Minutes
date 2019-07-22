using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnemyIntroNavigate : MonoBehaviour {

	public GameObject Sloth;
	public GameObject Patient;
	public GameObject Joker;
	public GameObject AngrySheriff;
	private int currenltyActive;
	private bool Change;
	void Start(){
		currenltyActive = 0;
		Change = false;
	}
	void Update(){
		if(Change){
			switch(currenltyActive){
			case 0:
				{
					Sloth.SetActive (true);
					Patient.SetActive (false);
					Joker.SetActive (false);
					AngrySheriff.SetActive (false);
					Change = false;
					break;
				}
			case 1:
				{
					Sloth.SetActive (false);
					Patient.SetActive (true);
					Joker.SetActive (false);
					AngrySheriff.SetActive (false);
					Change = false;
					break;
				}
			case 2:
				{
					Sloth.SetActive (false);
					Patient.SetActive (false);
					Joker.SetActive (true);
					AngrySheriff.SetActive (false);
					Change = false;
					break;
				}
			case 3:
				{
					Sloth.SetActive (false);
					Patient.SetActive (false);
					Joker.SetActive (false);
					AngrySheriff.SetActive (true);
					Change = false;
					break;
				}
			}
		}
	}
	public void previousButton(){
		if(currenltyActive == 0)
			return;
		currenltyActive -= 1;
		Change = true;
	}
	public void NextButton(){
		if(currenltyActive == 3)
			return;
		currenltyActive += 1;
		Change = true;
	}
}

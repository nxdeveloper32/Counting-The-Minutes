using UnityEngine;
using System.Collections;

public class AmmoBoxPickUp : MonoBehaviour {
	public GameObject PickUpNotification;
	private bool Pick;
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player")) {
			PickUpNotification.SetActive (true);
			Pick = true;
		}
	}
	void OnTriggerExit(Collider other){
		if (other.CompareTag("Player")) {
			PickUpNotification.SetActive (false);
			Pick = false;
		}
	}
	void Update(){
		if (Pick) {
			if (Input.GetButtonDown("PickUp")) {
				PlayerPrefs.SetInt ("PlayerBullets", (PlayerPrefs.GetInt ("PlayerBullets")+10));
				Destroy (gameObject);
				PickUpNotification.SetActive (false);
			}
		}
	}
}

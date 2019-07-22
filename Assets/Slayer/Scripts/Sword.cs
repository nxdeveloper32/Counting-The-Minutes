using UnityEngine;
using System.Collections;
public class Sword : MonoBehaviour {
	public Player_Sword_Gun PSG;
	public GameObject YakuNotification;
	void Awake(){
		PSG = transform.root.GetComponent<Player_Sword_Gun> ();
	}
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Enemy") && PSG.PlayerAttacking == true) {
			other.gameObject.GetComponent<Enemy>().Hit(20);
			other.gameObject.GetComponent<Enemy> ().pursuing = true;
		}
		if (other.CompareTag("EnemyJill") && PSG.PlayerAttacking == true) {
			other.gameObject.GetComponent<JillEnemy>().Hit(100);
			other.gameObject.GetComponent<JillEnemy> ().pursuing = true;
		}
		if (other.CompareTag("Yaku") && PSG.PlayerAttacking == true) {
			YakuNotification.SetActive (true);
			StartCoroutine (DisableNotification ());
			other.gameObject.GetComponent<YakuEnemy> ().pursuing = true;
		}
		if (other.CompareTag("SLusth") && PSG.PlayerAttacking == true) {
			other.gameObject.GetComponent<SLusth_Enemy>().Hit(100);
			other.gameObject.GetComponent<SLusth_Enemy> ().pursuing = true;
		}
	}
	IEnumerator DisableNotification(){
		yield return new WaitForSeconds (1);
		YakuNotification.SetActive (false);
	}
}

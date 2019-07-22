using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	void OnTriggerEnter ( Collider other){
		if (other.CompareTag("Player")) {
			transform.GetChild(0).gameObject.SetActive (true);
		}
	}
}

using UnityEngine;
using System.Collections;

public class EnemySword : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player")) {
			if (transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Enemy>().EnemyAttacking == true) {
				other.gameObject.GetComponent<Player_Health>().Hit(10);
			
			}
		}
	}
}

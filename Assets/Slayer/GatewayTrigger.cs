using UnityEngine;
using System.Collections;

public class GatewayTrigger : MonoBehaviour {
	public GameObject Enemy;
	private NavMeshAgent EnemyAi;
	void Start(){
		EnemyAi = Enemy.GetComponent<NavMeshAgent> ();
	}
	void OnTriggerEnter(Collider player){
		if (player.CompareTag( "Player")) {
			EnemyAi.enabled = true;
			Debug.Log("Player Entered");
		}
	}
}

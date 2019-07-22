using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JillEnemy : MonoBehaviour {
	private NavMeshAgent EnemyAgent;
	public GameObject Player;
	public bool Walk;
	public bool Run;
	private float angle;
	public bool pursuing = false;
	private float health = 100;
	private Image healthBar;
	private bool Isdead = false;
	private bool hitplayer = true;
	public bool EnemyAttacking = false;
	Animator myAnim;
	private AudioSource EnemySound;
	private bool move = true;

	void Start () {
		healthBar = transform.FindChild ("EnemyCanvas").FindChild ("HealthBG").FindChild ("Health").GetComponent<Image> ();
		EnemyAgent = GetComponent<NavMeshAgent> ();
		myAnim = GetComponent<Animator> ();
		EnemySound = GetComponent<AudioSource> ();

	}
	public void Hit(float Damage){
		health -= Damage;
		healthBar.fillAmount = health / 100;
		if (health <= 0) {
			EnemyAttacking = false;
			EnemySound.Stop ();
			Isdead = true;
			EnemyAgent.speed = 0;
			myAnim.SetTrigger ("Die");
			PlayerPrefs.SetInt ("PlayerKills", PlayerPrefs.GetInt ("PlayerKills") + 1);
			move = false;
			StartCoroutine (DeadEnemy (5.0f));
		}
	}
	IEnumerator DeadEnemy(float waitTime){
		yield return new WaitForSeconds(waitTime);
		Destroy (this.gameObject);

	}
	void Update () {
		if (move == true) {
			movement();
		}
		Vector3 direction = Player.transform.position - transform.position;
		angle = Vector3.Angle (direction, transform.forward);
		if (Vector3.Distance (Player.transform.position, transform.position) < 40 && (angle < 180 || pursuing)) {
			EnemySound.enabled = true;
			Run = true;
			Walk = false;
			pursuing = true;
			transform.rotation = Quaternion.Lerp (transform.rotation,Quaternion.LookRotation (direction), 0.1f);
			if(direction.magnitude < 2 && !Isdead){
				EnemyAttacking = true;
				myAnim.SetBool("Hit",true);
				if (hitplayer) {
					HitUser ();
				}
				Run = false;
				Walk = true;
			}else{
				myAnim.SetBool("Hit",false);
				EnemyAttacking = false;

			}
		} else {
			Run = false;
		}
		myAnim.SetBool("Run",Run);
		myAnim.SetBool("Walk",Walk);
		EnemyAgent.enabled = Run || Walk;
		if (!Walk && !Run) {
			pursuing = false;
			EnemySound.enabled = false;
		}
	}
	public void movement(){
		if (Walk) {
			EnemyAgent.destination = Player.transform.position;
			EnemyAgent.speed = 2;
		}
		if (Run) {
			EnemyAgent.destination = Player.transform.position;
			EnemyAgent.speed = 5.5f;
		}
	}
	IEnumerator HitPlayer(){
		yield return new WaitForSeconds (1.3f);
		if (EnemyAttacking) {
			Player.GetComponent<Player_Health>().Hit(2);
		}
		hitplayer = true;
	}
	private void HitUser(){
		StartCoroutine (HitPlayer ());
		hitplayer = false;
	}
}

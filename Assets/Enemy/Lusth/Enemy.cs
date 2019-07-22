using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	private NavMeshAgent EnemyAgent;
	public GameObject Player;
	public bool SlowWalk;
	public bool Run;
	public Transform head;
	private float angle;
	public bool pursuing = false;
	private float health = 100;
	private Image healthBar;
	private bool Isdead = false;
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
		if (health == 0) {
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
		angle = Vector3.Angle (direction, head.forward);
		if (Vector3.Distance (Player.transform.position, transform.position) < 30 && (angle < 30 || pursuing)) {
			EnemySound.enabled = true;
			Run = true;
			SlowWalk = false;
			pursuing = true;
			transform.rotation = Quaternion.Lerp (transform.rotation,Quaternion.LookRotation (direction), 0.1f);
			if(direction.magnitude < 2 && !Isdead){
				myAnim.SetBool("Hit",true);
				EnemyAttacking = true;
				Run = false;
				SlowWalk = true;
			}else{
				myAnim.SetBool("Hit",false);
				EnemyAttacking = false;

			}
		} else {
			Run = false;
		}
		myAnim.SetBool("Run",Run);
		myAnim.SetBool("SlowWalk",SlowWalk);
		EnemyAgent.enabled = Run || SlowWalk;
		if (!SlowWalk && !Run) {
			pursuing = false;
			EnemySound.enabled = false;
		}

	}
	public void movement(){
		if (SlowWalk) {
			EnemyAgent.destination = Player.transform.position;
			EnemyAgent.speed = 2;
		}
		if (Run) {
			EnemyAgent.destination = Player.transform.position;
			EnemyAgent.speed = 2.5f;
		}
	}
}

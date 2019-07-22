using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SLusth_Enemy : MonoBehaviour {
	
	public GameObject Player;
	public bool Run;
	public bool pursuing = false;
	public bool EnemyAttacking = true;
	public GameObject MuzzleFlash;
	public GameObject BulletPrefab;
	public Transform BulletSpawn;

	private float health = 100;
	private Image healthBar;
	private NavMeshAgent EnemyAgent;
	private float angle;
	private bool Isdead = false;
	Animator myAnim;
	private AudioSource EnemySound;
	private bool move = true;
	public float fireTime;
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
			transform.rotation = Quaternion.Lerp (transform.rotation,Quaternion.LookRotation (direction), 0.1f);
			if (direction.magnitude < 20 && !Isdead) {
				Run = false;
				myAnim.SetBool ("Hit", true);
				if(EnemyAttacking){
					EnemyAttack ();
				}
				if (direction.magnitude < 10 && !Isdead) {
					fireTime = .2f;
				} else {
					fireTime = 1f;
				}
			}else{
				myAnim.SetBool("Hit",false);
			}
		} else {
			Run = false;
			EnemySound.enabled = false;
		}
		myAnim.SetBool("Run",Run);
		EnemyAgent.enabled = Run;
	}
	public void movement(){
		if (Run) {
			EnemyAgent.destination = Player.transform.position;
			EnemyAgent.speed = 2.8f;
		}
	}
	IEnumerator disableMuzzleFlash (){
		yield return new WaitForSeconds (.1f);
		MuzzleFlash.SetActive (false);
	}
	IEnumerator AttackAfterTime(){
		yield return new WaitForSeconds (fireTime);
		Instantiate (BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
		MuzzleFlash.SetActive (true);
		EnemyAttacking = true;
		Player.GetComponent<Player_Health>().Hit(2);
		StartCoroutine (disableMuzzleFlash ());

	}
	public void EnemyAttack(){
		StartCoroutine(AttackAfterTime());
		EnemyAttacking = false;
	}
}

using UnityEngine;
using System.Collections;
public class Player_Sword_Gun : MonoBehaviour {

	public Transform sword , swordUnEquip ,swordEquip;
	public Transform Pistol , PistolUnEquip ,PistolEquip;
	public GameObject InventoryPanel;
	public bool sword_is_equipped;
	public bool Pistol_is_equipped;
	public Animator anim;

	public GameObject MyCam;
	public GameObject MyCamGun;

	public GameObject BulletPrefab;
	public Transform BulletSpawn;
	public bool aim;
	public bool fire;

	public GameObject MuzzleFlash;
	public bool PlayerAttacking = false;
	public IKControlHead IKControlIsActive;
	void Awake(){
		anim = GetComponent <Animator>();
		aim = false;
		fire = false;
		IKControlIsActive = GetComponent<IKControlHead> ();
	}

	void Update(){
		inventory ();
		SwordAttack ();
		Gun_Cam_Mov ();
		PistolAttack ();
	}
	void Gun_Cam_Mov (){
		if (Pistol_is_equipped) {
			if(Input.GetKeyDown("mouse 1")){
				MyCam.SetActive(false);
				MyCamGun.SetActive(true);
				aim = true;
				PlayerPrefs.SetInt ("CamAngle", 0);
				IKControlIsActive.ikActive = true;
			}
			if(Input.GetKeyUp("mouse 1")){
				MyCam.SetActive (true);
				MyCamGun.SetActive(false);
				aim = false;
				PlayerPrefs.SetInt ("CamAngle", 1);
				IKControlIsActive.ikActive = false;
			}
			anim.SetBool("Aim",aim);
		}
	}
	void PistolAttack(){
		if (Input.GetKeyDown ("mouse 0") && Pistol_is_equipped && !Input.GetKey ("tab") && aim) {
			fire = true;
			Shoot ();

		}
		if (Input.GetKeyUp ("mouse 0") && Pistol_is_equipped && !Input.GetKey ("tab")) {
			fire = false;
			MuzzleFlash.SetActive (false);
		}
		anim.SetBool ("Fire",fire);
	}
	void SwordAttack(){
		if (Input.GetKeyDown ("mouse 0") && sword_is_equipped && !Input.GetKey ("tab")) {
			anim.SetBool ("Hit",true);
			PlayerAttacking = true;
		}
		if (Input.GetKeyUp ("mouse 0") && sword_is_equipped && !Input.GetKey ("tab")) {
			anim.SetBool ("Hit",false);
		}
		if (Input.GetKeyDown ("q") && sword_is_equipped) {
			anim.SetBool ("Action",true);
			PlayerAttacking = true;
		}
		if (Input.GetKeyUp ("q") && sword_is_equipped) {
			anim.SetBool ("Action",false);
		}

	}
	IEnumerator playernotAttacking(){
		yield return new WaitForSeconds (1);
		PlayerAttacking = false;
	}
	void inventory(){
		if (Input.GetKey ("tab")) {
			InventoryPanel.SetActive (true);
			Cursor.visible = true;
		} else {
			InventoryPanel.SetActive (false);
			//Cursor.visible = false;
		}
		if (sword_is_equipped) {
			sword.position = swordEquip.position;
			sword.rotation = swordEquip.rotation;
		} else {
			sword.position = swordUnEquip.position;
			sword.rotation = swordUnEquip.rotation;
		}
		if (Pistol_is_equipped) {
			Pistol.position = PistolEquip.position;
			Pistol.rotation = PistolEquip.rotation;
		} else {
			Pistol.position = PistolUnEquip.position;
			Pistol.rotation = PistolUnEquip.rotation;
		}
	}
	public void Sword_Equip(){
		sword_is_equipped = true;
	}
	public void Sword_UnEquip(){
		sword_is_equipped = false;
	}
	public void Pistol_Equip(){
		Pistol_is_equipped = true;
	}
	public void Pistol_UnEquip(){
		Pistol_is_equipped = false;
	}
	void Shoot(){
		RaycastHit hit;
		if (PlayerPrefs.GetInt ("PlayerBullets") != 0) {
			Instantiate (BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
			MuzzleFlash.SetActive (true);
			StartCoroutine (disableMuzzleFlash ());
			PlayerPrefs.SetInt ("PlayerBullets", PlayerPrefs.GetInt ("PlayerBullets") - 1);
			if (Physics.Raycast (MyCamGun.transform.position, MyCamGun.transform.forward,out hit, 30.0f)) {
				if (hit.transform.tag == "Enemy") {
					if (Pistol_is_equipped == true) {
						hit.transform.gameObject.GetComponent<Enemy> ().Hit (10);
						hit.transform.gameObject.GetComponent<Enemy> ().pursuing = true;
					}
				}
				if (hit.transform.tag == "EnemyJill") {
					if (Pistol_is_equipped == true) {
						hit.transform.gameObject.GetComponent<JillEnemy> ().Hit (50);
						hit.transform.gameObject.GetComponent<JillEnemy> ().pursuing = true;
					}
				}
				if (hit.transform.tag == "Yaku") {
					if (Pistol_is_equipped == true) {
						hit.transform.gameObject.GetComponent<YakuEnemy> ().Hit (20);
						hit.transform.gameObject.GetComponent<YakuEnemy> ().pursuing = true;
					}
				}
				if (hit.transform.tag == "SLusth") {
					if (Pistol_is_equipped == true) {
						hit.transform.gameObject.GetComponent<SLusth_Enemy> ().Hit (10);
						hit.transform.gameObject.GetComponent<SLusth_Enemy> ().pursuing = true;
					}
				}
			}
		} else {
			AudioSource GunEmpty = GetComponent<AudioSource> ();
			GunEmpty.Play ();
		}
	}
	IEnumerator disableMuzzleFlash (){
		yield return new WaitForSeconds (.2f);
		MuzzleFlash.SetActive (false);
	}
}



















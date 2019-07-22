using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour {
	Animator myanim;
	public Transform Center_point;
	public Transform myCamera;
	public float rotDir_speed;
	public bool forward,back,left,right,slow_walk;
	int angle_to_rotate;
	public float JumpSpeed = 8f;
	public bool camflag = false;
	void Start () {
		myanim = GetComponent<Animator>();
		slow_walk = false;
	}
	void Awake(){
		Camerafollow.UseExistingOrCreateNewMainCamera ();
		PlayerPrefs.SetInt ("CamAngle", 1);
	}
	
	// Update is called once per frame
	void Update () {
		Player_Inputs ();
		Player_walk_run ();
		Player_Jump ();
		if (camflag == false && PlayerPrefs.GetInt ("CamAngle") == 1) {
			Center_point.eulerAngles = new Vector3 (0, myCamera.eulerAngles.y, 0);
			camflag = true;
		}
	}
	void Player_Inputs(){
		forward = Input.GetKey ("w");
		back = Input.GetKey ("s");
		left = Input.GetKey ("a");
		right = Input.GetKey ("d");
	}
	void Player_walk_run(){
		if (Input.GetKey ("left shift")) {
			slow_walk = false;
		} else {
			slow_walk = true;
		}

		myanim.SetBool("slow_walk",slow_walk);
		if (PlayerPrefs.GetInt ("CamAngle") == 1) {
			cal_angle ();
			myanim.SetFloat("movement",Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")),Mathf.Abs(Input.GetAxis("Vertical"))));
		} else if (PlayerPrefs.GetInt ("CamAngle") == 0) {
			strafe_Walk ();
			camflag = false;
		}
		if (myanim.GetCurrentAnimatorStateInfo (0).IsTag ("turnable")) {
			transform.eulerAngles += new Vector3 (0f,Mathf.DeltaAngle(transform.eulerAngles.y,Center_point.eulerAngles.y + angle_to_rotate) * Time.deltaTime*rotDir_speed,0f);
		}
	}
	void strafe_Walk(){
		myanim.SetFloat ("Horizontal", Input.GetAxis ("Horizontal"));
		myanim.SetFloat ("Vertical", Input.GetAxis ("Vertical"));
	}
	void cal_angle(){
		if (forward && !back) {
			if (left && !right) {
				angle_to_rotate = -45;
			} else if (!left && right) {
				angle_to_rotate = 45;
			} else {
				angle_to_rotate = 0 ;
			}
		} else if (!forward && back) {
			if (left && !right) {
				angle_to_rotate = -135;
			} else if (!left && right) {
				angle_to_rotate = 135;
			} else {
				angle_to_rotate = 180;
			}
		} else {
			if (left && !right) {
				angle_to_rotate = -90;
			} else if (!left && right) {
				angle_to_rotate = 90;
			}
		}
	}
	void Player_Jump (){
		if (Input.GetKeyDown ("space")) {
			myanim.SetBool("Jump",true);
		}
		if (Input.GetKeyUp ("space")) {
			myanim.SetBool("Jump",false);
		}
	}
}

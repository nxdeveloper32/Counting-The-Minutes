using UnityEngine;
using System.Collections;

public class CameraFollowGun : MonoBehaviour {

	public Transform Player;
	public Transform PlayerSpine;
	public Transform TargetLookAt;
	void Update(){
		TargetLookAt.position = Player.position + new Vector3 (0f, 2f, 0);
		TargetLookAt.eulerAngles += new Vector3 (-Input.GetAxis("Mouse Y") * 60 ,Input.GetAxis("Mouse X") * 100,0) * Time.deltaTime;
		transform.eulerAngles += new Vector3 (-Input.GetAxis("Mouse Y") * 100,0,0) * Time.deltaTime;
		Player.eulerAngles += new Vector3 (0f,Mathf.DeltaAngle(Player.eulerAngles.y,TargetLookAt.eulerAngles.y) * Time.deltaTime * 20,0f);
	}
	void LateUpdate(){
	}
}

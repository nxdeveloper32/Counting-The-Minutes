using UnityEngine;
using System.Collections;

public class Camerafollow : MonoBehaviour {
	public static Camerafollow Instance;
	public Transform TargetLookAt;
	public Transform player;
	public float Distance = 5f;
	public float DistanceMin = 3f;
	public float DistanceMax = 10f;
	public float DistanceSmooth = 0.05f;
	public float DistanceResumeSmooth = 1f;
	public float X_MouseSensitivity = 5f;
	public float Y_MouseSensitivity = 5f;
	public float MouseSensitivity = 5f;
	public float Y_MinLimit = -40f;
	public float Y_MaxLimit = 80f;
	
	private float mouseX = 0f;
	private float mouseY = 0f;
	private float velX = 0f;
	private float velY = 0f;
	private float velZ = 0f;
	private float velDistance = 0f;
	private float StartDistance = 0f;
	private Vector3 CamPosition = Vector3.zero;
	private Vector3 desiredPosition = Vector3.zero;
	private float desiredDistance = 0f;
	private float distanceSmooth =0f;
	private float preOccludedDistance =0;
	
	void Awake(){
		Instance = this;
	}
	void Start(){
		Distance = Mathf.Clamp (Distance, DistanceMin, DistanceMax);
		StartDistance = Distance;
		Reset ();

	}
	public static void UseExistingOrCreateNewMainCamera(){
		GameObject tempCamera;
		GameObject targetLookAt;
		Camerafollow myCamera;
		
		if (Camera.main != null) {
			tempCamera = Camera.main.gameObject;
		} else {
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent<Camera>();
			tempCamera.tag = "MainCamera";
		}
		//tempCamera.AddComponent<Camerafollow>();
		myCamera = tempCamera.GetComponent ("Camerafollow")as Camerafollow;
		
		targetLookAt = GameObject.Find ("targetLookAt") as GameObject;
		
		if (targetLookAt == null) {
			targetLookAt = new GameObject("targetLookAt");
			
		}
		targetLookAt.transform.position = Vector3.zero;
		myCamera.TargetLookAt = targetLookAt.transform;
	}
	public float OcclusionDistanceStep = 0.5f;
	public int MaxOcclusionChecks = 10;
	bool CheckIfoccluded(int count){
		var isOccluded = false;
		
		var nearestDistance = CheckCameraPoint (TargetLookAt.position, desiredPosition);
		
		if(nearestDistance != -1){
			if(count < MaxOcclusionChecks){
				isOccluded = true;
				Distance -= OcclusionDistanceStep;
				if(Distance < 0.25f){
					Distance = 0.25f;
				}
			}else{
				Distance = nearestDistance - Camera.main.nearClipPlane;
			}
			desiredDistance = Distance;
			distanceSmooth = DistanceResumeSmooth;
		}
		
		return isOccluded;
	}
	float CheckCameraPoint(Vector3 from, Vector3 to){
		var nearestDistance = -1f;
		
		RaycastHit hitInfo;
		
		Helper.ClipPlanePoints clipPlanePoint = Helper.ClipPlaneAtNear (to);
		
		Debug.DrawLine (from, to + transform.forward * -Camera.main.nearClipPlane , Color.red);
		Debug.DrawLine (from, clipPlanePoint.UpperLeft);
		Debug.DrawLine (from, clipPlanePoint.LowerLeft);
		Debug.DrawLine (from, clipPlanePoint.UpperRight);
		Debug.DrawLine (from, clipPlanePoint.LowerRight);
		
		Debug.DrawLine (clipPlanePoint.UpperLeft,clipPlanePoint.UpperRight);
		Debug.DrawLine (clipPlanePoint.UpperRight,clipPlanePoint.LowerRight);
		Debug.DrawLine (clipPlanePoint.LowerRight,clipPlanePoint.LowerLeft);
		Debug.DrawLine (clipPlanePoint.LowerLeft,clipPlanePoint.UpperLeft);
		
		if (Physics.Linecast (from, clipPlanePoint.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player" && hitInfo.collider.tag != "Enemy" && hitInfo.collider.tag != "ESword" && hitInfo.collider.tag != "PSword" && hitInfo.collider.tag != "Walls" && hitInfo.collider.tag != "EnemyJill" && hitInfo.collider.tag != "Yaku" && hitInfo.collider.tag != "SLusth") {
			nearestDistance = hitInfo.distance;
		}
		if (Physics.Linecast (from, clipPlanePoint.LowerLeft, out hitInfo) && hitInfo.collider.tag != "Player" && hitInfo.collider.tag != "Enemy" && hitInfo.collider.tag != "ESword" && hitInfo.collider.tag != "PSword" && hitInfo.collider.tag != "Walls" && hitInfo.collider.tag != "EnemyJill" && hitInfo.collider.tag != "Yaku" && hitInfo.collider.tag != "SLusth") {
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		if (Physics.Linecast (from, clipPlanePoint.UpperRight, out hitInfo) && hitInfo.collider.tag != "Player" && hitInfo.collider.tag != "Enemy" && hitInfo.collider.tag != "ESword" && hitInfo.collider.tag != "PSword" && hitInfo.collider.tag != "Walls" && hitInfo.collider.tag != "EnemyJill" && hitInfo.collider.tag != "Yaku" && hitInfo.collider.tag != "SLusth") {
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		if (Physics.Linecast (from, clipPlanePoint.LowerRight, out hitInfo) && hitInfo.collider.tag != "Player" && hitInfo.collider.tag != "Enemy" && hitInfo.collider.tag != "ESword" && hitInfo.collider.tag != "PSword" && hitInfo.collider.tag != "Walls" && hitInfo.collider.tag != "EnemyJill" && hitInfo.collider.tag != "Yaku" && hitInfo.collider.tag != "SLusth") {
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		if (Physics.Linecast (from, to + transform.forward * -Camera.main.nearClipPlane, out hitInfo) && hitInfo.collider.tag != "Player" && hitInfo.collider.tag != "Enemy" && hitInfo.collider.tag != "ESword" && hitInfo.collider.tag != "Walls" && hitInfo.collider.tag != "EnemyJill" && hitInfo.collider.tag != "Yaku" && hitInfo.collider.tag != "SLusth") {
			if(hitInfo.distance < nearestDistance || nearestDistance == -1)
				nearestDistance = hitInfo.distance;
		}
		return nearestDistance;
	}
	void ResetDesiredDistance(){
		if (desiredDistance < preOccludedDistance) {
			var pos = CalculatePosition(mouseY,mouseX,preOccludedDistance);
			
			var nearestDistance = CheckCameraPoint(TargetLookAt.position,pos);
			
			if(nearestDistance == -1 || nearestDistance > preOccludedDistance){
				desiredDistance = preOccludedDistance;
			}
		}
	}
	void LateUpdate(){
		TargetLookAt.position = player.position + new Vector3 (0, 1.5f, 0);
		TargetLookAt.eulerAngles += new Vector3 (0,Input.GetAxis("Mouse X") * X_MouseSensitivity,0) * Time.deltaTime;
		if (TargetLookAt == null)
			return;
		
		HandlePlayerInput ();
		
		
		var count = 0;
		do {
			CalculateDesiredPosition ();
			count++;
		} while(CheckIfoccluded(count));
		
		UpdatePosition ();
		
	}
	void HandlePlayerInput(){
		var deadZone = 0.01f;
		mouseX += Input.GetAxis ("Mouse X") * X_MouseSensitivity * Time.deltaTime;
		mouseY += -Input.GetAxis ("Mouse Y") * Y_MouseSensitivity * Time.deltaTime;
		mouseY = Helper.ClampAngle (mouseY, Y_MinLimit, Y_MaxLimit);
		if (Input.GetAxis ("Mouse ScrollWheel") < -deadZone || Input.GetAxis ("Mouse ScrollWheel") > deadZone) {
			desiredDistance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * MouseSensitivity,
			                              DistanceMin,DistanceMax);
			preOccludedDistance = desiredDistance;
			distanceSmooth = DistanceSmooth;
		}
	}
	void CalculateDesiredPosition(){
		ResetDesiredDistance ();
		Distance = Mathf.SmoothDamp (Distance, desiredDistance, ref velDistance, distanceSmooth);
		
		desiredPosition = CalculatePosition (mouseY, mouseX, Distance);
	}
	Vector3 CalculatePosition(float rotationX,float rotationY,float distance){
		Vector3 direction = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (rotationX, rotationY, 0);
		return TargetLookAt.position + rotation * direction;
	}
	void UpdatePosition(){
		var posX = Mathf.SmoothDamp (CamPosition.x, desiredPosition.x, ref velX, 0);
		var posY = Mathf.SmoothDamp (CamPosition.y, desiredPosition.y, ref velY, 0);
		var posZ = Mathf.SmoothDamp (CamPosition.z, desiredPosition.z, ref velZ, 0);
		if (!float.IsNaN (posX) && !float.IsNaN (posY) && !float.IsNaN (posZ)) {
			CamPosition = new Vector3 (posX  , posY , posZ );
			transform.position = CamPosition ;
		}
		transform.LookAt (TargetLookAt);
	}
	public void Reset(){
		mouseX = 0;
		mouseY = 10;
		Distance = StartDistance;
		desiredDistance = Distance;
		preOccludedDistance = Distance;
	}
}




















using UnityEngine;
using System.Collections;

public class IKControlHead : MonoBehaviour {
	protected Animator animator;
	public bool ikActive = false;
	public Transform HeadLookObject;
	public Transform RightHandObject = null;
	public Transform LeftHandObject = null;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void OnAnimatorIK() {
		if (animator) {
			if (ikActive) {
				if (HeadLookObject != null) {
					animator.SetLookAtWeight (1);
					animator.SetLookAtPosition (HeadLookObject.position);
	
				}
				if (RightHandObject != null) {
					animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
					animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
					animator.SetIKPosition(AvatarIKGoal.RightHand,RightHandObject.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand,RightHandObject.rotation);
				}
				if (LeftHandObject != null) {
					animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
					animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);  
					animator.SetIKPosition(AvatarIKGoal.LeftHand,LeftHandObject.position);
					animator.SetIKRotation(AvatarIKGoal.LeftHand,LeftHandObject.rotation);
				}
			}
		}
	}
}

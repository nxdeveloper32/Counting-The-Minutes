using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class CrossHair : MonoBehaviour {
	public GameObject CrossHairManager;
	public RectTransform Top;
	public RectTransform Bottom;
	public RectTransform Left;
	public RectTransform Right;

	private bool movement;
	private bool slow_walk;
	private bool pistol;
	private bool aim;
	private bool fire;


	public Player_Sword_Gun PSG;
	public Player_Movement PM;

	void Update(){
		GetAnimValues ();
		EnableDisableCH ();
	}
	private void GetAnimValues(){
		if (PM.forward || PM.back || PM.right || PM.left) {
			movement = true;
		} else {
			movement = false;
		}
		slow_walk = PM.slow_walk;
		pistol = PSG.Pistol_is_equipped;
		aim = PSG.aim;
		fire = PSG.fire;
	}
	private void EnableDisableCH(){
		if (aim) {
			CrossHairManager.SetActive(true);
			ChangeCrossHair ();
		} else {
			CrossHairManager.SetActive(false);
		}
	}
	private void ChangeCrossHair(){
		if (aim) {
			Top.localPosition = new Vector3 (0,5,0);
			Top.sizeDelta = new Vector2 (1, 5);
			Bottom.localPosition = new Vector3 (0,-5,0);
			Bottom.sizeDelta = new Vector2 (1, 5);
			Left.localPosition = new Vector3 (5,0,0);
			Left.sizeDelta = new Vector2 (1, 5);
			Right.localPosition = new Vector3 (-5,0,0);
			Right.sizeDelta = new Vector2 (1, 5);
		}
		if (!slow_walk) {
			Top.localPosition = new Vector3 (0,20,0);
			Top.sizeDelta = new Vector2 (3, 10);
			Bottom.localPosition = new Vector3 (0,-20,0);
			Bottom.sizeDelta = new Vector2 (3, 10);
			Left.localPosition = new Vector3 (20,0,0);
			Left.sizeDelta = new Vector2 (3, 10);
			Right.localPosition = new Vector3 (-20,0,0);
			Right.sizeDelta = new Vector2 (3, 10);
		}
		if (movement && slow_walk) {
			Top.localPosition = new Vector3 (0,10,0);
			Top.sizeDelta = new Vector2 (2, 8);
			Bottom.localPosition = new Vector3 (0,-10,0);
			Bottom.sizeDelta = new Vector2 (2, 8);
			Left.localPosition = new Vector3 (10,0,0);
			Left.sizeDelta = new Vector2 (2, 8);
			Right.localPosition = new Vector3 (-10,0,0);
			Right.sizeDelta = new Vector2 (2, 8);
		}
		if (fire && slow_walk && !movement ) {
			Top.localPosition = new Vector3 (0,8,0);
			Top.sizeDelta = new Vector2 (1, 5);
			Bottom.localPosition = new Vector3 (0,-8,0);
			Bottom.sizeDelta = new Vector2 (1, 5);
			Left.localPosition = new Vector3 (8,0,0);
			Left.sizeDelta = new Vector2 (1, 5);
			Right.localPosition = new Vector3 (-8,0,0);
			Right.sizeDelta = new Vector2 (1, 5);
		}
		if (fire && !slow_walk) {
			Top.localPosition = new Vector3 (0,23,0);
			Top.sizeDelta = new Vector2 (3, 10);
			Bottom.localPosition = new Vector3 (0,-23,0);
			Bottom.sizeDelta = new Vector2 (3, 10);
			Left.localPosition = new Vector3 (23,0,0);
			Left.sizeDelta = new Vector2 (3, 10);
			Right.localPosition = new Vector3 (-23,0,0);
			Right.sizeDelta = new Vector2 (3, 10);
		}
		if (fire && slow_walk && movement ) {
			Top.localPosition = new Vector3 (0,13,0);
			Top.sizeDelta = new Vector2 (2, 8);
			Bottom.localPosition = new Vector3 (0,-13,0);
			Bottom.sizeDelta = new Vector2 (2, 8);
			Left.localPosition = new Vector3 (13,0,0);
			Left.sizeDelta = new Vector2 (2, 8);
			Right.localPosition = new Vector3 (-13,0,0);
			Right.sizeDelta = new Vector2 (2, 8);
		}
	}
}

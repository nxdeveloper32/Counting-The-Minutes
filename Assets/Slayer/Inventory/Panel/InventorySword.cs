using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventorySword : MonoBehaviour {
	public Texture Sword;
	public Texture SwordSelected;
	public GameObject SwordPanel;
	public Texture Gun;
	public Texture GunSelected;
	public GameObject GunPanel;

	public Player_Sword_Gun PS;
	void Start(){
		Cursor.visible = true;
	}
	void Update(){
		
		if (Input.GetKeyDown (KeyCode.PageUp)) {
			Debug.Log ("hi");
		}
	}
	public void SelectSword(){
		SwordPanel.GetComponent<RawImage>().texture = SwordSelected as Texture;
	}
	public void DeselectSword(){
		SwordPanel.GetComponent<RawImage>().texture = Sword as Texture;
	}
	public void Sword_Select(){
		PS.anim.SetTrigger("Sword");
	}
	public void SelectGun(){
		GunPanel.GetComponent<RawImage> ().texture = GunSelected as Texture;
	}
	public void DeselectGun(){
		GunPanel.GetComponent<RawImage> ().texture = Gun as Texture;
	}
	public void Gun_Select(){
		PS.anim.SetTrigger("Pistol");
	}
}

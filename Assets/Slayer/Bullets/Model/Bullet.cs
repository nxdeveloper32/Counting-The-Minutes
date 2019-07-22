using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private Rigidbody Rb;
	void Start(){
		Rb = GetComponent<Rigidbody> ();
		Rb.AddForce (Vector3.right * .2f);
		Rb.AddForce (Vector3.up * .02f);
		StartCoroutine (DeleteBulletPrefab());
	}
	IEnumerator DeleteBulletPrefab(){
		yield return new WaitForSeconds (5);
		Destroy (this.gameObject);
	}
}

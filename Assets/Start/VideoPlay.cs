using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent (typeof(AudioSource))]
public class VideoPlay : MonoBehaviour {
	public MovieTexture movie;
	AudioSource audio;
	public Texture Loading;
	// Use this for initialization
	void Start () {
		GetComponent<RawImage> ().texture = movie as MovieTexture;
		audio = GetComponent<AudioSource> ();
		audio.clip = movie.audioClip;
		movie.Play ();
		audio.Play ();
	}
	void Update () {
		if (!movie.isPlaying || Input.GetKeyDown("space")) {
			audio.Stop();
			movie.Stop();
			GetComponent<RawImage> ().texture = Loading;
			StartCoroutine(LoadNewScene());
		}
	}
	IEnumerator LoadNewScene() {
		
		yield return new WaitForSeconds(3);
		Application.LoadLevel("Desert");
		
	}
}

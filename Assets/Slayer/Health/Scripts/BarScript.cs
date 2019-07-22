using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BarScript : MonoBehaviour {
	[SerializeField]
	private float fillAmount;
	[SerializeField]
	private Image content;
	public float CurrentHealth = 100;
	public float MaxHealth = 100;
	public Color FullColor;
	public Color LowColor;
	// Update is called once per frame
	void Update () {
		HandleBar ();
	}
	private void HandleBar(){
		content.fillAmount = Mathf.Lerp(content.fillAmount,Map(CurrentHealth,MaxHealth),Time.deltaTime * 3);
		content.color = Color.Lerp (LowColor, FullColor, Map (CurrentHealth, MaxHealth));
	}
	private float Map(float value,float inMax){
		if (value == 0) {
			return 0;
		}else if(value > inMax){
			return inMax;
		}else {
			return value / inMax;
		}
	}
}













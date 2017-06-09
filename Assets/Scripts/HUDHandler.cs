using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {

	public Text PlayerScoreText;
	public Text OpponentScoreText;
	public Text TimerText;
	public GameObject TimerBar;

	public Image HurtImage;
	public float HurtTime = 0.5f;
	public float HurtTransparencyMax = 0.7f;

	public Image SwapImage;
	public float SwapTime = 0.5f;
	public float SwapTransparencyMax = 0.7f;

	bool _hurtAnimation = false;
	bool _swapAnimation = false;

	public void ScoreData (int player, int opponent) {
		PlayerScoreText.text = player.ToString();
		OpponentScoreText.text = opponent.ToString();
	}

	public void Timer (float timer) {
		if (timer == -1) {
			TimerBar.SetActive(false);
			TimerText.text = "";
		}
		else {
			TimerBar.SetActive(true);
			TimerText.text = Mathf.FloorToInt(timer).ToString();
		}
	}

	public void Hurt () {
		if (!_hurtAnimation) {
			_hurtAnimation = true;
			StartCoroutine(HurtAnimation());
		}
	}

	IEnumerator HurtAnimation()
	{
		float value = 0;

		while(value < HurtTransparencyMax)
		{
			yield return new WaitForFixedUpdate ();
			value += Time.deltaTime * (1 / (HurtTime * 0.5f));
			Color _newColor = HurtImage.color; 
			_newColor.a = value;
			HurtImage.color = _newColor;
		}

		while(value > 0)
		{
			yield return new WaitForFixedUpdate ();
			value -= Time.deltaTime * (1 / (HurtTime * 0.5f));
			Color _newColor = HurtImage.color; 
			_newColor.a = value;
			HurtImage.color = _newColor;
		}

		_hurtAnimation = false;
	}

	public void Swap () {
		if (!_swapAnimation) {
			_swapAnimation = true;
			StartCoroutine(SwapAnimation());
		}
	}

	IEnumerator SwapAnimation()
	{
		float value = 0;

		while(value < SwapTransparencyMax)
		{
			yield return new WaitForFixedUpdate ();
			value += Time.deltaTime * (1 / (SwapTime * 0.5f));
			Color _newColor = SwapImage.color; 
			_newColor.a = value;
			SwapImage.color = _newColor;
		}

		while(value > 0)
		{
			yield return new WaitForFixedUpdate ();
			value -= Time.deltaTime * (1 / (SwapTime * 0.5f));
			Color _newColor = SwapImage.color; 
			_newColor.a = value;
			SwapImage.color = _newColor;
		}

		_swapAnimation = false;
	}
}

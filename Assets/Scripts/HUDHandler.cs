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

	bool _animation = false;

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
		if (!_animation) {
			_animation = true;
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

		_animation = false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerandPoint : MonoBehaviour {

	[SerializeField]
	private float maxTime = 60;
	private float timeDecrease = 2;
	private float decreaseRate = 1;
	private float scoreMultiplier = 100;
	private float score = 0;
	private bool pause = false;

	private UnityEngine.UI.Slider timer;
	private UnityEngine.UI.Text scoreText;
	private UnityEngine.UI.Text timerText;

	private LevelController lc;

	void Start () {
		score = 0;
		lc = GameObject.Find ("GM").GetComponent<LevelController>();
		timer = GameObject.Find ("Canvas").transform.Find ("TimerSlider").GetComponent<UnityEngine.UI.Slider>();
		scoreText = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<UnityEngine.UI.Text>();
		timerText = GameObject.Find("Canvas").transform.Find("TimerText").GetComponent<UnityEngine.UI.Text>();
		timer.maxValue = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (!pause) {
			timer.value -= Time.deltaTime * decreaseRate;
		}

		timerText.text = "Times Left: "+Mathf.Round(timer.value * 100f)/100f+ "s";
		scoreText.text = "Scores: "+score;

		if (timer.value <= 0) {
			//ResetTimer ();
		}
	}

	public void ResetTimer()
	{
		CalculatePoints ();
		maxTime -= timeDecrease;
		timer.value = maxTime;

	}

	public void CalculatePoints()
	{
		score += Mathf.Round (timer.value * scoreMultiplier);
	}

	public IEnumerator NextLevel()
	{
		pause = true;
		yield return new WaitForSeconds (1f);
		lc.RotateLevel ();
		ResetTimer ();
		yield return new WaitForSeconds (0.5f);
		pause = false;
	}
}

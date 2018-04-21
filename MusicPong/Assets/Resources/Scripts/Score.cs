using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour{

	public static Score Instance = null; 
	
	public Text ScoreGuiText;
	public Text HighScoreGuiText;
	public Text HitGuiText;
	public string Name; // let Player input name somehow
	private int _score;
	private int _highScore;
	public int PerfectHit;
	public int GreatHit;
	public int GoodHit;
	public int Missed;
	private string highScoreKey = "highScore";
	private string playerName = "playerName";

	private Vector3 HitTextFinalSize = new Vector3(0.6f, 0.6f,0.6f);
	
	public enum Hit
	{
		Perfect,
		Great,
		Good,
		Missed
	}

	void Start () {
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		Initialise ();
	}

	void Update ()
	{
		// If the Score is higher than the High Score…
		if (_highScore < _score) {
			_highScore = _score;
		}
		//Debug.Log (score);
		// Display both the Score and High Score
		if (ScoreGuiText == null) return;
		ScoreGuiText.text = _score.ToString ();
	}

	private void Initialise () {
		_highScore = PlayerPrefs.GetInt (highScoreKey, 0);
		Debug.Log (_highScore);
	}

	public void AddHitCount(Hit hitType)
	{
		if (hitType == Hit.Perfect)
		{
			PerfectHit++;
			AddPoint(3);
			StartCoroutine(ShowHitGuiText(Hit.Perfect,30));
		}
		else if (hitType == Hit.Great)
		{
			GreatHit++;
			AddPoint(2);
			StartCoroutine(ShowHitGuiText(Hit.Great,30));
		} 
		else if (hitType == Hit.Good)
		{
			GoodHit++;
			AddPoint(1);
			StartCoroutine(ShowHitGuiText(Hit.Good,30));
		} 
		else if (hitType == Hit.Missed)
		{
			Missed++;
		}
	}

	private IEnumerator ShowHitGuiText(Hit type, float animDuration)
	{
		var duration = 0f;
		HitGuiText.transform.localScale = Vector3.zero;
		while (duration < animDuration)
		{
			HitGuiText.transform.localScale = Vector3.Lerp(Vector3.zero, HitTextFinalSize, duration*0.2f);
			duration += Time.deltaTime;
		}
		HitGuiText.text = type.ToString();
		yield return null;

	}

	public void AddPoint(int point)
	{
		_score = _score + point;
		Debug.LogError(_score);
	}

	public void Save() {
		//PlayerPrefs.SetName and shit 
		//Saving after scene ends
		//PlayerPrefs.SetString(playerName, name);
		PlayerPrefs.SetInt (highScoreKey, _highScore);
		PlayerPrefs.Save ();
		Initialise ();
	}

	public void Reset()
	{
		_score = 0;
	}
}


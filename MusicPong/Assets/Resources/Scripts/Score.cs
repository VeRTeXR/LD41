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
	private bool _isAnimatingScoreText;
	private bool _isAnimatingGrowing;
	private bool _isAnimatingShrink;

	private Vector3 HitTextFinalSize = new Vector3(0.8f, 0.8f,0.8f);
	
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

		if (_isAnimatingScoreText)
		{
			HitGuiText.transform.localScale = Vector3.Lerp(HitGuiText.transform.localScale, HitTextFinalSize, Time.deltaTime*3);
			if (HitGuiText.transform.localScale.x >= 0.6f)
			{
				HitGuiText.transform.localScale = Vector3.zero;
				_isAnimatingScoreText = false;
			}
		}

	}

	private void Initialise () {
		_highScore = PlayerPrefs.GetInt (highScoreKey, 0);
	}

	public void AddHitCount(Hit hitType)
	{
		if (hitType == Hit.Perfect)
		{
			PerfectHit++;
			AddPoint(3);
			ShowHitGuiText(Hit.Perfect);
		}
		else if (hitType == Hit.Great)
		{
			GreatHit++;
			AddPoint(2);
			ShowHitGuiText(Hit.Great);
		} 
		else if (hitType == Hit.Good)
		{
			GoodHit++;
			AddPoint(1);
			ShowHitGuiText(Hit.Good);
		} 
		else if (hitType == Hit.Missed)
		{
			Missed++;
		}
	}

	public int GetTotalScore()
	{
		return _score;
	}

	private void ShowHitGuiText(Hit type)
	{
		
		HitGuiText.transform.localScale = Vector3.zero;
		HitGuiText.text = type.ToString();
		_isAnimatingScoreText = true;

	}

	public void AddPoint(int point)
	{
		_score = _score + point;
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


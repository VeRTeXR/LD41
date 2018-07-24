using System.Collections;
using UnityEngine;


public class StartOptions : MonoBehaviour {
	public bool InMainMenu = true;
	public Animator AnimColorFade;
	public Animator AnimMenuAlpha;
	public Animator AnimRetryAlpha;
	public Animator GameplayMenuAlpha;
	public AnimationClip FadeColorAnimationClip;
	public AnimationClip FadeAlphaAnimationClip;
	public Pause PauseScript;
	public bool ChangeMusicOnStart;

	private PlayMusic _playMusic;
	private float fastFadeIn = .01f;
	private ShowPanels _showPanels;
	private GameObject _player;
	private GameObject _startOptionSelector; 

	public void Awake()
	{
		InitializeMenuAndPauseGameplay();
	}

	private void InitializeMenuAndPauseGameplay()
	{
		_showPanels = GetComponent<ShowPanels>();
		PauseScript = GetComponent<Pause>();
		_playMusic = GetComponent<PlayMusic>();
		SetUnscaleUiAnimatorUpdateMode();
		PauseScript.DoPause();
	}


	private void SetUnscaleUiAnimatorUpdateMode()
	{
		AnimMenuAlpha.updateMode = AnimatorUpdateMode.UnscaledTime;
		AnimColorFade.updateMode = AnimatorUpdateMode.UnscaledTime;
	}


	public void StartButtonClicked()
	{
		FadeOutMusicOnStartIfAppropriate();
		StartGameInScene();
	}

	private void FadeOutMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
		{
			_playMusic.FadeDown(FadeColorAnimationClip.length);
		}
	}

	public IEnumerator HideRetry()
	{
		yield return new WaitForSecondsRealtime(FadeAlphaAnimationClip.length);
		_showPanels.HideRetry();
		AnimMenuAlpha.ResetTrigger("fade");
	}	
	
	public IEnumerator HideDelayed()
	{
		yield return new WaitForSecondsRealtime(FadeAlphaAnimationClip.length);
		_showPanels.HideMenu();
		AnimMenuAlpha.ResetTrigger("fade");
	}

	public IEnumerator HideGameplay()
	{
		yield return new WaitForSecondsRealtime(FadeAlphaAnimationClip.length);
		_showPanels.HideGameplay();
		GameplayMenuAlpha.ResetTrigger("fade");
		_showPanels.ShowRetryPanel(); 
	}
	
	public void StartGameInScene()
	{
		InMainMenu = false;
		ChangeMusicOnStartIfAppropriate();
		FadeAndDisableMenuPanel();
		_showPanels.ShowGameplay();
		StartCoroutine("UnpauseGameAfterMenuFaded");

	}

	public void GameOver()
	{
		_showPanels.RetryPanel.GetComponent<CanvasGroup>().alpha = 1;
		GameplayMenuAlpha.SetTrigger("fade");
		StartCoroutine("HideGameplay");
		
	}

	public void Retry()
	{
		_showPanels.GameplayPanel.GetComponent<CanvasGroup>().alpha = 1;
		StartGameInScene();
		
		Manager.Instance.RetryResetBall();
		Score.Instance.Reset();
		Manager.Instance.Retry();
		FadeAndDisableRetryPanel();
	}
	
	private void FadeAndDisableMenuPanel()
	{
		AnimMenuAlpha.SetTrigger("fade");
		StartCoroutine("HideDelayed");
	}

	private void FadeAndDisableRetryPanel()
	{
		AnimRetryAlpha.SetTrigger("fade");
		StartCoroutine("HideRetry");
	}
	
	private void ChangeMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
			Invoke("PlayNewMusic", FadeAlphaAnimationClip.length);
	}

	public IEnumerator UnpauseGameAfterMenuFaded()
	{
		yield return new WaitUntil(() => !_showPanels.MenuPanel.gameObject.activeSelf);
		PauseScript.UnPause();
	}

	public void PlayNewMusic()
	{
		_playMusic.FadeUp (fastFadeIn);
		_playMusic.PlaySelectedMusic (1);
	}
}

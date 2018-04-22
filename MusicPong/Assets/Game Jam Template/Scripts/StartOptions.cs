using System.Collections;
using UnityEngine;


public class StartOptions : MonoBehaviour {
	public bool InMainMenu = true;
	public Animator AnimColorFade;
	public Animator AnimMenuAlpha;
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
		_player = GameObject.FindGameObjectWithTag("Player");

		SetPlayerState(false);
		SetUnscaleUiAnimatorUpdateMode();
		PauseScript.DoPause();
	}

	public void SetPlayerState(bool isEnabled)
	{
//		if (_player != null)
//		{
//			_player.gameObject.GetComponent<Player>().enabled = isEnabled;
//			_player.gameObject.GetComponent<Controller2D>().enabled = isEnabled;
//		}
	}

	private void SetUnscaleUiAnimatorUpdateMode()
	{
		AnimMenuAlpha.updateMode = AnimatorUpdateMode.UnscaledTime;
		AnimColorFade.updateMode = AnimatorUpdateMode.UnscaledTime;
	}


	public void StartButtonClicked()
	{
		FadeOutMusicOnStartIfAppropriate();
		SetPlayerState(true);
		StartGameInScene();
	}

	private void FadeOutMusicOnStartIfAppropriate()
	{
		if (ChangeMusicOnStart)
		{
			_playMusic.FadeDown(FadeColorAnimationClip.length);
		}
	}


	public IEnumerator HideDelayed()
	{
		yield return new WaitForSecondsRealtime(FadeAlphaAnimationClip.length);
		_showPanels.HideMenu();
		AnimMenuAlpha.ResetTrigger("fade");
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
		GameplayMenuAlpha.SetTrigger("fade");
		_showPanels.HideGameplay();
		StartCoroutine("HideDelayed");
		_showPanels.ShowFinishPanel(); 
	}

	public void Retry()
	{
		_showPanels.ShowGameplay();
		Score.Instance.Reset();
		Manager.Instance.Retry();
		_showPanels.HideFinishPanel(); 
		
		StartGameInScene();
	}
	
	private void FadeAndDisableMenuPanel()
	{
		AnimMenuAlpha.SetTrigger("fade");
		StartCoroutine("HideDelayed");
		
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

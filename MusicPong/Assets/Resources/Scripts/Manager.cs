using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
	
	public static Manager Instance = null; 
	//private Manager managerScript;
	public GameObject ExitTest;
	public GameObject[] enemy;
	public GameObject player;
	public GameObject spawn;
	//public GameObject ExitTest;
	public float levelStartDelay = 0.3f;
	public float levelStartCountdown;
	public float HP = 3;
	public int level;
	public GameObject StartMenu;
	public GameObject ScoreManager;
	public GameObject GameplayArea;

	private GameObject levelImage;
	private Text levelText;
	private bool doingSetup = true;
	public Pause PauseScript;

	// Title

	void Start () {
		
		HP = 3;
		if (Instance == null) {
			Instance = this;
		}
		else if (Instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		StartMenu = GameObject.FindGameObjectWithTag("StartMenu");
		if (StartMenu != null)
		{
			PauseScript = StartMenu.GetComponent<Pause>();
		}
		GameplayArea = GameObject.FindGameObjectWithTag("GameplayArea");
		InitGame ();
	}

	void OnLevelWasLoaded(int index)  {
		FindObjectOfType<Score> ().Save (); // save everything on level load
	}


	void InitGame()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		spawn = GameObject.FindGameObjectWithTag("Spawn");
		if (GameObject.FindGameObjectWithTag("Player") == null)
		{
			var p = Instantiate(Resources.Load("Prefabs/Player") as GameObject, spawn.transform.position, spawn.transform.rotation);
			p.transform.parent = GameplayArea.transform;
		}
		else
			player.transform.position = spawn.transform.position;
		ScoreManager = Score.Instance.gameObject;
	}

	void Update () {
        if (level < 1)
        {
            HP = 20;
	        Score.Instance.Reset(); //score = 0;
	        //check score, health, reset it! 
        }
		if (Input.GetKeyDown (KeyCode.Z)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			//Application.LoadLevel("StartScn");
			//level = -1;
			//score = 0;
			//HP = 20;
			//Debug.Log(level);
			//	reload will actually reload from beginning
		}
	}

	void HideLevelImage () {
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver() {

		//display node diagram of story
		//level survived?
		//
		StartMenu.GetComponent<StartOptions>().GameOver();
		FindObjectOfType<Score> ().Save ();
//		levelText.text = "You survived " + level + " levels";
//		levelImage.SetActive (true);
	}

	public void Retry()
	{
		Instance.HP = 4;
		InitGame();
	}

	public void OutOfBound()
	{
		Debug.LogError(Instance.HP);
		var HpObject = GameObject.FindWithTag("Hp");
		if (Instance.HP >= 3)
		{
			for (var i = 0; i < HpObject.transform.childCount; i++)
				HpObject.transform.GetChild(i).GetComponent<Image>().color = Color.white;
		}
		Instance.HP--;
		for (var i = 0; i < HpObject.transform.childCount; i++)
		{
			if (Instance.HP - 1 < i)
			{
				HpObject.transform.GetChild(i).GetComponent<Image>().color = Color.black;
			} 
		}
		if (HP <= 0)
			GameOver(); 
		else
			ResetBall();
	}

	private void ResetBall()
	{
		FindObjectOfType<BallSpawner>().ResetBall();
	}

	public void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

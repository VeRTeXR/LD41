using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class UiGameOverController : MonoBehaviour
{
	public GameObject ScoreText; 
	// Use this for initialization
	void Start ()
	{
		ScoreText = transform.GetChild(1).gameObject;
		
		ScoreText.GetComponent<Text>().text = Score.Instance.GetTotalScore().ToString();
	}

	public void ResetScoreText()
	{
		ScoreText.GetComponent<Text>().text = Score.Instance.GetTotalScore().ToString();
	}

	public void RetryScoreText()
	{
		ScoreText.GetComponent<Text>().text = "retry?";
	}
}

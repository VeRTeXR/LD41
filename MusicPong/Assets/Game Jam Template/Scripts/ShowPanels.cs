using UnityEngine;

public class ShowPanels : MonoBehaviour {

	public GameObject OptionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject OptionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject MenuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject PausePanel;							//Store a reference to the Game Object PausePanel 
	public GameObject GameplayPanel;
	public GameObject RetryPanel;
	public GameObject GameplayArea;
	
	public void ShowOptionsPanel()
	{
		OptionsPanel.SetActive(true);
		OptionsTint.SetActive(true);
	}

	public void HideOptionsPanel()
	{
		OptionsPanel.SetActive(false);
		OptionsTint.SetActive(false);
	}

	public void ShowMenu()
	{
		Cursor.visible = true;
		MenuPanel.SetActive (true);
	}

	public void HideMenu()
	{
		MenuPanel.SetActive (false);
	}

	public void HideRetry()
	{
		RetryPanel.SetActive(false);
	}

	public void ShowGameplay()
	{
		Cursor.visible = false;
		GameplayPanel.SetActive(true);
		ShowGameplayArea();
	}

	public void HideGameplay()
	{
		Cursor.visible = true;
		GameplayPanel.SetActive(false);
		HideGameplayArea();
	}

	public void HideGameplayArea()
	{
		GameplayArea.SetActive(false);
	}
	
	public void ShowGameplayArea()
	{
		GameplayArea.SetActive(true);
	}

	public void ShowRetryPanel()
	{
		RetryPanel.SetActive(true);
	}
	
	public void ShowPausePanel()
	{
		Cursor.visible = true;
		PausePanel.SetActive (true);
		OptionsTint.SetActive(true);
	}

	public void HidePausePanel()
	{
		Cursor.visible = false;
		PausePanel.SetActive (false);
		OptionsTint.SetActive(false);
	}
}

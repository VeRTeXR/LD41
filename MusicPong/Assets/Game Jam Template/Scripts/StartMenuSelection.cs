﻿using System.Collections;
using UnityEngine;

public class StartMenuSelection : MonoBehaviour
{
	public GameObject[] _button;
	private bool _inputAllowed;

	public StartMenuSelection(GameObject[] button)
	{
		_button = button;
	}

	void Start ()
	{
		_button = new GameObject[transform.childCount];
		for (var i = 0; i < transform.childCount; i++)
		{
			_button[i] = transform.GetChild(i).gameObject;
		}
		_button[0].transform.GetChild(0).gameObject.SetActive(true);
		_inputAllowed = true;
	}

	void Update()
	{
		if (_inputAllowed)
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
			{
				_inputAllowed = false;
				StartCoroutine(SwitchActiveButtonPosition());
			}
		}

		if (Input.GetKey(KeyCode.Return))
		{
			if (ActiveButton() == 0)
			{
				Manager.Instance.StartMenu.GetComponent<StartOptions>().StartButtonClicked();
			}
			if (ActiveButton() == 1)
			{
				Manager.Instance.StartMenu.GetComponent<QuitApplication>().Quit();
			}
		}
	}

	private int ActiveButton()
	{
		if (_button[0].transform.GetChild(0).gameObject.activeSelf)
		{
			return 0;
		}
		if (_button[1].transform.GetChild(0).gameObject.activeSelf)
			return 1;
		return 1;
	}

	public void DisableMarker()
	{
		_button[0].transform.GetChild(0).gameObject.SetActive(false);
		_button[1].transform.GetChild(0).gameObject.SetActive(false);
	}
	
	private IEnumerator SwitchActiveButtonPosition()
	{
		if (_button[0].transform.GetChild(0).gameObject.activeSelf)
		{
			_button[0].transform.GetChild(0).gameObject.SetActive(false);
			_button[1].transform.GetChild(0).gameObject.SetActive(true);
//			SetSelectedButtonPosition(1);
			yield return new WaitForSecondsRealtime(0.1f);
			_inputAllowed = true;
		}
		else
		{
			_button[1].transform.GetChild(0).gameObject.SetActive(false);
			_button[0].transform.GetChild(0).gameObject.SetActive(true);
//			SetSelectedButtonPosition(0);
			yield return new WaitForSecondsRealtime(0.1f);
			_inputAllowed = true;
		}
	}

//	private void SetSelectedButtonPosition(int selectedButton)
//	{
//		if (selectedButton == 1)
//		{
//			for (var i = 0; i < _button[1].transform.childCount; i++)
//			{
//				_button[1].transform.GetChild(i).transform.localPosition += new Vector3(0, 9.5f, 0);
//			}
//			for (var i = 0; i < _button[0].transform.childCount; i++)
//			{
//				_button[0].transform.GetChild(i).transform.localPosition -= new Vector3(0, 9.5f, 0);
//			}
//		}
//		if (selectedButton == 0)
//		{
//			for (var i = 0; i < _button[0].transform.childCount; i++)
//			{
//				_button[0].transform.GetChild(i).transform.localPosition += new Vector3(0, 9.5f, 0);
//			}
//			for (var i = 0; i < _button[1].transform.childCount; i++)
//			{
//				_button[1].transform.GetChild(i).transform.localPosition -= new Vector3(0, 9.5f, 0);
//			}
//		}
//	}
}

using System;
using UnityEngine;

public class Rating : MonoBehaviour
{
	[SerializeField]
	private GameObject popup;

	[SerializeField]
	private string marketURL;

	public void Show(bool value)
	{
		this.popup.SetActive(value);
		if (value)
		{
			Tracking.instance.Rate_Show();
		}
	}

	public void Rate()
	{
		this.Show(false);
		Tracking.instance.Rate_Action("Rated");
		Application.OpenURL(this.marketURL);
	}

	public void Close()
	{
		this.Show(false);
		Tracking.instance.Rate_Action("Close");
	}
}

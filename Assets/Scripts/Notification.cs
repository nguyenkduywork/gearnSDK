using System;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
	public static Notification instance;

	[SerializeField]
	private Text confirm;

	[SerializeField]
	private Text warning;

	[SerializeField]
	private GameObject warningPopup;

	[SerializeField]
	private GameObject confirmPopup;

	private Action done;

	private void Awake()
	{
		if (Notification.instance == null)
		{
			Notification.instance = this;
		}
		else if (Notification.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Confirm(Action done, string text)
	{
		this.done = done;
		GameUtilities.String.ToText(this.confirm, text);
		Singleton<SoundManager>.Instance.Play("Popup");
		this.confirmPopup.SetActive(true);
	}

	public void Warning(string text)
	{
		this.warningPopup.SetActive(false);
		GameUtilities.String.ToText(this.warning, text);
		this.warningPopup.SetActive(true);
	}

	public void Apply()
	{
		if (this.done != null)
		{
			this.done();
		}
		this.Cancel();
	}

	public void Cancel()
	{
		this.done = null;
		this.confirmPopup.SetActive(false);
	}
}

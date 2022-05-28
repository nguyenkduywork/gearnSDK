using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
	[SerializeField]
	private GameObject popup;

	[SerializeField]
	private Text musicStatusLabel;

	[SerializeField]
	private Text soundStatusLabel;

	[SerializeField]
	private Image musicIconImage;

	[SerializeField]
	private Image soundIconImage;

	[SerializeField]
	private Image musicButtonImage;

	[SerializeField]
	private Image soundButtonImage;

	[SerializeField]
	private Sprite musicEnableSprite;

	[SerializeField]
	private Sprite soundEnableSprite;

	[SerializeField]
	private Sprite musicDisableSprite;

	[SerializeField]
	private Sprite soundDisableSprite;

	[SerializeField]
	private Sprite buttonEnableSprite;

	[SerializeField]
	private Sprite buttonDisableSprite;

	[SerializeField]
	private Color enableOutlineColor;

	[SerializeField]
	private Color disableOutlinecolor;

	public void MusicChange(bool value)
	{
		this.musicIconImage.sprite = ((!value) ? this.musicEnableSprite : this.musicDisableSprite);
		this.musicButtonImage.sprite = ((!value) ? this.buttonEnableSprite : this.buttonDisableSprite);
		this.musicStatusLabel.gameObject.GetComponent<Outline>().effectColor = ((!value) ? this.enableOutlineColor : this.disableOutlinecolor);
		GameUtilities.String.ToText(this.musicStatusLabel, "Music: " + ((!value) ? "On" : "Off"));
	}

	public void SoundChange(bool value)
	{
		this.soundIconImage.sprite = ((!value) ? this.soundEnableSprite : this.soundDisableSprite);
		this.soundButtonImage.sprite = ((!value) ? this.buttonEnableSprite : this.buttonDisableSprite);
		this.soundStatusLabel.gameObject.GetComponent<Outline>().effectColor = ((!value) ? this.enableOutlineColor : this.disableOutlinecolor);
		GameUtilities.String.ToText(this.soundStatusLabel, "Sound: " + ((!value) ? "On" : "Off"));
	}

	public void Show(bool value)
	{
		if (value)
		{
			Singleton<SoundManager>.Instance.Play("Popup");
		}
		this.popup.SetActive(value);
	}
}

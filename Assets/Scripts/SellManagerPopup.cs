using System;
using UnityEngine;
using UnityEngine.UI;

public class SellManagerPopup : MonoBehaviour
{
	private Action onSellHalf;

	private Action onSellFull;

	[SerializeField]
	private Text price;

	[SerializeField]
	private Text duration;

	[SerializeField]
	private Text effective;

	[SerializeField]
	private Text experience;

	[SerializeField]
	private Image skillIamge;

	[SerializeField]
	private Image avatarImage;

	[SerializeField]
	private GameObject popup;

	public void Show(ManagerProfile profile, Action sellHalf, Action sellFull)
	{
		this.onSellHalf = sellHalf;
		this.onSellFull = sellFull;
		this.skillIamge.sprite = Singleton<GameProcess>.Instance.GetManagerSkillSprite(profile.skill, true);
		this.avatarImage.sprite = Singleton<GameProcess>.Instance.GetManagerAvatarSprite(profile.experience);
		GameUtilities.String.ToText(this.price, GameUtilities.Currencies.Convert(profile.price));
		GameUtilities.String.ToText(this.experience, Enum.GetName(typeof(Experience), profile.experience));
		this.experience.color = Singleton<GameProcess>.Instance.GetManagerTextColor(profile.experience);
		this.experience.gameObject.GetComponent<Outline>().effectColor = Singleton<GameProcess>.Instance.GetManagerBorderColor(profile.experience);
		GameUtilities.String.ToText(this.duration, "Duration: " + GameUtilities.DateTime.Convert(Singleton<GameProcess>.Instance.GetManagerSkillDuration(profile.experience, profile.skill)));
		switch (profile.skill)
		{
		case ManagerSkill.UpgradeCost:
			GameUtilities.String.ToText(this.effective, "-" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Upgrade Cost");
			break;
		case ManagerSkill.WalkingSpeed:
			GameUtilities.String.ToText(this.effective, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Walking Speed");
			break;
		case ManagerSkill.CookingSpeed:
			GameUtilities.String.ToText(this.effective, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Cooking Speed");
			break;
		case ManagerSkill.LoadingSpeed:
			GameUtilities.String.ToText(this.effective, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Loading Speed");
			break;
		case ManagerSkill.MovementSpeed:
			GameUtilities.String.ToText(this.effective, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Movement Speed");
			break;
		case ManagerSkill.LoadExpansion:
			GameUtilities.String.ToText(this.effective, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Load Expansion");
			break;
		}
		Singleton<SoundManager>.Instance.Play("Popup");
		this.popup.SetActive(true);
	}

	public void Sell(bool value)
	{
		if (value)
		{
			if (!AdsControl.Instance.GetRewardAvailable())
			{
				Notification.instance.Warning("No available video at the moment.");
				Singleton<SoundManager>.Instance.Play("Notification");
				return;
			}
			AdsControl.Instance.PlayDelegateRewardVideo(delegate
			{
				this.onSellFull();
				this.Cancel();
				Tracking.instance.Ads_Impress("reward", "SellManager");
			});
		}
		else
		{
			this.onSellHalf();
			this.Cancel();
		}
	}

	public void Cancel()
	{
		this.onSellHalf = null;
		this.onSellFull = null;
		this.popup.SetActive(false);
	}
}

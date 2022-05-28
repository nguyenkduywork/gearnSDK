using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerItem : MonoBehaviour
{
	private ManagerProfile profile;

	[SerializeField]
	private GameObject assignedLayout;

	[SerializeField]
	private GameObject unassignLayout;

	[SerializeField]
	private Image skillImage;

	[SerializeField]
	private Image avatarImage;

	[SerializeField]
	private Text durationText;

	[SerializeField]
	private Text effectiveText;

	[SerializeField]
	private Text experienceText;

	public void Initialize(ManagerProfile profile)
	{
		this.profile = profile;
		this.assignedLayout.SetActive(profile.assign);
		this.unassignLayout.SetActive(!profile.assign);
		this.avatarImage.sprite = Singleton<GameProcess>.Instance.GetManagerAvatarSprite(profile.experience);
		this.skillImage.sprite = Singleton<GameProcess>.Instance.GetManagerSkillSprite(profile.skill, profile.state == ManagerState.Ready);
		GameUtilities.String.ToText(this.experienceText, Enum.GetName(typeof(Experience), profile.experience));
		this.experienceText.gameObject.GetComponent<Outline>().effectColor = Singleton<GameProcess>.Instance.GetManagerBorderColor(profile.experience);
		this.experienceText.color = Singleton<GameProcess>.Instance.GetManagerTextColor(profile.experience);
		GameUtilities.String.ToText(this.durationText, "Duration: " + GameUtilities.DateTime.Convert(Singleton<GameProcess>.Instance.GetManagerSkillDuration(profile.experience, profile.skill)));
		switch (profile.skill)
		{
		case ManagerSkill.UpgradeCost:
			GameUtilities.String.ToText(this.effectiveText, "-" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + "% Upgrade Cost");
			break;
		case ManagerSkill.WalkingSpeed:
			GameUtilities.String.ToText(this.effectiveText, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Walking Speed");
			break;
		case ManagerSkill.CookingSpeed:
			GameUtilities.String.ToText(this.effectiveText, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Cooking Speed");
			break;
		case ManagerSkill.LoadingSpeed:
			GameUtilities.String.ToText(this.effectiveText, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Loading Speed");
			break;
		case ManagerSkill.MovementSpeed:
			GameUtilities.String.ToText(this.effectiveText, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Movement Speed");
			break;
		case ManagerSkill.LoadExpansion:
			GameUtilities.String.ToText(this.effectiveText, "x" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + " Load Expansion");
			break;
		}
	}

	public void Assign()
	{
		this.profile.assign = true;
		Singleton<ManagerPopup>.Instance.Assign(this.profile);
		base.gameObject.SetActive(false);
		Tracking.instance.UI_Interaction("ManagerPopup", "AssignManager");
	}

	public void Unassign()
	{
		this.profile.assign = false;
		Singleton<ManagerPopup>.Instance.Unassign(this.profile);
		base.gameObject.SetActive(false);
		Tracking.instance.UI_Interaction("ManagerPopup", "UnassignManager");
	}

	public void Sell()
	{
		Singleton<ManagerPopup>.Instance.SellConfirm(this.profile, base.gameObject);
		Tracking.instance.UI_Interaction("ManagerPopup", "SellManager");
	}

	public void Activate()
	{
		if (this.profile.state != ManagerState.Ready || !this.profile.assign)
		{
			return;
		}
		Location location = this.profile.location;
		if (location != Location.Elevator)
		{
			if (location != Location.Restaurant)
			{
				if (location == Location.Kitchen)
				{
					Singleton<GameManager>.Instance.kitchenController[this.profile.kitchenFloor].managerController.ManagerActivate();
				}
			}
			else
			{
				Singleton<GameManager>.Instance.restaurant.managerController.ManagerActivate();
			}
		}
		else
		{
			Singleton<GameManager>.Instance.elevator.managerController.ManagerActivate();
		}
		this.skillImage.sprite = Singleton<GameProcess>.Instance.GetManagerSkillSprite(this.profile.skill, false);
		Tracking.instance.UI_Interaction("ManagerPopup", "ActiveManager");
	}
}

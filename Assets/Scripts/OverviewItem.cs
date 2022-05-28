using System;
using UnityEngine;
using UnityEngine.UI;

public class OverviewItem : MonoBehaviour
{
	private ManagerProfile profile;

	[SerializeField]
	private Text title;

	[SerializeField]
	private Text duration;

	[SerializeField]
	private Text effective;

	[SerializeField]
	private Text experience;

	[SerializeField]
	private Text performance;

	[SerializeField]
	private Image skillImage;

	[SerializeField]
	private Image avatarImage;

	[SerializeField]
	private GameObject assignLayout;

	[SerializeField]
	private GameObject unassignLayout;

	public void Initialize(ManagerProfile profile, Location location, double totalExtraction, int level)
	{
		this.profile = profile;
		this.assignLayout.SetActive(profile.assign);
		this.unassignLayout.SetActive(!profile.assign);
		GameUtilities.String.ToText(this.title, Enum.GetName(typeof(Location), location) + " Lv. " + level.ToString());
		if (!profile.assign)
		{
			return;
		}
		this.avatarImage.sprite = Singleton<GameProcess>.Instance.GetManagerAvatarSprite(profile.experience);
		this.skillImage.sprite = Singleton<GameProcess>.Instance.GetManagerSkillSprite(profile.skill, profile.state == ManagerState.Ready);
		GameUtilities.String.ToText(this.experience, Enum.GetName(typeof(Experience), profile.experience));
		this.experience.gameObject.GetComponent<Outline>().effectColor = Singleton<GameProcess>.Instance.GetManagerBorderColor(profile.experience);
		this.experience.color = Singleton<GameProcess>.Instance.GetManagerTextColor(profile.experience);
		GameUtilities.String.ToText(this.performance, GameUtilities.Currencies.Convert(totalExtraction) + "/s");
		GameUtilities.String.ToText(this.duration, "Duration: " + GameUtilities.DateTime.Convert(Singleton<GameProcess>.Instance.GetManagerSkillDuration(profile.experience, profile.skill)));
		switch (profile.skill)
		{
		case ManagerSkill.UpgradeCost:
			GameUtilities.String.ToText(this.effective, "-" + Singleton<GameProcess>.Instance.GetManagerSkillEffective(profile.experience, profile.skill) + "% Upgrade Cost");
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
	}

	private void OnDisable()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void ManagerActivate()
	{
		if (!this.profile.assign || this.profile.state != ManagerState.Ready)
		{
			return;
		}
		this.skillImage.sprite = Singleton<GameProcess>.Instance.GetManagerSkillSprite(this.profile.skill, false);
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
		Tracking.instance.UI_Interaction("OverviewPopup", "ActiveManager");
	}
}

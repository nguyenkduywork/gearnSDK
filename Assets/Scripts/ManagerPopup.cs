using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPopup : Singleton<ManagerPopup>
{
	private sealed class _SellConfirm_c__AnonStorey0
	{
		internal ManagerProfile targetProfile;

		internal GameObject tag;

		internal ManagerPopup _this;

		internal void __m__0()
		{
			double num = Math.Round(this.targetProfile.price / 2.0);
			Singleton<GameManager>.Instance.SetCash(num);
			this._this.profile.Remove(this.targetProfile);
			this.tag.SetActive(false);
			Notification.instance.Warning("Manager sold for <color=#FBFF00FF>" + GameUtilities.Currencies.Convert(num) + "</color> coin");
		}

		internal void __m__1()
		{
			double num = Math.Round(this.targetProfile.price);
			Singleton<GameManager>.Instance.SetCash(num);
			this._this.profile.Remove(this.targetProfile);
			this.tag.SetActive(false);
			Notification.instance.Warning("Manager sold for <color=#FBFF00FF>" + GameUtilities.Currencies.Convert(num) + "</color> coin");
		}
	}

	private sealed class _HireManager_c__AnonStorey1
	{
		internal bool value;

		internal ManagerPopup _this;

		internal void __m__0()
		{
			this._this.HireManagerApply(this.value);
			Tracking.instance.Ads_Impress("reward", "HireManager");
		}
	}

	[SerializeField]
	private Sprite enableSprite;

	[SerializeField]
	private Sprite disableSprite;

	[SerializeField]
	private Text managerPriceText;

	[SerializeField]
	private Image hireManagerButton;

	[SerializeField]
	private GameObject targetPopup;

	[SerializeField]
	private GameObject managerItem;

	[SerializeField]
	private GameObject hireSpecialButton;

	[SerializeField]
	private RectTransform assignContent;

	[SerializeField]
	private RectTransform unassignContent;

	[SerializeField]
	private SellManagerPopup sellManager;

	[SerializeField]
	private GameObject tutorial_6;

	private int targetRestaurant;

	private KitchenController kitchen;

	private ElevatorController elevator;

	private RestaurantController restaurant;

	private List<ManagerProfile> profile;

	private List<GameObject> profileItem;

	private static Predicate<GameObject> __f__am_cache0;

	private void Start()
	{
		this.profileItem = new List<GameObject>();
		GameManager expr_10 = Singleton<GameManager>.Instance;
		expr_10.onCashChange = (Action<double>)Delegate.Combine(expr_10.onCashChange, new Action<double>(this.OnCashChange));
		this.targetRestaurant = Singleton<DataManager>.Instance.database.targetRestaurant;
		this.profile = Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].profile;
		this.tutorial_6.SetActive(!GameManager.IsDoneTutorial(6) && GameManager.IsDoneTutorial(5));
		this.hireSpecialButton.SetActive(GameManager.IsDoneTutorial(6));
	}

	public void Show(KitchenController kitchen)
	{
		this.kitchen = kitchen;
		if (kitchen.managerController.hasManager)
		{
			this.CreateProfileItem(this.assignContent.transform, kitchen.managerController.managerProfile);
		}
		for (int i = 0; i < this.profile.Count; i++)
		{
			if (this.profile[i].location == Location.Kitchen && !this.profile[i].assign)
			{
				this.CreateProfileItem(this.unassignContent.transform, this.profile[i]);
			}
		}
		double managerPrice = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].kitchenRecruitment, Location.Kitchen);
		this.hireManagerButton.sprite = ((Singleton<GameManager>.Instance.database.cash < managerPrice) ? this.disableSprite : this.enableSprite);
		GameUtilities.String.ToText(this.managerPriceText, GameUtilities.Currencies.Convert(managerPrice));
		Singleton<SoundManager>.Instance.Play("Popup");
		this.targetPopup.SetActive(true);
		this.tutorial_6.SetActive(!GameManager.IsDoneTutorial(6) && GameManager.IsDoneTutorial(5));
		if (!GameManager.IsDoneTutorial(6) && GameManager.IsDoneTutorial(5))
		{
			Tracking.instance.Tutorial_Start("Step6");
		}
	}

	public void Show(ElevatorController elevator)
	{
		this.elevator = elevator;
		if (elevator.managerController.hasManager)
		{
			this.CreateProfileItem(this.assignContent.transform, elevator.managerController.managerProfile);
		}
		for (int i = 0; i < this.profile.Count; i++)
		{
			if (this.profile[i].location == Location.Elevator && !this.profile[i].assign)
			{
				this.CreateProfileItem(this.unassignContent.transform, this.profile[i]);
			}
		}
		double managerPrice = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].elevatorRecruitment, Location.Elevator);
		this.hireManagerButton.sprite = ((Singleton<GameManager>.Instance.database.cash < managerPrice) ? this.disableSprite : this.enableSprite);
		GameUtilities.String.ToText(this.managerPriceText, GameUtilities.Currencies.Convert(managerPrice));
		Singleton<SoundManager>.Instance.Play("Popup");
		this.targetPopup.SetActive(true);
	}

	public void Show(RestaurantController restaurant)
	{
		this.restaurant = restaurant;
		if (restaurant.managerController.hasManager)
		{
			this.CreateProfileItem(this.assignContent.transform, restaurant.managerController.managerProfile);
		}
		for (int i = 0; i < this.profile.Count; i++)
		{
			if (this.profile[i].location == Location.Restaurant && !this.profile[i].assign)
			{
				this.CreateProfileItem(this.unassignContent.transform, this.profile[i]);
			}
		}
		double managerPrice = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].restaurantRecruitment, Location.Restaurant);
		this.hireManagerButton.sprite = ((Singleton<GameManager>.Instance.database.cash < managerPrice) ? this.disableSprite : this.enableSprite);
		GameUtilities.String.ToText(this.managerPriceText, GameUtilities.Currencies.Convert(managerPrice));
		Singleton<SoundManager>.Instance.Play("Popup");
		this.targetPopup.SetActive(true);
	}

	public void Close()
	{
		this.kitchen = null;
		this.elevator = null;
		this.restaurant = null;
		for (int i = 0; i < this.profileItem.Count; i++)
		{
			this.profileItem[i].SetActive(false);
		}
		this.targetPopup.SetActive(false);
		Tracking.instance.UI_Interaction("ManagerPopup", "ClosePopup");
	}

	public void Assign(ManagerProfile targetProfile)
	{
		if (this.kitchen != null)
		{
			if (!this.kitchen.managerController.hasManager)
			{
				targetProfile.kitchenFloor = this.kitchen.kitchenData.floor;
				this.CreateProfileItem(this.assignContent.transform, targetProfile);
				this.kitchen.managerController.managerProfile = targetProfile;
				this.kitchen.managerController.ManagerAssign();
			}
			else
			{
				targetProfile.kitchenFloor = this.kitchen.kitchenData.floor;
				this.assignContent.GetChild(0).GetComponent<ManagerItem>().Unassign();
				this.CreateProfileItem(this.assignContent.transform, targetProfile);
				this.kitchen.managerController.managerProfile = targetProfile;
				this.kitchen.managerController.ManagerAssign();
			}
		}
		if (this.elevator != null)
		{
			if (!this.elevator.managerController.hasManager)
			{
				this.CreateProfileItem(this.assignContent.transform, targetProfile);
				this.elevator.managerController.managerProfile = targetProfile;
				this.elevator.managerController.ManagerAssign();
			}
			else
			{
				this.assignContent.GetChild(0).GetComponent<ManagerItem>().Unassign();
				this.CreateProfileItem(this.assignContent.transform, targetProfile);
				this.elevator.managerController.managerProfile = targetProfile;
				this.elevator.managerController.ManagerAssign();
			}
		}
		if (this.restaurant != null)
		{
			if (!this.restaurant.managerController.hasManager)
			{
				this.CreateProfileItem(this.assignContent.transform, targetProfile);
				this.restaurant.managerController.managerProfile = targetProfile;
				this.restaurant.managerController.ManagerAssign();
			}
			else
			{
				this.assignContent.GetChild(0).GetComponent<ManagerItem>().Unassign();
				this.CreateProfileItem(this.assignContent.transform, targetProfile);
				this.restaurant.managerController.managerProfile = targetProfile;
				this.restaurant.managerController.ManagerAssign();
			}
		}
		Singleton<GameManager>.Instance.IdleCashCompute();
	}

	public void Unassign(ManagerProfile targetProfile)
	{
		if (this.kitchen != null)
		{
			this.kitchen.managerController.ManagerUnassign();
		}
		if (this.elevator != null)
		{
			this.elevator.managerController.ManagerUnassign();
		}
		if (this.restaurant != null)
		{
			this.restaurant.managerController.ManagerUnassign();
		}
		this.CreateProfileItem(this.unassignContent.transform, targetProfile);
		Singleton<GameManager>.Instance.IdleCashCompute();
	}

	public void SellConfirm(ManagerProfile targetProfile, GameObject tag)
	{
		this.sellManager.Show(targetProfile, delegate
		{
			double num = Math.Round(targetProfile.price / 2.0);
			Singleton<GameManager>.Instance.SetCash(num);
			this.profile.Remove(targetProfile);
			tag.SetActive(false);
			Notification.instance.Warning("Manager sold for <color=#FBFF00FF>" + GameUtilities.Currencies.Convert(num) + "</color> coin");
		}, delegate
		{
			double num = Math.Round(targetProfile.price);
			Singleton<GameManager>.Instance.SetCash(num);
			this.profile.Remove(targetProfile);
			tag.SetActive(false);
			Notification.instance.Warning("Manager sold for <color=#FBFF00FF>" + GameUtilities.Currencies.Convert(num) + "</color> coin");
		});
	}

	public void HireManager(bool value)
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
				this.HireManagerApply(value);
				Tracking.instance.Ads_Impress("reward", "HireManager");
			});
		}
		else
		{
			this.HireManagerApply(value);
			if (!GameManager.IsDoneTutorial(6) && GameManager.IsDoneTutorial(5))
			{
				GameManager.TutorialDone(6);
				this.hireSpecialButton.SetActive(true);
				this.tutorial_6.SetActive(false);
				Tracking.instance.Tutorial_Done("Step6");
			}
		}
		Tracking.instance.UI_Interaction("ManagerPopup", "HireManager");
	}

	private void OnCashChange(double cash)
	{
		double num = 0.0;
		if (this.kitchen != null)
		{
			num = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].kitchenRecruitment, Location.Kitchen);
		}
		if (this.elevator != null)
		{
			num = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].elevatorRecruitment, Location.Elevator);
		}
		if (this.restaurant != null)
		{
			num = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].restaurantRecruitment, Location.Restaurant);
		}
		GameUtilities.String.ToText(this.managerPriceText, GameUtilities.Currencies.Convert(num));
		this.hireManagerButton.sprite = ((cash < num) ? this.disableSprite : this.enableSprite);
	}

	private void HireManagerApply(bool noJunior)
	{
		double num = 0.0;
		if (this.elevator != null)
		{
			if (!noJunior)
			{
				num = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].elevatorRecruitment, Location.Elevator);
				if (Singleton<GameManager>.Instance.database.cash < num)
				{
					return;
				}
			}
			ManagerProfile managerProfile = Singleton<GameProcess>.Instance.GetManagerProfile(Location.Elevator, num, noJunior);
			Singleton<GameManager>.Instance.database.restaurant[this.targetRestaurant].elevatorRecruitment++;
			if (!this.elevator.managerController.hasManager)
			{
				managerProfile.assign = true;
				this.elevator.managerController.managerProfile = managerProfile;
				this.elevator.managerController.ManagerAssign();
				this.CreateProfileItem(this.assignContent.transform, managerProfile);
			}
			else
			{
				this.CreateProfileItem(this.unassignContent.transform, managerProfile);
			}
			this.profile.Add(managerProfile);
		}
		if (this.restaurant != null)
		{
			if (!noJunior)
			{
				num = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].restaurantRecruitment, Location.Restaurant);
				if (Singleton<GameManager>.Instance.database.cash < num)
				{
					return;
				}
			}
			ManagerProfile managerProfile2 = Singleton<GameProcess>.Instance.GetManagerProfile(Location.Restaurant, num, noJunior);
			Singleton<GameManager>.Instance.database.restaurant[this.targetRestaurant].restaurantRecruitment++;
			if (!this.restaurant.managerController.hasManager)
			{
				managerProfile2.assign = true;
				this.restaurant.managerController.managerProfile = managerProfile2;
				this.restaurant.managerController.ManagerAssign();
				this.CreateProfileItem(this.assignContent.transform, managerProfile2);
			}
			else
			{
				this.CreateProfileItem(this.unassignContent.transform, managerProfile2);
			}
			this.profile.Add(managerProfile2);
		}
		if (this.kitchen != null)
		{
			if (!noJunior)
			{
				num = Singleton<GameProcess>.Instance.GetManagerPrice(Singleton<DataManager>.Instance.database.restaurant[this.targetRestaurant].kitchenRecruitment, Location.Kitchen);
				if (Singleton<GameManager>.Instance.database.cash < num)
				{
					return;
				}
			}
			ManagerProfile managerProfile3 = Singleton<GameProcess>.Instance.GetManagerProfile(Location.Kitchen, num, noJunior);
			Singleton<GameManager>.Instance.database.restaurant[this.targetRestaurant].kitchenRecruitment++;
			if (!this.kitchen.managerController.hasManager)
			{
				managerProfile3.assign = true;
				managerProfile3.kitchenFloor = this.kitchen.kitchenData.floor;
				this.kitchen.managerController.managerProfile = managerProfile3;
				this.kitchen.managerController.ManagerAssign();
				this.CreateProfileItem(this.assignContent.transform, managerProfile3);
			}
			else
			{
				this.CreateProfileItem(this.unassignContent.transform, managerProfile3);
			}
			this.profile.Add(managerProfile3);
		}
		if (num != 0.0)
		{
			Singleton<GameManager>.Instance.SetCash(-num);
		}
		Singleton<SoundManager>.Instance.Play("Paper");
		Singleton<GameManager>.Instance.IdleCashCompute();
	}

	private void CreateProfileItem(Transform parent, ManagerProfile profile)
	{
		GameObject gameObject = this.profileItem.Find((GameObject target) => !target.activeSelf);
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			gameObject.GetComponent<RectTransform>().SetParent(parent);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.managerItem, parent);
			this.profileItem.Add(gameObject);
		}
		gameObject.transform.SetAsFirstSibling();
		gameObject.GetComponent<ManagerItem>().Initialize(profile);
		this.unassignContent.anchoredPosition = new Vector2(0f, 0f);
	}
}

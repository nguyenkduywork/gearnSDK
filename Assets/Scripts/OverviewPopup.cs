using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewPopup : Singleton<OverviewPopup>
{
	private List<KitchenController> kitchen;

	private List<OverviewItem> overviewItem;

	[SerializeField]
	private Sprite enableSprite;

	[SerializeField]
	private Sprite disableSprite;

	[SerializeField]
	private Image activateButton;

	[SerializeField]
	private Text totalExtractionText;

	[SerializeField]
	private Transform overviewContent;

	[SerializeField]
	private GameObject overviewPopup;

	[SerializeField]
	private GameObject overviewItemPrefab;

	[SerializeField]
	private ElevatorController elevator;

	[SerializeField]
	private RestaurantController restaurant;

	private void Start()
	{
		this.overviewItem = new List<OverviewItem>();
	}

	public void Show()
	{
		int num = 0;
		double num2 = 0.0;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.overviewItemPrefab, this.overviewContent);
		this.overviewItem.Add(gameObject.GetComponent<OverviewItem>());
		this.overviewItem[this.overviewItem.Count - 1].Initialize(this.elevator.managerController.managerProfile, Location.Elevator, this.elevator.elevatorProperties.transportation, this.elevator.elevatorData.level);
		if (this.elevator.managerController.hasManager && this.elevator.managerController.managerProfile.state == ManagerState.Ready)
		{
			num++;
		}
		gameObject = UnityEngine.Object.Instantiate<GameObject>(this.overviewItemPrefab, this.overviewContent);
		this.overviewItem.Add(gameObject.GetComponent<OverviewItem>());
		this.overviewItem[this.overviewItem.Count - 1].Initialize(this.restaurant.managerController.managerProfile, Location.Restaurant, this.restaurant.restaurantProperties.transportation, this.restaurant.restaurantData.level);
		if (this.restaurant.managerController.hasManager && this.restaurant.managerController.managerProfile.state == ManagerState.Ready)
		{
			num++;
		}
		this.kitchen = Singleton<GameManager>.Instance.kitchenController;
		for (int i = 0; i < this.kitchen.Count; i++)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.overviewItemPrefab, this.overviewContent);
			this.overviewItem.Add(gameObject.GetComponent<OverviewItem>());
			this.overviewItem[this.overviewItem.Count - 1].Initialize(this.kitchen[i].managerController.managerProfile, Location.Kitchen, this.kitchen[i].kitchenProperties.totalExtraction, this.kitchen[i].kitchenData.level);
			if (this.kitchen[i].managerController.hasManager && this.kitchen[i].managerController.managerProfile.state == ManagerState.Ready)
			{
				num++;
			}
			num2 += this.kitchen[i].kitchenProperties.totalExtraction;
		}
		GameUtilities.String.ToText(this.totalExtractionText, GameUtilities.Currencies.Convert(num2) + "/s");
		this.activateButton.sprite = ((num <= 0) ? this.disableSprite : this.enableSprite);
		Singleton<SoundManager>.Instance.Play("Popup");
		this.overviewPopup.SetActive(true);
	}

	public void Close()
	{
		this.overviewItem.Clear();
		this.overviewPopup.SetActive(false);
		Tracking.instance.UI_Interaction("OverviewPopup", "ClosePopup");
	}

	public void ActiveAllManager()
	{
		this.activateButton.sprite = this.disableSprite;
		for (int i = 0; i < this.overviewItem.Count; i++)
		{
			this.overviewItem[i].ManagerActivate();
		}
		Tracking.instance.UI_Interaction("OverviewPopup", "ActiveAllManager");
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private Text idleCashText;

	[SerializeField]
	private Text mainScreenCash;

	[SerializeField]
	private Text shopScreenCash;

	[SerializeField]
	private Text mainScreenDiamond;

	[SerializeField]
	private Text shopScreenDiamond;

	[SerializeField]
	private GameObject kitchenPrefab;

	[SerializeField]
	private Transform kitchenContent;

	[SerializeField]
	private BarrierController barrier;

	[SerializeField]
	private BoostManager boostManager;

	[SerializeField]
	private Configuration configuration;

	private const float floorDistance = 2f;

	public Database database;

	[HideInInspector]
	public List<KitchenController> kitchenController;

	public ElevatorController elevator;

	public RestaurantController restaurant;

	public Action<double> onCashChange;

	public Action<double> onIdleCashChange;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
	{
		this.database = Singleton<DataManager>.Instance.database;
		GameUtilities.String.ToText(this.mainScreenDiamond, this.database.diamond.ToString());
		GameUtilities.String.ToText(this.shopScreenDiamond, this.database.diamond.ToString());
		GameUtilities.String.ToText(this.mainScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
		GameUtilities.String.ToText(this.shopScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
		GameUtilities.String.ToText(this.idleCashText, GameUtilities.Currencies.Convert(this.database.restaurant[this.database.targetRestaurant].idleCash) + "/s");
		this.InitKitchen();
		this.InitElevator();
		this.InitRestaurant();
		this.InitManager();
		this.barrier.Initialize();
		this.boostManager.Initialize();
		this.onCashChange(this.database.cash);
        //Singleton<GameManager>.Instance.SetDiamond(10000000);
    }

	private void InitKitchen()
	{
		this.kitchenController = new List<KitchenController>();
		for (int i = 0; i < this.database.restaurant[this.database.targetRestaurant].kitchen.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.kitchenPrefab, this.kitchenContent);
			gameObject.transform.SetSiblingIndex(1);
			KitchenController component = gameObject.GetComponent<KitchenController>();
			component.kitchenData = this.database.restaurant[this.database.targetRestaurant].kitchen[i];
			component.Initialize();
			this.kitchenController.Add(component);
		}
	}

	private void InitElevator()
	{
		this.elevator.elevatorData = this.database.restaurant[this.database.targetRestaurant].elevator;
		this.elevator.kitchenController = this.kitchenController;
		this.elevator.Initialize();
	}

	private void InitRestaurant()
	{
		this.restaurant.restaurantData = this.database.restaurant[this.database.targetRestaurant].restaurant;
		this.restaurant.Initialize();
	}

	private void InitManager()
	{
		for (int i = 0; i < this.database.restaurant[this.database.targetRestaurant].profile.Count; i++)
		{
			if (this.database.restaurant[this.database.targetRestaurant].profile[i].assign)
			{
				Location location = this.database.restaurant[this.database.targetRestaurant].profile[i].location;
				if (location != Location.Elevator)
				{
					if (location != Location.Restaurant)
					{
						if (location == Location.Kitchen)
						{
							int kitchenFloor = this.database.restaurant[this.database.targetRestaurant].profile[i].kitchenFloor;
							this.kitchenController[kitchenFloor].managerController.managerProfile = this.database.restaurant[this.database.targetRestaurant].profile[i];
							this.kitchenController[kitchenFloor].managerController.ManagerAssign();
						}
					}
					else
					{
						this.restaurant.managerController.managerProfile = this.database.restaurant[this.database.targetRestaurant].profile[i];
						this.restaurant.managerController.ManagerAssign();
					}
				}
				else
				{
					this.elevator.managerController.managerProfile = this.database.restaurant[this.database.targetRestaurant].profile[i];
					this.elevator.managerController.ManagerAssign();
				}
			}
		}
	}

	public void IdleCashCompute()
	{
		if (this.elevator.managerController.hasManager && this.restaurant.managerController.hasManager)
		{
			double num = 0.0;
			for (int i = 0; i < this.kitchenController.Count; i++)
			{
				if (this.kitchenController[i].managerController.hasManager)
				{
					num += this.kitchenController[i].kitchenProperties.totalExtraction;
				}
			}
			double num2 = num;
			if (num2 > this.elevator.elevatorProperties.transportation)
			{
				num2 = this.elevator.elevatorProperties.transportation;
			}
			if (num2 > this.restaurant.restaurantProperties.transportation)
			{
				num2 = this.restaurant.restaurantProperties.transportation;
			}
			this.database.restaurant[this.database.targetRestaurant].idleCash = Math.Round(num2 / 100.0 * (double)this.configuration.general.idleCashRate);
		}
		else
		{
			this.database.restaurant[this.database.targetRestaurant].idleCash = 0.0;
		}
		this.onIdleCashChange(this.database.restaurant[this.database.targetRestaurant].idleCash);
		GameUtilities.String.ToText(this.idleCashText, GameUtilities.Currencies.Convert(this.database.restaurant[this.database.targetRestaurant].idleCash) + "/s");
	}

	public void UnlockKitchen()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.kitchenPrefab, this.kitchenContent);
		gameObject.transform.SetSiblingIndex(1);
		KitchenController component = gameObject.GetComponent<KitchenController>();
		KitchenData kitchenData = new KitchenData();
		kitchenData.floor = this.kitchenController.Count;
		kitchenData.level = 1;
		component.kitchenData = kitchenData;
		component.Initialize();
		this.database.restaurant[this.database.targetRestaurant].kitchen.Add(kitchenData);
		this.kitchenController.Add(component);
		this.barrier.Refresh();
		if (this.kitchenController.Count == 1 && this.elevator.managerController.hasManager)
		{
			this.elevator.StartTransport();
		}
	}

	public void SetCash(double cash)
	{
		this.database.cash += cash;
		GameUtilities.String.ToText(this.mainScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
		GameUtilities.String.ToText(this.shopScreenCash, GameUtilities.Currencies.Convert(this.database.cash));
		this.onCashChange(this.database.cash);
	}

	public void SetDiamond(int diamond)
	{
		this.database.diamond += diamond;
		GameUtilities.String.ToText(this.mainScreenDiamond, this.database.diamond.ToString());
		GameUtilities.String.ToText(this.shopScreenDiamond, this.database.diamond.ToString());
	}

	public static bool IsDoneTutorial(int index)
	{
		return Singleton<DataManager>.Instance.database.tutorialCompleted.Contains(index);
	}

	public static void TutorialDone(int index)
	{
		Singleton<DataManager>.Instance.database.tutorialCompleted.Add(index);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantController : MonoBehaviour
{
	public Text levelText;

	public GameObject[] levelUp;

	public GameObject waiterPrefab;

	public GameObject jumpCashPrefab;

	public Transform gatheringPoint;

	public Transform exploitedPoint;

	public Transform restaurantGroup;

	public Transform jumpCashPosition;

	public GameManager gameManager;

	public BoostManager boostManager;

	public BoostController boostController;

	public ManagerController managerController;

	public ElevatorController elevatorController;

	public RestaurantPopup restaurantPopup;

	public GameObject tutorial_4;

	[HideInInspector]
	public RestaurantData restaurantData;

	[HideInInspector]
	public RestaurantProperties restaurantProperties;

	private List<WaiterController> waiterController;

	public void Initialize()
	{
		this.waiterController = new List<WaiterController>();
		this.managerController.managerAssign = new Action(this.StartTransport);
		GameManager expr_27 = Singleton<GameManager>.Instance;
		expr_27.onCashChange = (Action<double>)Delegate.Combine(expr_27.onCashChange, new Action<double>(this.OnCashChange));
		float distance = Vector3.Distance(this.gatheringPoint.position, this.exploitedPoint.position);
		this.restaurantProperties = Singleton<GameProcess>.Instance.GetRestaurantProperties(distance, this.restaurantData.level);
		GameUtilities.String.ToText(this.levelText, "Level \n" + this.restaurantData.level.ToString());
		this.boostController.Refresh();
		this.SetWaiter(this.restaurantProperties.waiter);
	}

	public void ShowManagerProfile()
	{
		Singleton<ManagerPopup>.Instance.Show(this);
	}

	public void ShowRestaurantProperties()
	{
		this.restaurantPopup.Show();
	}

	public void SetCash(double cash)
	{
		this.gameManager.SetCash(cash * (double)this.boostManager.totalEffective);
		if (cash <= 0.0)
		{
			return;
		}
		GameObject gameObject = ObjectPool.Spawn(this.jumpCashPrefab, this.jumpCashPosition.position, Quaternion.identity);
		gameObject.transform.SetParent(this.jumpCashPosition);
		gameObject.transform.localScale = Vector3.one;
		gameObject.GetComponent<JumpCash>().Init(cash * (double)this.boostManager.totalEffective);
		if (!GameManager.IsDoneTutorial(5) && cash > 0.0)
		{
			Singleton<GameManager>.Instance.kitchenController[0].Tutorial_5();
			Tracking.instance.Tutorial_Start("Step5");
		}
	}

	public void Upgrade()
	{
		float distance = Vector3.Distance(this.gatheringPoint.position, this.exploitedPoint.position);
		this.restaurantProperties = Singleton<GameProcess>.Instance.GetRestaurantProperties(distance, this.restaurantData.level);
		if (this.restaurantProperties.waiter > this.waiterController.Count)
		{
			this.SetWaiter(this.restaurantProperties.waiter);
		}
		GameUtilities.String.ToText(this.levelText, "Level \n" + this.restaurantData.level.ToString());
	}

	public void StartTransport()
	{
		for (int i = 0; i < this.waiterController.Count; i++)
		{
			if (this.waiterController[i].isIdle)
			{
				this.waiterController[i].StartTransport();
			}
		}
		if (!GameManager.IsDoneTutorial(4))
		{
			GameManager.TutorialDone(4);
			this.tutorial_4.SetActive(false);
			Tracking.instance.Tutorial_Done("Step4");
		}
	}

	private void SetWaiter(int count)
	{
		for (int i = this.waiterController.Count; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.waiterPrefab, this.restaurantGroup);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.position = this.gatheringPoint.position + Vector3.left * 0.3f * (float)i;
			this.waiterController.Add(gameObject.GetComponent<WaiterController>());
			this.waiterController[i].Initialize(this.elevatorController, this);
		}
		if (this.managerController.hasManager)
		{
			this.StartTransport();
		}
	}

	private void OnCashChange(double cash)
	{
		int maxUpgradeLevel = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(cash, this.boostController.upgradeCostReduced, this.restaurantData.level, Location.Restaurant, 0);
		this.levelUp[0].SetActive(maxUpgradeLevel > 0);
		this.levelUp[1].SetActive(maxUpgradeLevel > 9);
		this.levelUp[2].SetActive(maxUpgradeLevel >= 50);
	}
}

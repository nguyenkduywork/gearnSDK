using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenController : MonoBehaviour
{
	public Text cashText;

	public Text levelText;

	public Text floorText;

	public GameObject[] levelUp;

	public GameObject transporterPrefab;

	public Transform kitchenGroup;

	public Transform exploitedPoint;

	public Transform gatheringPoint;

	public BoostController boostController;

	public ManagerController managerController;

	public GameObject tutorial_2;

	public GameObject tutorial_5;

	[NonSerialized]
	public KitchenData kitchenData;

	[NonSerialized]
	public KitchenProperties kitchenProperties;

	private float distance;

	private List<TransporterController> transporterController;

	public void Initialize()
	{
		this.managerController.managerAssign = new Action(this.StartWorking);
		GameManager expr_1C = Singleton<GameManager>.Instance;
		expr_1C.onCashChange = (Action<double>)Delegate.Combine(expr_1C.onCashChange, new Action<double>(this.OnCashChange));
		this.transporterController = new List<TransporterController>();
		this.distance = Vector3.Distance(this.gatheringPoint.position, this.exploitedPoint.position);
		this.kitchenProperties = Singleton<GameProcess>.Instance.GetKitchenProperties(this.distance, this.kitchenData.floor, this.kitchenData.level, 1f);
		GameUtilities.String.ToText(this.floorText, (this.kitchenData.floor + 1).ToString());
		GameUtilities.String.ToText(this.cashText, GameUtilities.Currencies.Convert(this.kitchenData.cash));
		GameUtilities.String.ToText(this.levelText, "Level \n" + this.kitchenData.level.ToString());
		this.boostController.Refresh();
		this.SetTransporter(this.kitchenProperties.transporter);
		this.tutorial_2.SetActive(!GameManager.IsDoneTutorial(2));
		this.tutorial_5.SetActive(!GameManager.IsDoneTutorial(5) && GameManager.IsDoneTutorial(4));
		if (!GameManager.IsDoneTutorial(2))
		{
			Tracking.instance.Tutorial_Start("Step2");
		}
	}

	public void ShowManagerProfile()
	{
		if (!GameManager.IsDoneTutorial(5) && GameManager.IsDoneTutorial(4))
		{
			GameManager.TutorialDone(5);
			this.tutorial_5.SetActive(false);
			Tracking.instance.Tutorial_Done("Step5");
		}
		Singleton<ManagerPopup>.Instance.Show(this);
	}

	public void ShowKitchenProperties()
	{
		Singleton<KitchenPopup>.Instance.Show(this);
	}

	public void SetCash(double cash)
	{
		this.kitchenData.cash += cash;
		GameUtilities.String.ToText(this.cashText, GameUtilities.Currencies.Convert(this.kitchenData.cash));
		if (!Singleton<DataManager>.Instance.database.tutorialCompleted.Contains(3))
		{
			Singleton<GameManager>.Instance.elevator.Tutorial_3();
		}
	}

	public void StartWorking()
	{
		for (int i = 0; i < this.transporterController.Count; i++)
		{
			if (this.transporterController[i].isIdle)
			{
				this.transporterController[i].StartWorking();
			}
		}
		if (!GameManager.IsDoneTutorial(2))
		{
			GameManager.TutorialDone(2);
			this.tutorial_2.SetActive(false);
			Tracking.instance.Tutorial_Done("Step2");
		}
	}

	public void Upgrade()
	{
		this.kitchenProperties = Singleton<GameProcess>.Instance.GetKitchenProperties(this.distance, this.kitchenData.floor, this.kitchenData.level, 1f);
		if (this.kitchenProperties.transporter > this.transporterController.Count)
		{
			this.SetTransporter(this.kitchenProperties.transporter);
		}
		GameUtilities.String.ToText(this.levelText, "Level \n" + this.kitchenData.level.ToString());
	}

	public void Tutorial_5()
	{
		this.tutorial_5.SetActive(true);
		Tracking.instance.Tutorial_Start("Step5");
	}

	private void SetTransporter(int count)
	{
		for (int i = this.transporterController.Count; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.transporterPrefab, this.kitchenGroup);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.position = this.gatheringPoint.position + Vector3.right * 0.2f * (float)i;
			this.transporterController.Add(gameObject.GetComponent<TransporterController>());
			this.transporterController[i].Initialize(this);
		}
		if (this.managerController.hasManager)
		{
			this.StartWorking();
		}
	}

	private void OnCashChange(double cash)
	{
		int maxUpgradeLevel = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(cash, this.boostController.upgradeCostReduced, this.kitchenData.level, Location.Kitchen, this.kitchenData.floor);
		this.levelUp[0].SetActive(maxUpgradeLevel > 0);
		this.levelUp[1].SetActive(maxUpgradeLevel > 9);
		this.levelUp[2].SetActive(maxUpgradeLevel >= 50);
	}
}

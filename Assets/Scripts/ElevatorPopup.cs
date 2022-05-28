using System;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorPopup : MonoBehaviour
{
	[SerializeField]
	private GameObject popup;

	[SerializeField]
	private Text title;

	[SerializeField]
	private Text currentLevel;

	[SerializeField]
	private Text nextLevel;

	[SerializeField]
	private Text currentTotal;

	[SerializeField]
	private Text nextTotal;

	[SerializeField]
	private Text currentLoad;

	[SerializeField]
	private Text nextLoad;

	[SerializeField]
	private Text currentLoading;

	[SerializeField]
	private Text nextLoading;

	[SerializeField]
	private Text currentSpeed;

	[SerializeField]
	private Text nextSpeed;

	[SerializeField]
	private Image currentLevelFill;

	[SerializeField]
	private Image nextLevelFill;

	[SerializeField]
	private Text bonusDiamond;

	[SerializeField]
	private Text levelNumber;

	[SerializeField]
	private Text upgradePrice;

	[SerializeField]
	private Image upgradeButton;

	[SerializeField]
	private Sprite enableSprite;

	[SerializeField]
	private Sprite disableSprite;

	[SerializeField]
	private Transform handle;

	[SerializeField]
	private Transform[] selection;

	[SerializeField]
	private ElevatorController elevatorController;

	private int upgradeStep = 1;

	private void Start()
	{
		GameManager expr_05 = Singleton<GameManager>.Instance;
		expr_05.onCashChange = (Action<double>)Delegate.Combine(expr_05.onCashChange, new Action<double>(this.OnCashChange));
	}

	public void SelectUpgradeStep(int value)
	{
		int num = 0;
		this.upgradeStep = value;
		int num2 = this.upgradeStep;
		if (num2 != 0)
		{
			if (num2 != 1)
			{
				if (num2 != 10)
				{
					if (num2 == 50)
					{
						num = 2;
					}
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 3;
		}
		this.handle.transform.localPosition = this.selection[num].localPosition;
		if (!this.IsMaxLevel())
		{
			this.Show();
		}
		Tracking.instance.UI_Interaction("ElevatorPopup", "SelectUpgradeStep");
	}

	public void Show()
	{
		GameUtilities.String.ToText(this.title, "Elevator Lv." + this.elevatorController.elevatorData.level.ToString());
		int lastBonusAtLevel = Singleton<GameProcess>.Instance.GetLastBonusAtLevel(this.elevatorController.elevatorData.level, Location.Elevator);
		int nextBonusAtLevel = Singleton<GameProcess>.Instance.GetNextBonusAtLevel(this.elevatorController.elevatorData.level, Location.Elevator);
		GameUtilities.String.ToText(this.currentLevel, "Level " + this.elevatorController.elevatorData.level.ToString());
		GameUtilities.String.ToText(this.nextLevel, (nextBonusAtLevel != 2147483647) ? ("Next boost at level " + nextBonusAtLevel.ToString()) : "Boost maximum.");
		this.currentLevelFill.fillAmount = ((nextBonusAtLevel != 2147483647) ? ((float)(this.elevatorController.elevatorData.level - lastBonusAtLevel) / (float)(nextBonusAtLevel - lastBonusAtLevel)) : 1f);
		ElevatorProperties elevatorProperties = this.elevatorController.elevatorProperties;
		GameUtilities.String.ToText(this.currentLoad, GameUtilities.Currencies.Convert(elevatorProperties.load));
		GameUtilities.String.ToText(this.currentSpeed, Math.Round((double)elevatorProperties.movementSpeed, 2).ToString());
		GameUtilities.String.ToText(this.currentLoading, GameUtilities.Currencies.Convert(elevatorProperties.loadingSpeed) + "/s");
		GameUtilities.String.ToText(this.currentTotal, GameUtilities.Currencies.Convert(elevatorProperties.transportation) + "/s");
		if (this.IsMaxLevel())
		{
			GameUtilities.String.ToText(this.nextLoad, "Max");
			GameUtilities.String.ToText(this.nextTotal, "Max");
			GameUtilities.String.ToText(this.nextSpeed, "Max");
			GameUtilities.String.ToText(this.nextLoading, "Max");
			GameUtilities.String.ToText(this.levelNumber, "Max");
			GameUtilities.String.ToText(this.upgradePrice, "0");
			this.upgradeButton.sprite = this.disableSprite;
		}
		else
		{
			int num;
			if (this.upgradeStep != 0)
			{
				num = this.upgradeStep;
			}
			else
			{
				num = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(Singleton<GameManager>.Instance.database.cash, this.elevatorController.boostController.upgradeCostReduced, this.elevatorController.elevatorData.level, Location.Elevator, 0);
			}
			num = Mathf.Clamp(num, 1, Singleton<GameProcess>.Instance.GetMaxLevel(Location.Elevator) - 1);
			float distance = (float)this.elevatorController.kitchenController.Count * 2f;
			ElevatorProperties elevatorProperties2 = Singleton<GameProcess>.Instance.GetElevatorProperties(distance, this.elevatorController.elevatorData.level + num);
			GameUtilities.String.ToText(this.nextLoad, "+" + GameUtilities.Currencies.Convert(elevatorProperties2.load - elevatorProperties.load));
			GameUtilities.String.ToText(this.nextSpeed, "+" + Math.Round((double)(elevatorProperties2.movementSpeed - elevatorProperties.movementSpeed), 2).ToString());
			GameUtilities.String.ToText(this.nextLoading, "+" + GameUtilities.Currencies.Convert(elevatorProperties2.loadingSpeed - elevatorProperties.loadingSpeed) + "/s");
			GameUtilities.String.ToText(this.nextTotal, "+" + GameUtilities.Currencies.Convert(elevatorProperties2.transportation - elevatorProperties.transportation) + "/s");
			double num2 = Singleton<GameProcess>.Instance.GetUpgradePrice(this.elevatorController.elevatorData.level, num, this.elevatorController.boostController.upgradeCostReduced, Location.Elevator, 0);
			GameUtilities.String.ToText(this.levelNumber, "Level Up x" + num.ToString());
			GameUtilities.String.ToText(this.upgradePrice, GameUtilities.Currencies.Convert(num2));
			this.upgradeButton.sprite = ((Singleton<GameManager>.Instance.database.cash < num2 || this.elevatorController.elevatorData.level + num > Singleton<GameProcess>.Instance.GetMaxLevel(Location.Elevator)) ? this.disableSprite : this.enableSprite);
			this.nextLevelFill.fillAmount = ((nextBonusAtLevel != 2147483647) ? ((float)(this.elevatorController.elevatorData.level + num - lastBonusAtLevel) / (float)(nextBonusAtLevel - lastBonusAtLevel)) : 1f);
			int diamondBonus = Singleton<GameProcess>.Instance.GetDiamondBonus(this.elevatorController.elevatorData.level + num, this.elevatorController.elevatorData.level, Location.Elevator);
			GameUtilities.String.ToText(this.bonusDiamond, "+" + diamondBonus.ToString());
			this.bonusDiamond.gameObject.SetActive(diamondBonus > 0);
		}
		if (!this.popup.activeInHierarchy)
		{
			Singleton<SoundManager>.Instance.Play("Popup");
			this.popup.SetActive(true);
		}
	}

	public void Close()
	{
		this.popup.SetActive(false);
		Tracking.instance.UI_Interaction("ElevatorPopup", "ClosePopup");
	}

	public void Upgrade()
	{
		if (this.IsMaxLevel())
		{
			return;
		}
		int num;
		if (this.upgradeStep != 0)
		{
			num = this.upgradeStep;
		}
		else
		{
			num = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(Singleton<GameManager>.Instance.database.cash, this.elevatorController.boostController.upgradeCostReduced, this.elevatorController.elevatorData.level, Location.Elevator, 0);
		}
		num = Mathf.Clamp(num, 1, Singleton<GameProcess>.Instance.GetMaxLevel(Location.Elevator) - 1);
		double num2 = Singleton<GameProcess>.Instance.GetUpgradePrice(this.elevatorController.elevatorData.level, num, this.elevatorController.boostController.upgradeCostReduced, Location.Elevator, 0);
		if (Singleton<GameManager>.Instance.database.cash < num2)
		{
			return;
		}
		int diamondBonus = Singleton<GameProcess>.Instance.GetDiamondBonus(this.elevatorController.elevatorData.level + num, this.elevatorController.elevatorData.level, Location.Elevator);
		Singleton<GameManager>.Instance.SetDiamond(diamondBonus);
		this.elevatorController.elevatorData.level += num;
		this.elevatorController.Upgrade();
		Singleton<GameManager>.Instance.SetCash(-num2);
		Singleton<GameManager>.Instance.IdleCashCompute();
		Singleton<SoundManager>.Instance.Play("Upgrade");
		this.Show();
		Tracking.instance.UI_Interaction("ElevatorPopup", "Upgrade");
	}

	private bool IsMaxLevel()
	{
		int maxLevel = Singleton<GameProcess>.Instance.GetMaxLevel(Location.Elevator);
		return this.elevatorController.elevatorData.level == maxLevel;
	}

	private void OnCashChange(double cash)
	{
		if (!this.popup.activeInHierarchy || this.IsMaxLevel())
		{
			return;
		}
		this.Show();
	}
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantPopup : MonoBehaviour
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
	private Text currentWaiter;

	[SerializeField]
	private Text nextWaiter;

	[SerializeField]
	private Text currentLoadPerWaiter;

	[SerializeField]
	private Text nextLoadPerWaiter;

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
	private RestaurantController restaurantController;

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
		Tracking.instance.UI_Interaction("RestaurantPopup", "SelectUpgradeStep");
	}

	public void Show()
	{
		GameUtilities.String.ToText(this.title, "Restaurant Lv." + this.restaurantController.restaurantData.level.ToString());
		int lastBonusAtLevel = Singleton<GameProcess>.Instance.GetLastBonusAtLevel(this.restaurantController.restaurantData.level, Location.Restaurant);
		int nextBonusAtLevel = Singleton<GameProcess>.Instance.GetNextBonusAtLevel(this.restaurantController.restaurantData.level, Location.Restaurant);
		GameUtilities.String.ToText(this.currentLevel, "Level " + this.restaurantController.restaurantData.level.ToString());
		GameUtilities.String.ToText(this.nextLevel, (nextBonusAtLevel != 2147483647) ? ("Next boost at level " + nextBonusAtLevel.ToString()) : "Boost maximum.");
		this.currentLevelFill.fillAmount = ((nextBonusAtLevel != 2147483647) ? ((float)(this.restaurantController.restaurantData.level - lastBonusAtLevel) / (float)(nextBonusAtLevel - lastBonusAtLevel)) : 1f);
		RestaurantProperties restaurantProperties = this.restaurantController.restaurantProperties;
		GameUtilities.String.ToText(this.currentWaiter, restaurantProperties.waiter.ToString());
		GameUtilities.String.ToText(this.currentSpeed, Math.Round((double)restaurantProperties.walkingSpeed, 2).ToString());
		GameUtilities.String.ToText(this.currentLoadPerWaiter, GameUtilities.Currencies.Convert(restaurantProperties.loadPerWaiter));
		GameUtilities.String.ToText(this.currentLoading, GameUtilities.Currencies.Convert(restaurantProperties.loadingSpeed) + "/s");
		GameUtilities.String.ToText(this.currentTotal, GameUtilities.Currencies.Convert(restaurantProperties.transportation) + "/s");
		if (this.IsMaxLevel())
		{
			GameUtilities.String.ToText(this.nextTotal, "Max");
			GameUtilities.String.ToText(this.nextSpeed, "Max");
			GameUtilities.String.ToText(this.nextWaiter, "Max");
			GameUtilities.String.ToText(this.nextLoading, "Max");
			GameUtilities.String.ToText(this.levelNumber, "Max");
			GameUtilities.String.ToText(this.nextLoadPerWaiter, "Max");
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
				num = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(Singleton<GameManager>.Instance.database.cash, this.restaurantController.boostController.upgradeCostReduced, this.restaurantController.restaurantData.level, Location.Restaurant, 0);
			}
			num = Mathf.Clamp(num, 1, Singleton<GameProcess>.Instance.GetMaxLevel(Location.Restaurant) - 1);
			float distance = Vector3.Distance(this.restaurantController.gatheringPoint.position, this.restaurantController.exploitedPoint.position);
			RestaurantProperties restaurantProperties2 = Singleton<GameProcess>.Instance.GetRestaurantProperties(distance, this.restaurantController.restaurantData.level + num);
			GameUtilities.String.ToText(this.nextLoadPerWaiter, "+" + GameUtilities.Currencies.Convert(restaurantProperties2.loadPerWaiter - restaurantProperties.loadPerWaiter));
			GameUtilities.String.ToText(this.nextWaiter, "+" + (restaurantProperties2.waiter - restaurantProperties.waiter).ToString());
			GameUtilities.String.ToText(this.nextSpeed, "+" + Math.Round((double)(restaurantProperties2.walkingSpeed - restaurantProperties.walkingSpeed), 2).ToString());
			GameUtilities.String.ToText(this.nextLoading, "+" + GameUtilities.Currencies.Convert(restaurantProperties2.loadingSpeed - restaurantProperties.loadingSpeed) + "/s");
			GameUtilities.String.ToText(this.nextTotal, "+" + GameUtilities.Currencies.Convert(restaurantProperties2.transportation - restaurantProperties.transportation) + "/s");
			double num2 = Singleton<GameProcess>.Instance.GetUpgradePrice(this.restaurantController.restaurantData.level, num, this.restaurantController.boostController.upgradeCostReduced, Location.Restaurant, 0);
			GameUtilities.String.ToText(this.levelNumber, "Level Up x" + num.ToString());
			GameUtilities.String.ToText(this.upgradePrice, GameUtilities.Currencies.Convert(num2));
			this.upgradeButton.sprite = ((Singleton<GameManager>.Instance.database.cash < num2 || this.restaurantController.restaurantData.level + num > Singleton<GameProcess>.Instance.GetMaxLevel(Location.Restaurant)) ? this.disableSprite : this.enableSprite);
			this.nextLevelFill.fillAmount = ((nextBonusAtLevel != 2147483647) ? ((float)(this.restaurantController.restaurantData.level + num - lastBonusAtLevel) / (float)(nextBonusAtLevel - lastBonusAtLevel)) : 1f);
			int diamondBonus = Singleton<GameProcess>.Instance.GetDiamondBonus(this.restaurantController.restaurantData.level + num, this.restaurantController.restaurantData.level, Location.Elevator);
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
		Tracking.instance.UI_Interaction("RestaurantPopup", "ClosePopup");
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
			num = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(Singleton<GameManager>.Instance.database.cash, this.restaurantController.boostController.upgradeCostReduced, this.restaurantController.restaurantData.level, Location.Restaurant, 0);
		}
		num = Mathf.Clamp(num, 1, Singleton<GameProcess>.Instance.GetMaxLevel(Location.Elevator) - 1);
		double num2 = Singleton<GameProcess>.Instance.GetUpgradePrice(this.restaurantController.restaurantData.level, num, this.restaurantController.boostController.upgradeCostReduced, Location.Restaurant, 0);
		if (Singleton<GameManager>.Instance.database.cash < num2)
		{
			return;
		}
		int diamondBonus = Singleton<GameProcess>.Instance.GetDiamondBonus(this.restaurantController.restaurantData.level + num, this.restaurantController.restaurantData.level, Location.Restaurant);
		Singleton<GameManager>.Instance.SetDiamond(diamondBonus);
		this.restaurantController.restaurantData.level += num;
		this.restaurantController.Upgrade();
		Singleton<GameManager>.Instance.SetCash(-num2);
		Singleton<GameManager>.Instance.IdleCashCompute();
		Singleton<SoundManager>.Instance.Play("Upgrade");
		this.Show();
		Tracking.instance.UI_Interaction("RestaurantPopup", "Upgrade");
	}

	private bool IsMaxLevel()
	{
		int maxLevel = Singleton<GameProcess>.Instance.GetMaxLevel(Location.Restaurant);
		return this.restaurantController.restaurantData.level == maxLevel;
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

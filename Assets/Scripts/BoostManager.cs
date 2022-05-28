using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BoostManager : MonoBehaviour
{
	[Serializable]
	public struct SpriteBoost
	{
		public Sprite sprite;

		public int effective;
	}

	private sealed class _TotalEffectiveCompute_c__AnonStorey1
	{
		internal int i;

		internal BoostManager _this;

		internal bool __m__0(BoostManager.SpriteBoost target)
		{
			return target.effective == this._this.boostData.boosts[this.i].effective;
		}
	}

	private sealed class _Boosting_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal BoostManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		private static Predicate<Boost> __f__am_cache0;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _Boosting_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.boosting = true;
				break;
			case 1u:
				if (this._this.totalRemaining == 0)
				{
					this._this.boostData.boosts.RemoveAll((Boost item) => item.remaining == 0);
					this._this.TotalEffectiveCompute();
				}
				break;
			default:
				return false;
			}
			if (this._this.totalRemaining > 0)
			{
				if (this._this.boostData.boostRemaining > 0)
				{
					this._this.boostData.boostRemaining--;
				}
				for (int i = 0; i < this._this.boostData.boosts.Count; i++)
				{
					this._this.boostData.boosts[i].remaining--;
				}
				this._this.totalRemaining--;
				this._this.AdBoostPopupDisplay();
				GameUtilities.String.ToText(this._this.inventoryRemaining, GameUtilities.DateTime.Convert(this._this.totalRemaining));
				GameUtilities.String.ToText(this._this.mainScreenRemaining, GameUtilities.DateTime.Convert(this._this.totalRemaining));
				this._current = this._this.waitForSeconds;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.boosting = false;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}

		private static bool __m__0(Boost item)
		{
			return item.remaining == 0;
		}
	}

	public static BoostManager instance;

	private bool boosting;

	private int totalRemaining;

	[NonSerialized]
	public int totalEffective;

	[NonSerialized]
	public BoostData boostData;

    [SerializeField]
    public Configuration configuration;

	[SerializeField]
	private Image boostBorder;

	[SerializeField]
	private Image nextBoostFill;

	[SerializeField]
	private Image currentBoostFill;

	[SerializeField]
	private Image[] boostItem;

	[SerializeField]
	private BoostManager.SpriteBoost[] spriteBoost;

	[SerializeField]
	private Text boostDescription;

	[SerializeField]
	private Text inventoryEffective;

	[SerializeField]
	private Text inventoryRemaining;

	[SerializeField]
	private Text mainScreenEffective;

	[SerializeField]
	private Text mainScreenRemaining;

	[SerializeField]
	private GameObject boostVFX;

	[SerializeField]
	private GameObject targetPopup;

	[SerializeField]
	private GameObject enablePanel;

	[SerializeField]
	private GameObject disablePanel;

	private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

	private static Predicate<Boost> __f__am_cache0;

	private void Awake()
	{
		if (BoostManager.instance == null)
		{
			BoostManager.instance = this;
		}
		else if (BoostManager.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Initialize()
	{
		int targetRestaurant = Singleton<DataManager>.Instance.database.targetRestaurant;
		this.boostData = Singleton<DataManager>.Instance.database.restaurant[targetRestaurant].boost;
		this.OfflineTimeCalculate();
		this.TotalEffectiveCompute();
		this.AdBoostPopupDisplay();
	}

	public void TotalEffectiveCompute()
	{
		int num = 0;
		for (int k = 0; k < this.boostData.boosts.Count; k++)
		{
			num += this.boostData.boosts[k].effective;
		}
		if (num == 0)
		{
			num = 1;
		}
		int i;
		for (i = 0; i < this.boostItem.Length; i++)
		{
			this.boostItem[i].transform.gameObject.SetActive(i < this.boostData.boosts.Count);
			if (i < this.boostData.boosts.Count)
			{
				this.boostItem[i].transform.GetChild(1).GetComponent<Image>().sprite = Array.Find<BoostManager.SpriteBoost>(this.spriteBoost, (BoostManager.SpriteBoost target) => target.effective == this.boostData.boosts[i].effective).sprite;
				this.boostItem[i].transform.GetChild(1).GetComponent<Image>().SetNativeSize();
			}
		}
		int num2 = (!Singleton<DataManager>.Instance.database.nonConsume.Contains("onlinepack")) ? 1 : this.configuration.boost.onlineBoostEffective;
		int num3 = (this.boostData.boostRemaining <= 0) ? 1 : this.configuration.boost.boostIncomeEffective;
		this.boostBorder.enabled = (this.boostData.boostRemaining > 0);
		this.boostVFX.SetActive(this.boostData.boostRemaining > 0);
		this.totalEffective = num * num3 * num2;
		this.mainScreenEffective.transform.parent.gameObject.SetActive(this.totalEffective > 1);
		GameUtilities.String.ToText(this.inventoryEffective, "x" + this.totalEffective.ToString());
		GameUtilities.String.ToText(this.mainScreenEffective, "x" + this.totalEffective.ToString());
		if (this.boostData.boosts.Count > 0)
		{
			this.totalRemaining = this.boostData.boosts[0].remaining;
			for (int j = 0; j < this.boostData.boosts.Count; j++)
			{
				if (this.boostData.boosts[j].remaining < this.totalRemaining)
				{
					this.totalRemaining = this.boostData.boosts[j].remaining;
				}
			}
		}
		if (this.totalRemaining > 0)
		{
			if (this.boostData.boostRemaining > 0 && this.boostData.boostRemaining < this.totalRemaining)
			{
				this.totalRemaining = this.boostData.boostRemaining;
			}
		}
		else
		{
			this.totalRemaining = this.boostData.boostRemaining;
		}
		if (this.totalRemaining == 0)
		{
			GameUtilities.String.ToText(this.mainScreenRemaining, "BOOST");
			if (Singleton<DataManager>.Instance.database.nonConsume.Contains("onlinepack"))
			{
				GameUtilities.String.ToText(this.inventoryRemaining, "Unlimited");
			}
		}
		if (this.totalRemaining > 0 && !this.boosting)
		{
			base.StartCoroutine(this.Boosting());
		}
		this.enablePanel.SetActive(this.totalRemaining > 0 || Singleton<DataManager>.Instance.database.nonConsume.Contains("onlinepack"));
		this.disablePanel.SetActive(this.totalRemaining == 0 && !Singleton<DataManager>.Instance.database.nonConsume.Contains("onlinepack"));
	}

	public void AddBoostItem(Boost boost)
	{
		for (int i = 0; i < this.boostData.boosts.Count; i++)
		{
			if (this.boostData.boosts[i].effective == boost.effective)
			{
				this.boostData.boosts[i].remaining += boost.remaining;
				this.TotalEffectiveCompute();
				return;
			}
		}
		this.boostData.boosts.Add(boost);
		this.TotalEffectiveCompute();
	}

	public void WatchAdBoost()
	{
		if (!AdsControl.Instance.GetRewardAvailable())
		{
			Notification.instance.Warning("No available video at the moment.");
			Singleton<SoundManager>.Instance.Play("Notification");
			return;
		}
        if (AdsControl.Instance.GetRewardAvailable())
        {
            AdsControl.Instance.PlayDelegateRewardVideo(delegate
            {
                this.boostData.boostRemaining += this.configuration.boost.boostIncomeDuration;
                if (this.boostData.boostRemaining > this.configuration.boost.boostIncomeMaxDuration)
                {
                    this.boostData.boostRemaining = this.configuration.boost.boostIncomeMaxDuration;
                }
                this.TotalEffectiveCompute();
                this.AdBoostPopupDisplay();
                Tracking.instance.Ads_Impress("reward", "BoostIncome");
            });
            Tracking.instance.UI_Interaction("BoostPopup", "WatchAdsBoost");
        }

        /*
		MyAdvertisement.instance.ShowReward(delegate
		{
			this.boostData.boostRemaining += this.configuration.boost.boostIncomeDuration;
			if (this.boostData.boostRemaining > this.configuration.boost.boostIncomeMaxDuration)
			{
				this.boostData.boostRemaining = this.configuration.boost.boostIncomeMaxDuration;
			}
			this.TotalEffectiveCompute();
			this.AdBoostPopupDisplay();
			Tracking.instance.Ads_Impress("reward", "BoostIncome");
		});
		Tracking.instance.UI_Interaction("BoostPopup", "WatchAdsBoost");
		*/
    }

	public void ShowPopup(bool value)
	{
		if (value)
		{
			Singleton<SoundManager>.Instance.Play("Popup");
		}
		this.targetPopup.SetActive(value);
		if (!value)
		{
			Tracking.instance.UI_Interaction("BoostPopup", "ClosePopup");
		}
	}

	private IEnumerator Boosting()
	{
		BoostManager._Boosting_c__Iterator0 _Boosting_c__Iterator = new BoostManager._Boosting_c__Iterator0();
		_Boosting_c__Iterator._this = this;
		return _Boosting_c__Iterator;
	}

	private void AdBoostPopupDisplay()
	{
		float num = (float)this.configuration.boost.boostIncomeEffective;
		int num2 = this.configuration.boost.boostIncomeMaxDuration - this.boostData.boostRemaining;
		if (num2 > this.configuration.boost.boostIncomeDuration)
		{
			num2 = this.configuration.boost.boostIncomeDuration;
		}
		GameUtilities.String.ToText(this.boostDescription, string.Concat(new object[]
		{
			"<color=#009FD6FF>x",
			num,
			" income</color> for additional ",
			GameUtilities.DateTime.Convert(num2)
		}));
		this.nextBoostFill.fillAmount = (float)(this.boostData.boostRemaining + this.configuration.boost.boostIncomeDuration) / (float)this.configuration.boost.boostIncomeMaxDuration;
		this.currentBoostFill.fillAmount = (float)this.boostData.boostRemaining / (float)this.configuration.boost.boostIncomeMaxDuration;
	}

	private void OfflineTimeCalculate()
	{
		int targetRestaurant = Singleton<DataManager>.Instance.database.targetRestaurant;
		int num = GameUtilities.DateTime.Offline(Singleton<DataManager>.Instance.database.restaurant[targetRestaurant].dateTime);
		if (this.boostData.boostRemaining > num)
		{
			this.boostData.boostRemaining -= num;
		}
		else
		{
			this.boostData.boostRemaining = 0;
		}
		for (int i = 0; i < this.boostData.boosts.Count; i++)
		{
			if (this.boostData.boosts[i].remaining > num)
			{
				this.boostData.boosts[i].remaining -= num;
			}
			else
			{
				this.boostData.boosts[i].remaining = 0;
			}
		}
		this.boostData.boosts.RemoveAll((Boost item) => item.remaining == 0);
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused && this.boostData != null)
		{
			this.OfflineTimeCalculate();
			this.TotalEffectiveCompute();
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class OfflinePopup : MonoBehaviour
{
	[SerializeField]
	private Text offlineCash;

	[SerializeField]
	private Text rewardedCash;

	[SerializeField]
	private GameObject popup;

	[SerializeField]
	private Configuration config;

	[SerializeField]
	private CoinItemPool coinItemPool;

	[SerializeField]
	private Transform targetCoinLabel;

	private double offlineCashGain;

	private void Start()
	{
		this.Show();
	}

	private void Show()
	{
		int targetRestaurant = Singleton<DataManager>.Instance.database.targetRestaurant;
		int num = GameUtilities.DateTime.Offline(Singleton<DataManager>.Instance.database.restaurant[targetRestaurant].dateTime);
		double idleCash = Singleton<DataManager>.Instance.database.restaurant[targetRestaurant].idleCash;
		if (num == 0 || num < this.config.general.minOfflineTime || idleCash == 0.0)
		{
			return;
		}
		if (num > this.config.general.maxOfflineTime)
		{
			num = this.config.general.maxOfflineTime;
		}
		int num2 = (!Singleton<DataManager>.Instance.database.nonConsume.Contains("offlinepack")) ? 1 : this.config.boost.offlineBoostEffective;
		BoostData boost = Singleton<DataManager>.Instance.database.restaurant[targetRestaurant].boost;
		int num3 = 0;
		for (int i = 0; i < boost.boosts.Count; i++)
		{
			num3 += boost.boosts[i].effective;
		}
		if (num3 == 0)
		{
			num3 = 1;
		}
		int num4 = (boost.boostRemaining != 0) ? this.config.boost.boostIncomeEffective : 1;
		this.offlineCashGain = (double)num * idleCash * (double)num2 * (double)num3 * (double)num4;
		GameUtilities.String.ToText(this.offlineCash, GameUtilities.Currencies.Convert(this.offlineCashGain));
		GameUtilities.String.ToText(this.rewardedCash, GameUtilities.Currencies.Convert(this.offlineCashGain * 2.0));
		this.popup.SetActive(true);
	}

	public void DoubleReward()
	{
		if (!AdsControl.Instance.GetRewardAvailable())
		{
			Notification.instance.Warning("No available video at the moment.");
			Singleton<SoundManager>.Instance.Play("Notification");
			return;
		}
		AdsControl.Instance.PlayDelegateRewardVideo(delegate
		{
			this.Close(2);
			Tracking.instance.Ads_Impress("reward", "DoubleOfflineCoin");
		});
	}

	public void Close(int factor)
	{
		this.coinItemPool.Pool(this.targetCoinLabel, this.offlineCashGain * (double)factor);
		this.popup.SetActive(false);
		this.offlineCashGain = 0.0;
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			this.Show();
		}
	}
}

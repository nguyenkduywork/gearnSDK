using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GetFreeCoin : MonoBehaviour
{
	private sealed class _Initialize_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal WWW _www___0;

		internal GetFreeCoin _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

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

		public _Initialize_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._www___0 = new WWW("http://mega.ikame.vn/index.php?index=get_time");
				this._current = this._www___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._www___0.error == null)
				{
					this._this.currentTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
					this._this.currentTime = this._this.currentTime.AddSeconds(Convert.ToDouble(this._www___0.text)).ToLocalTime();
					DateTime dateTime = Convert.ToDateTime(this._this.freeCoinData.lastTimeGetFree);
					if ((dateTime.Day != this._this.currentTime.Day || dateTime.Month != this._this.currentTime.Month) && !Singleton<DataManager>.Instance.database.freeCashData.free)
					{
						Singleton<DataManager>.Instance.database.freeCashData.free = true;
					}
					if (this._this.freeCoinData.watchAds == this._this.config.freeCash.watchAdLimited)
					{
						int num2 = (int)this._this.currentTime.Subtract(Convert.ToDateTime(this._this.freeCoinData.lastTimeWatchAd)).TotalSeconds;
						if (num2 >= this._this.config.freeCash.cooldownPerAds)
						{
							this._this.freeCoinData.watchAds = 0;
						}
					}
				}
				this._this.FreeCashValidate();
				this._PC = -1;
				break;
			}
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
	}

	private sealed class _Cooldown_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _duration___0;

		internal GetFreeCoin _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

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

		public _Cooldown_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.cooldown = true;
				this._duration___0 = (int)DateTime.Now.Subtract(Convert.ToDateTime(this._this.freeCoinData.lastTimeWatchAd)).TotalSeconds;
				this._duration___0 = Mathf.Clamp(this._this.config.freeCash.cooldownPerAds - this._duration___0, 0, this._this.config.freeCash.cooldownPerAds);
				break;
			case 1u:
				this._duration___0--;
				break;
			default:
				return false;
			}
			if (this._duration___0 > 0)
			{
				GameUtilities.String.ToText(this._this.watchAdsRemaining, GameUtilities.DateTime.Convert(this._duration___0));
				this._current = this._this.waitForSeconds;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.cooldown = false;
			this._this.watchAdsLabel.SetActive(true);
			this._this.watchAdsRemaining.gameObject.SetActive(false);
			this._this.freeCoinData.watchAds = 0;
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
	}

	private bool cooldown;

	private Coroutine cooldowing;

	private DateTime currentTime;

	private FreeCoinData freeCoinData;

	private WaitForSeconds waitForSeconds;

	[SerializeField]
	private Configuration config;

	[SerializeField]
	private Text watchAdsRemaining;

	[SerializeField]
	private GameObject watchAdsLabel;

	[SerializeField]
	private GameObject freeCashButton;

	[SerializeField]
	private GameObject watchAdsButton;

	[SerializeField]
	private GameObject[] notification;

	private void Start()
	{
		this.waitForSeconds = new WaitForSeconds(1f);
		this.freeCoinData = Singleton<DataManager>.Instance.database.freeCashData;
		base.StartCoroutine(this.Initialize());
	}

	private IEnumerator Initialize()
	{
		GetFreeCoin._Initialize_c__Iterator0 _Initialize_c__Iterator = new GetFreeCoin._Initialize_c__Iterator0();
		_Initialize_c__Iterator._this = this;
		return _Initialize_c__Iterator;
	}

	private void FreeCashValidate()
	{
		this.freeCashButton.SetActive(this.freeCoinData.free);
		this.watchAdsButton.SetActive(!this.freeCoinData.free);
		for (int i = 0; i < this.notification.Length; i++)
		{
			this.notification[i].SetActive(this.freeCoinData.free);
		}
		bool flag = this.freeCoinData.watchAds == this.config.freeCash.watchAdLimited;
		this.watchAdsLabel.SetActive(!flag);
		this.watchAdsRemaining.gameObject.SetActive(flag);
		if (flag && !this.cooldown)
		{
			this.cooldowing = base.StartCoroutine(this.Cooldown());
		}
	}

	public void GetFreeCash()
	{
		this.freeCoinData.free = false;
		this.freeCoinData.lastTimeGetFree = DateTime.Now.ToString();
		Singleton<GameManager>.Instance.SetDiamond(this.config.freeCash.diamondBonus);
		Notification.instance.Warning("Received <color=#00FFDFFF>" + this.config.freeCash.diamondBonus.ToString() + "</color> diamond");
		Singleton<SoundManager>.Instance.Play("Rewarded");
		this.FreeCashValidate();
	}

	public void WatchAdsFreeCash()
	{
		if (!AdsControl.Instance.GetRewardAvailable())
		{
			Notification.instance.Warning("No available video at the moment.");
			Singleton<SoundManager>.Instance.Play("Notification");
			return;
		}
        AdsControl.Instance.PlayDelegateRewardVideo(delegate
		{
			if (this.freeCoinData.watchAds == this.config.freeCash.watchAdLimited)
			{
				return;
			}
			this.freeCoinData.watchAds++;
			if (this.freeCoinData.watchAds == this.config.freeCash.watchAdLimited)
			{
				this.freeCoinData.lastTimeWatchAd = DateTime.Now.ToString();
			}
			Singleton<GameManager>.Instance.SetDiamond(this.config.freeCash.diamondBonus);
			Notification.instance.Warning("Received <color=#00FFDFFF>" + this.config.freeCash.diamondBonus.ToString() + "</color> diamond");
			Singleton<SoundManager>.Instance.Play("Rewarded");
			this.FreeCashValidate();
			Tracking.instance.Ads_Impress("reward", "GetFreeDiamond");
		});
	}

	private IEnumerator Cooldown()
	{
		GetFreeCoin._Cooldown_c__Iterator1 _Cooldown_c__Iterator = new GetFreeCoin._Cooldown_c__Iterator1();
		_Cooldown_c__Iterator._this = this;
		return _Cooldown_c__Iterator;
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			if (this.cooldown)
			{
				this.cooldown = false;
				base.StopCoroutine(this.cooldowing);
			}
		}
		else
		{
			base.StartCoroutine(this.Initialize());
		}
	}
}

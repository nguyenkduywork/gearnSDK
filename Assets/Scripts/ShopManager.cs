using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	[Serializable]
	public struct PackProduct
	{
		public string id;

		public float price;

		public Text priceLabel;

		public GameObject target;
	}

	[Serializable]
	public struct CoinProduct
	{
		public int value;

		public int price;

		public Text priceLabel;

		public Text valueLabel;
	}

	[Serializable]
	public struct BoostProduct
	{
		public int time;

		public int value;

		public int price;

		public Text priceLabel;

		public Text valueLabel;
	}

	[Serializable]
	public struct DiamondProduct
	{
		public string id;

		public int value;

		public float price;

		public Text valueLabel;

		public Text priceLabel;
	}

	private sealed class _BuyPack_c__AnonStorey0
	{
		internal int index;

		internal ShopManager _this;

		internal void __m__0()
		{
			if (this.index != 0)
			{
				if (this.index == 1)
				{
					Singleton<DataManager>.Instance.database.nonConsume.Add(this._this.packProduct[this.index].id);
					this._this.boostManager.TotalEffectiveCompute();
					this._this.packProduct[this.index].target.SetActive(false);
				}
			}
			else
			{
				Singleton<DataManager>.Instance.database.nonConsume.Add(this._this.packProduct[this.index].id);
				this._this.packProduct[this.index].target.SetActive(false);
			}
			Singleton<SoundManager>.Instance.Play("Purchased");
			Notification.instance.Warning("Purchased Completed");
			if (Singleton<DataManager>.Instance.database.nonConsume.Count == this._this.packProduct.Length)
			{
				this._this.packHeader.SetActive(false);
			}
			Tracking.instance.IAP(this._this.packProduct[this.index].id);
		}
	}

	private sealed class _BuyCoin_c__AnonStorey1
	{
		internal int index;

		internal ShopManager _this;

		internal void __m__0()
		{
			this._this.gameManager.SetDiamond(-this._this.coinProduct[this.index].price);
			double cash = this._this.GetInstantCash() * (double)this._this.coinProduct[this.index].value;
			this._this.coinItemPool.Pool(this._this.coinTargetLabel, cash);
			Singleton<SoundManager>.Instance.Play("Purchased");
		}
	}

	private sealed class _BuyBoost_c__AnonStorey2
	{
		internal int index;

		internal ShopManager _this;

		internal void __m__0()
		{
			this._this.gameManager.SetDiamond(-this._this.boostProduct[this.index].price);
			Item item = new Item();
			item.duration = this._this.boostProduct[this.index].time;
			item.effective = this._this.boostProduct[this.index].value;
			item.itemCount = 1;
			Singleton<Inventory>.Instance.Add(item);
			Singleton<SoundManager>.Instance.Play("Purchased");
		}
	}

	private sealed class _BuyDiamond_c__AnonStorey3
	{
		internal int index;

		internal ShopManager _this;

		internal void __m__0()
		{
			this._this.gameManager.SetDiamond(this._this.diamondProduct[this.index].value);
			Notification.instance.Warning("Received <color=#00FFDFFF>" + this._this.diamondProduct[this.index].value.ToString() + "</color> diamond");
			Singleton<SoundManager>.Instance.Play("Purchased");
			Tracking.instance.IAP(this._this.diamondProduct[this.index].id);
		}
	}

	[SerializeField]
	private ShopManager.PackProduct[] packProduct;

	[SerializeField]
	private ShopManager.CoinProduct[] coinProduct;

	[SerializeField]
	private ShopManager.BoostProduct[] boostProduct;

	[SerializeField]
	private ShopManager.DiamondProduct[] diamondProduct;

	[SerializeField]
	private RectTransform shopRectransform;

	[SerializeField]
	private GameObject packHeader;

	[SerializeField]
	private GameObject targetPopup;

	[SerializeField]
	private GameManager gameManager;

	[SerializeField]
	private BoostManager boostManager;

	[SerializeField]
	private CoinItemPool coinItemPool;

	[SerializeField]
	private Transform coinTargetLabel;

	private void Start()
	{
		this.LoadDefaultPackProductPrice();
		this.LoadDefaultDiamondProductPrice();
		this.LoadDefaultBoostProductPrice();
		this.LoadDefaultCoinProductPrice();
	}

	private void LoadDefaultPackProductPrice()
	{
		for (int i = 0; i < this.packProduct.Length; i++)
		{
			if (Singleton<DataManager>.Instance.database.nonConsume.Contains(this.packProduct[i].id))
			{
				this.packProduct[i].target.SetActive(false);
			}
			else
			{
				GameUtilities.String.ToText(this.packProduct[i].priceLabel, "$" + this.packProduct[i].price);
			}
		}
		if (Singleton<DataManager>.Instance.database.nonConsume.Count == this.packProduct.Length)
		{
			this.packHeader.SetActive(false);
		}
	}

	private void LoadDefaultCoinProductPrice()
	{
		for (int i = 0; i < this.coinProduct.Length; i++)
		{
			GameUtilities.String.ToText(this.coinProduct[i].valueLabel, GameUtilities.Currencies.Convert(this.GetInstantCash() * (double)this.coinProduct[i].value));
			GameUtilities.String.ToText(this.coinProduct[i].priceLabel, this.coinProduct[i].price.ToString());
		}
	}

	private void LoadDefaultBoostProductPrice()
	{
		for (int i = 0; i < this.boostProduct.Length; i++)
		{
			GameUtilities.String.ToText(this.boostProduct[i].valueLabel, "+" + GameUtilities.DateTime.Convert(this.boostProduct[i].time));
			GameUtilities.String.ToText(this.boostProduct[i].priceLabel, this.boostProduct[i].price.ToString());
		}
	}

	private void LoadDefaultDiamondProductPrice()
	{
		for (int i = 0; i < this.diamondProduct.Length; i++)
		{
			GameUtilities.String.ToText(this.diamondProduct[i].priceLabel, "$" + this.diamondProduct[i].price);
			GameUtilities.String.ToText(this.diamondProduct[i].valueLabel, "+" + this.diamondProduct[i].value);
		}
	}

	private double GetInstantCash()
	{
		double idleCash = Singleton<DataManager>.Instance.database.restaurant[Singleton<DataManager>.Instance.database.targetRestaurant].idleCash;
		return Singleton<GameProcess>.Instance.GetInstantCash(idleCash);
	}

	public void BuyPack(int index)
	{
		InAppPurchase.instance.BuyProductID(this.packProduct[index].id, delegate
		{
			if (index != 0)
			{
				if (index == 1)
				{
					Singleton<DataManager>.Instance.database.nonConsume.Add(this.packProduct[index].id);
					this.boostManager.TotalEffectiveCompute();
					this.packProduct[index].target.SetActive(false);
				}
			}
			else
			{
				Singleton<DataManager>.Instance.database.nonConsume.Add(this.packProduct[index].id);
				this.packProduct[index].target.SetActive(false);
			}
			Singleton<SoundManager>.Instance.Play("Purchased");
			Notification.instance.Warning("Purchased Completed");
			if (Singleton<DataManager>.Instance.database.nonConsume.Count == this.packProduct.Length)
			{
				this.packHeader.SetActive(false);
			}
			Tracking.instance.IAP(this.packProduct[index].id);
		});
	}

	public void BuyCoin(int index)
	{
		if (Singleton<DataManager>.Instance.database.diamond < this.coinProduct[index].price)
		{
			Notification.instance.Warning("Not Enough Diamond");
			Singleton<SoundManager>.Instance.Play("Notification");
			return;
		}
		Notification.instance.Confirm(delegate
		{
			this.gameManager.SetDiamond(-this.coinProduct[index].price);
			double cash = this.GetInstantCash() * (double)this.coinProduct[index].value;
			this.coinItemPool.Pool(this.coinTargetLabel, cash);
			Singleton<SoundManager>.Instance.Play("Purchased");
		}, "Do you want to buy this item or <color=#00B5FFFF>" + this.coinProduct[index].price.ToString() + "</color> diamond ?");
	}

	public void BuyBoost(int index)
	{
		if (Singleton<DataManager>.Instance.database.diamond < this.boostProduct[index].price)
		{
			Notification.instance.Warning("Not Enough Diamond");
			Singleton<SoundManager>.Instance.Play("Notification");
			return;
		}
		Notification.instance.Confirm(delegate
		{
			this.gameManager.SetDiamond(-this.boostProduct[index].price);
			Item item = new Item();
			item.duration = this.boostProduct[index].time;
			item.effective = this.boostProduct[index].value;
			item.itemCount = 1;
			Singleton<Inventory>.Instance.Add(item);
			Singleton<SoundManager>.Instance.Play("Purchased");
		}, "Do you want to buy this item for <color=#00B5FFFF>" + this.boostProduct[index].price.ToString() + "</color> diamond ?");
	}

	public void BuyDiamond(int index)
	{
		InAppPurchase.instance.BuyProductID(this.diamondProduct[index].id, delegate
		{
			this.gameManager.SetDiamond(this.diamondProduct[index].value);
			Notification.instance.Warning("Received <color=#00FFDFFF>" + this.diamondProduct[index].value.ToString() + "</color> diamond");
			Singleton<SoundManager>.Instance.Play("Purchased");
			Tracking.instance.IAP(this.diamondProduct[index].id);
		});
	}

	public void ShowPopup(bool value)
	{
		if (value)
		{
			Singleton<SoundManager>.Instance.Play("Popup");
			this.LoadDefaultCoinProductPrice();
		}
		this.targetPopup.SetActive(value);
	}

	public void MoveToBoth(RectTransform rectTransform)
	{
		this.shopRectransform.anchoredPosition = new Vector2(this.shopRectransform.anchoredPosition.x, -(rectTransform.anchoredPosition.y + 120f));
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

public class InAppPurchase : MonoBehaviour, IStoreListener
{
	[Serializable]
	public class Products
	{
		public string name;

		public string price;

		public string appStoreID;

		public string googlePlayID;

		public ProductType productType;
	}

	private static IStoreController storeController;

	private static IExtensionProvider storeExtensionProvider;

	public static InAppPurchase instance;

	public List<InAppPurchase.Products> products;

	public Action purchased;

	private static Action<bool> __f__am_cache0;

	private void Awake()
	{
		if (InAppPurchase.instance == null)
		{
			InAppPurchase.instance = this;
		}
		else if (InAppPurchase.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		if (InAppPurchase.storeController == null)
		{
			this.InitializePurchasing();
		}
	}

	private void InitializePurchasing()
	{
		if (this.IsInitialized())
		{
			return;
		}
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);
		for (int i = 0; i < this.products.Count; i++)
		{
			configurationBuilder.AddProduct(this.products[i].name, this.products[i].productType, new IDs
			{
				{
					this.products[i].googlePlayID,
					new string[]
					{
						"GooglePlay"
					}
				},
				{
					this.products[i].appStoreID,
					new string[]
					{
						"AppleAppStore"
					}
				}
			});
		}
		UnityPurchasing.Initialize(this, configurationBuilder);
		base.Invoke("LoadLocalizePrice", 2f);
	}

	public string GetLocalizePrice(int index)
	{
		return (!this.IsInitialized()) ? this.products[index].price : InAppPurchase.storeController.products.WithID(this.products[index].googlePlayID).metadata.localizedPriceString;
	}

	private void LoadLocalizePrice()
	{
		Product[] all = InAppPurchase.storeController.products.all;
		if (all.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.products.Count; i++)
		{
			this.products[i].price = all[i].metadata.localizedPriceString;
		}
	}

	public void BuyProductID(string productID, Action buyDone)
	{
		if (!this.IsInitialized())
		{
			return;
		}
		this.purchased = buyDone;
		Product product = InAppPurchase.storeController.products.WithID(productID);
		if (product != null && product.availableToPurchase)
		{
			InAppPurchase.storeController.InitiatePurchase(product);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		InAppPurchase.storeController = controller;
		InAppPurchase.storeExtensionProvider = extensions;
	}

	public void RestorePurchases()
	{
		if (!this.IsInitialized())
		{
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			IAppleExtensions extension = InAppPurchase.storeExtensionProvider.GetExtension<IAppleExtensions>();
			extension.RestoreTransactions(delegate(bool result)
			{
				if (result && Notification.instance != null)
				{
					Notification.instance.Warning("Restore Completed.");
				}
				if (!result && Notification.instance != null)
				{
					Notification.instance.Warning("Restore Failed.");
				}
			});
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		//bool flag = true;
		//CrossPlatformValidator crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
		//try
		//{
		//	IPurchaseReceipt[] array = crossPlatformValidator.Validate(args.purchasedProduct.receipt);
		//}
		//catch (IAPSecurityException)
		//{
		//	flag = false;
		//}
		//if (flag)
		//{
			for (int i = 0; i < this.products.Count; i++)
			{
				if (string.Equals(args.purchasedProduct.definition.id, this.products[i].name, StringComparison.Ordinal))
				{
					if (this.purchased != null)
					{
						this.purchased();
					}
					else if (this.products[i].productType == ProductType.NonConsumable)
					{
						string name = this.products[i].name;
						if (name != null)
						{
							if (!(name == "offlinepack"))
							{
								if (name == "onlinepack")
								{
									Singleton<DataManager>.Instance.database.nonConsume.Add(this.products[i].name);
									if (BoostManager.instance != null)
									{
										BoostManager.instance.TotalEffectiveCompute();
									}
								}
							}
							else
							{
								Singleton<DataManager>.Instance.database.nonConsume.Add(this.products[i].name);
							}
						}
					}
					return PurchaseProcessingResult.Complete;
				}
			}
			this.purchased = null;
		//}
		return PurchaseProcessingResult.Complete;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
	}

	private bool IsInitialized()
	{
		return InAppPurchase.storeController != null && InAppPurchase.storeExtensionProvider != null;
	}
}

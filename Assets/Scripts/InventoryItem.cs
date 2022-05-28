using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
	private Item item;

	private Action onChange;

	[SerializeField]
	private Text itemCount;

	[SerializeField]
	private Text itemValue;

	[SerializeField]
	private Image itemIcon;

	public void Init(Item item)
	{
		this.item = item;
		this.onChange = new Action(this.UpdateItemCount);
		Inventory expr_1E = Singleton<Inventory>.Instance;
		expr_1E.onChange = (Action)Delegate.Combine(expr_1E.onChange, this.onChange);
		this.Display();
	}

	public void Use()
	{
		Boost boost = new Boost();
		boost.duration = this.item.duration;
		boost.remaining = this.item.duration;
		boost.effective = this.item.effective;
		BoostManager.instance.AddBoostItem(boost);
		if (this.item.itemCount == 1)
		{
			Inventory expr_5A = Singleton<Inventory>.Instance;
			expr_5A.onChange = (Action)Delegate.Remove(expr_5A.onChange, this.onChange);
			Singleton<Inventory>.Instance.Remove(this.item);
			this.item = null;
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.item.itemCount--;
		}
		this.UpdateItemCount();
		Tracking.instance.UI_Interaction("InventoryPopup", "UseBoostItem");
	}

	private void UpdateItemCount()
	{
		if (this.item != null)
		{
			GameUtilities.String.ToText(this.itemCount, "Left: " + this.item.itemCount.ToString());
		}
	}

	private void Display()
	{
		GameUtilities.String.ToText(this.itemValue, GameUtilities.DateTime.Convert(this.item.duration));
		this.itemIcon.sprite = Singleton<Inventory>.Instance.GetItemSprite(this.item.effective);
		this.itemIcon.SetNativeSize();
		this.UpdateItemCount();
	}
}

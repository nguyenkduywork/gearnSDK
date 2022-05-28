using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
	[Serializable]
	public struct ItemSprite
	{
		public int effective;

		public Sprite sprite;
	}

	private sealed class _GetItemSprite_c__AnonStorey0
	{
		internal int effective;

		internal bool __m__0(Inventory.ItemSprite item)
		{
			return item.effective == this.effective;
		}
	}

	private List<Item> items;

	public Action onChange;

	[SerializeField]
	private GameObject popup;

	[SerializeField]
	private GameObject itemSlot;

	[SerializeField]
	private Transform itemContent;

	[SerializeField]
	private Inventory.ItemSprite[] itemSprite;

	private void Start()
	{
		this.items = Singleton<DataManager>.Instance.database.item;
		for (int i = 0; i < this.items.Count; i++)
		{
			this.CreateInventoryItem(this.items[i]);
		}
	}

	public Sprite GetItemSprite(int effective)
	{
		return Array.Find<Inventory.ItemSprite>(this.itemSprite, (Inventory.ItemSprite item) => item.effective == effective).sprite;
	}

	public void Add(Item item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].effective == item.effective && this.items[i].duration == item.duration)
			{
				this.items[i].itemCount += item.itemCount;
				if (this.onChange != null)
				{
					this.onChange();
				}
				return;
			}
		}
		this.items.Add(item);
		this.CreateInventoryItem(item);
	}

	public void Remove(Item item)
	{
		this.items.Remove(item);
	}

	public void Show(bool value)
	{
		if (value)
		{
			Singleton<SoundManager>.Instance.Play("Popup");
		}
		this.popup.SetActive(value);
		if (!value)
		{
			Tracking.instance.UI_Interaction("InventoryPopup", "ClosePopup");
		}
	}

	private void CreateInventoryItem(Item item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemSlot, this.itemContent);
		gameObject.GetComponent<InventoryItem>().Init(item);
	}
}

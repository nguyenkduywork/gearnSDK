using System;
using System.Collections.Generic;

[Serializable]
public class Database
{
	public double cash;

	public int diamond;

	public int targetRestaurant;

	public List<Item> item;

	public List<string> nonConsume;

	public FreeCoinData freeCashData;

	public SoundSetting soundSetting;

	public List<Restaurant> restaurant;

	public List<int> tutorialCompleted;

	public Database(Configuration config)
	{
		this.item = new List<Item>();
		this.nonConsume = new List<string>();
		this.restaurant = new List<Restaurant>();
		this.restaurant.Add(new Restaurant(config));
		this.tutorialCompleted = new List<int>();
		this.soundSetting = new SoundSetting();
		this.freeCashData = new FreeCoinData();
		this.cash = config.general.startCash;
		this.diamond = config.general.startDiamond;
	}
}

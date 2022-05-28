using System;
using System.Collections.Generic;

[Serializable]
public class Restaurant
{
	public double idleCash;

	public string dateTime;

	public BoostData boost;

	public BarrierData barrier;

	public ElevatorData elevator;

	public RestaurantData restaurant;

	public List<KitchenData> kitchen;

	public List<ManagerProfile> profile;

	public int kitchenRecruitment;

	public int elevatorRecruitment;

	public int restaurantRecruitment;

	public Restaurant(Configuration config)
	{
		this.dateTime = DateTime.Now.ToString();
		this.boost = new BoostData();
		this.kitchen = new List<KitchenData>();
		this.profile = new List<ManagerProfile>();
		this.elevator = new ElevatorData
		{
			level = 1
		};
		this.restaurant = new RestaurantData
		{
			level = 1
		};
		this.barrier = new BarrierData
		{
			nextBarrierFloor = config.kitchen.barrierStep
		};
	}
}

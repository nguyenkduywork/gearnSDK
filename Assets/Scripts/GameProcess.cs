using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameProcess : Singleton<GameProcess>
{
	private sealed class _GetManagerTextColor_c__AnonStorey0
	{
		internal Experience experience;

		internal bool __m__0(Configuration.ExperienceRate target)
		{
			return target.experience == this.experience;
		}
	}

	private sealed class _GetManagerBorderColor_c__AnonStorey1
	{
		internal Experience experience;

		internal bool __m__0(Configuration.ExperienceRate target)
		{
			return target.experience == this.experience;
		}
	}

	private sealed class _GetManagerSkillDuration_c__AnonStorey2
	{
		internal Experience experience;

		internal ManagerSkill skill;

		internal bool __m__0(Configuration.SkillEffective target)
		{
			return target.experience == this.experience && target.skill == this.skill;
		}
	}

	private sealed class _GetManagerSkillCooldown_c__AnonStorey3
	{
		internal Experience experience;

		internal ManagerSkill skill;

		internal bool __m__0(Configuration.SkillEffective target)
		{
			return target.experience == this.experience && target.skill == this.skill;
		}
	}

	private sealed class _GetManagerSkillEffective_c__AnonStorey4
	{
		internal Experience experience;

		internal ManagerSkill skill;

		internal bool __m__0(Configuration.SkillEffective target)
		{
			return target.experience == this.experience && target.skill == this.skill;
		}
	}

	private sealed class _GetManagerAvatarSprite_c__AnonStorey5
	{
		internal Experience experience;

		internal bool __m__0(Configuration.ExperienceRate target)
		{
			return target.experience == this.experience;
		}
	}

	private sealed class _GetManagerSkillSprite_c__AnonStorey6
	{
		internal ManagerSkill skill;

		internal bool __m__0(Configuration.SkillRate target)
		{
			return target.skill == this.skill;
		}
	}

	[SerializeField]
	private Configuration configuration;

	public Color GetManagerTextColor(Experience experience)
	{
		return Array.Find<Configuration.ExperienceRate>(this.configuration.experienceRate, (Configuration.ExperienceRate target) => target.experience == experience).textColor;
	}

	public Color GetManagerBorderColor(Experience experience)
	{
		return Array.Find<Configuration.ExperienceRate>(this.configuration.experienceRate, (Configuration.ExperienceRate target) => target.experience == experience).borderColor;
	}

	public ManagerProfile GetManagerProfile(Location location, double price, bool noJunior)
	{
		ManagerProfile managerProfile = new ManagerProfile();
		managerProfile.price = price;
		managerProfile.location = location;
		int num = 0;
		for (int i = (!noJunior) ? 0 : 1; i < this.configuration.experienceRate.Length; i++)
		{
			num += this.configuration.experienceRate[i].rate;
		}
		int num2 = UnityEngine.Random.Range(0, num);
		for (int j = (!noJunior) ? 0 : 1; j < this.configuration.experienceRate.Length; j++)
		{
			if (num2 <= this.configuration.experienceRate[j].rate)
			{
				managerProfile.experience = this.configuration.experienceRate[j].experience;
				break;
			}
			num2 -= this.configuration.experienceRate[j].rate;
		}
		num = 0;
		for (int k = 0; k < this.configuration.skillRate.Length; k++)
		{
			num += this.configuration.skillRate[k].rate;
		}
		while (true)
		{
			num2 = UnityEngine.Random.Range(0, num);
			for (int l = 0; l < this.configuration.skillRate.Length; l++)
			{
				if (num2 <= this.configuration.skillRate[l].rate)
				{
					managerProfile.skill = this.configuration.skillRate[l].skill;
					break;
				}
				num2 -= this.configuration.skillRate[l].rate;
			}
			if ((location != Location.Elevator || managerProfile.skill != ManagerSkill.CookingSpeed) && (location != Location.Elevator || managerProfile.skill != ManagerSkill.WalkingSpeed))
			{
				if ((location != Location.Restaurant || managerProfile.skill != ManagerSkill.CookingSpeed) && (location != Location.Restaurant || managerProfile.skill != ManagerSkill.MovementSpeed))
				{
					if ((location != Location.Kitchen || managerProfile.skill != ManagerSkill.LoadExpansion) && (location != Location.Kitchen || managerProfile.skill != ManagerSkill.MovementSpeed) && (location != Location.Kitchen || managerProfile.skill != ManagerSkill.LoadingSpeed))
					{
						break;
					}
				}
			}
		}
		return managerProfile;
	}

	public double GetManagerPrice(int count, Location position)
	{
		if (position == Location.Kitchen)
		{
			return Math.Round((double)this.configuration.kitchen.baseManagerPrice * Math.Pow((double)this.configuration.kitchen.hireManagerFactor, (double)count));
		}
		if (position == Location.Elevator)
		{
			return Math.Round((double)this.configuration.elevator.baseManagerPrice * Math.Pow((double)this.configuration.elevator.hireManagerFactor, (double)count));
		}
		if (position == Location.Restaurant)
		{
			return Math.Round((double)this.configuration.restaurant.baseManagerPrice * Math.Pow((double)this.configuration.restaurant.hireManagerFactor, (double)count));
		}
		return 0.0;
	}

	public int GetManagerSkillDuration(Experience experience, ManagerSkill skill)
	{
		return Array.Find<Configuration.SkillEffective>(this.configuration.skillEffective, (Configuration.SkillEffective target) => target.experience == experience && target.skill == skill).remaining;
	}

	public int GetManagerSkillCooldown(Experience experience, ManagerSkill skill)
	{
		return Array.Find<Configuration.SkillEffective>(this.configuration.skillEffective, (Configuration.SkillEffective target) => target.experience == experience && target.skill == skill).cooldown;
	}

	public int GetManagerSkillEffective(Experience experience, ManagerSkill skill)
	{
		return Array.Find<Configuration.SkillEffective>(this.configuration.skillEffective, (Configuration.SkillEffective target) => target.experience == experience && target.skill == skill).effective;
	}

	public Sprite GetManagerAvatarSprite(Experience experience)
	{
		return Array.Find<Configuration.ExperienceRate>(this.configuration.experienceRate, (Configuration.ExperienceRate target) => target.experience == experience).avatar;
	}

	public Sprite GetManagerSkillSprite(ManagerSkill skill, bool value)
	{
		Configuration.SkillRate skillRate = Array.Find<Configuration.SkillRate>(this.configuration.skillRate, (Configuration.SkillRate target) => target.skill == skill);
		return (!value) ? skillRate.disableSprite : skillRate.enableSprite;
	}

	public double GetInstantCash(double idleCash)
	{
		double num = Math.Round(idleCash * (double)this.configuration.general.instantCoinTime);
		return (num <= 0.0) ? ((double)this.configuration.general.baseInstantCoin) : num;
	}

	public int GetMaxLevel(Location location)
	{
		if (location == Location.Elevator)
		{
			return this.configuration.elevator.maxLevel;
		}
		if (location != Location.Restaurant)
		{
			return this.configuration.kitchen.maxLevel;
		}
		return this.configuration.restaurant.maxLevel;
	}

	public double GetUpgradePrice(int level, int step, int reduce, Location location, int floor = 0)
	{
		double num = 0.0;
		float num2 = 0f;
		if (location != Location.Kitchen)
		{
			if (location != Location.Elevator)
			{
				if (location == Location.Restaurant)
				{
					num = (double)this.configuration.restaurant.baseUpgradePrice;
					num2 = this.configuration.restaurant.upgradePriceFactor;
				}
			}
			else
			{
				num = (double)this.configuration.elevator.baseUpgradePrice;
				num2 = this.configuration.elevator.upgradePriceFactor;
			}
		}
		else
		{
			num = this.GetFloorPrice(floor);
			num2 = this.configuration.kitchen.upgradePriceFactor;
		}
		float num3 = (num2 + 100f) / 100f;
		double num4 = Math.Round(num * ((Math.Pow((double)num3, (double)(level + step)) - Math.Pow((double)num3, (double)level)) / (double)(num3 - 1f)));
		return num4 - num4 / 100.0 * (double)reduce;
	}

	public int GetMaxUpgradeLevel(double cash, int reduce, int level, Location location, int floor = 0)
	{
		float num = 0f;
		double num2 = 0.0;
		int num3 = 0;
		if (location != Location.Kitchen)
		{
			if (location != Location.Elevator)
			{
				if (location == Location.Restaurant)
				{
					num3 = this.configuration.restaurant.maxLevel;
					num = this.configuration.restaurant.upgradePriceFactor;
					num2 = this.GetUpgradePrice(level, 1, reduce, location, 0);
				}
			}
			else
			{
				num3 = this.configuration.elevator.maxLevel;
				num = this.configuration.elevator.upgradePriceFactor;
				num2 = this.GetUpgradePrice(level, 1, reduce, location, 0);
			}
		}
		else
		{
			num3 = this.configuration.kitchen.maxLevel;
			num = this.configuration.kitchen.upgradePriceFactor;
			num2 = this.GetUpgradePrice(level, 1, reduce, location, floor);
		}
		num = (num + 100f) / 100f;
		int num4 = (int)Math.Log(cash / num2 * (double)(num - 1f) + 1.0, (double)num);
		if (level + num4 > num3)
		{
			num4 = num3 - level;
		}
		return num4;
	}

	public int GetNextBonusAtLevel(int level, Location location)
	{
		Configuration.NextBonusLevel[] array = new Configuration.NextBonusLevel[0];
		int result = 2147483647;
		if (location != Location.Kitchen)
		{
			if (location != Location.Elevator)
			{
				if (location == Location.Restaurant)
				{
					array = this.configuration.restaurant.bonus;
				}
			}
			else
			{
				array = this.configuration.elevator.bonus;
			}
		}
		else
		{
			array = this.configuration.kitchen.bonus;
		}
		if (level >= array[array.Length - 1].level)
		{
			return result;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (level < array[i].level)
			{
				result = array[i].level;
				break;
			}
		}
		return result;
	}

	public int GetLastBonusAtLevel(int level, Location location)
	{
		Configuration.NextBonusLevel[] array = new Configuration.NextBonusLevel[0];
		int result = 0;
		if (location != Location.Kitchen)
		{
			if (location != Location.Elevator)
			{
				if (location == Location.Restaurant)
				{
					array = this.configuration.restaurant.bonus;
				}
			}
			else
			{
				array = this.configuration.elevator.bonus;
			}
		}
		else
		{
			array = this.configuration.kitchen.bonus;
		}
		if (level < array[0].level)
		{
			return result;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (level < array[i].level)
			{
				result = array[i - 1].level;
				break;
			}
		}
		return result;
	}

	public int GetDiamondBonus(int nextLevel, int currentLevel, Location location)
	{
		Configuration.NextBonusLevel[] array = new Configuration.NextBonusLevel[0];
		int num = 0;
		if (location != Location.Kitchen)
		{
			if (location != Location.Elevator)
			{
				if (location == Location.Restaurant)
				{
					array = this.configuration.restaurant.bonus;
				}
			}
			else
			{
				array = this.configuration.elevator.bonus;
			}
		}
		else
		{
			array = this.configuration.kitchen.bonus;
		}
		if (currentLevel > array[array.Length - 1].level)
		{
			return num;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].level > currentLevel && array[i].level <= nextLevel)
			{
				num += array[i].superCash;
			}
		}
		return num;
	}

	public double GetTotalPerformance(int loop, float distance, float movementSpeed, double workingSpeed, double capacity)
	{
		return Math.Round(capacity / (capacity / workingSpeed * (double)loop + (double)(distance * 2f / movementSpeed)));
	}

	public double GetFloorPrice(int floor)
	{
		int barrierStep = this.configuration.kitchen.barrierStep;
		float unlockBarrierFactor = this.configuration.kitchen.unlockBarrierFactor;
		return Math.Round((double)this.configuration.kitchen.baseUnlockPrice * Math.Pow((double)this.configuration.kitchen.unlockPriceFactor, (double)floor) * Math.Pow((double)unlockBarrierFactor, (double)(floor / barrierStep)));
	}

	public int GetFloorPrice(int floor, double idleCash)
	{
		double floorPrice = this.GetFloorPrice(floor);
		int value = (int)(floorPrice / idleCash / 60.0);
		return Mathf.Clamp(value, this.configuration.kitchen.minSuperCashUnlock, this.configuration.kitchen.maxSuperCashUnlock);
	}

	public KitchenProperties GetKitchenProperties(float distance, int floor, int level, float multiplier)
	{
		KitchenProperties result = default(KitchenProperties);
		double floorPrice = this.GetFloorPrice(floor);
		double num = (double)this.configuration.kitchen.transporterCapacityFactor / Math.Pow((double)this.configuration.kitchen.transporterCapacityFactor, (double)floor);
		float num2 = (this.configuration.kitchen.upgradeBonusFactor + 100f) / 100f;
		int num3 = 0;
		for (int i = 0; i < this.configuration.kitchen.bonusTransporterLevel.Length; i++)
		{
			if (level >= this.configuration.kitchen.bonusTransporterLevel[i])
			{
				num3++;
			}
		}
		result.transporter = 1 + num3;
		result.walkingSpeed = this.configuration.kitchen.baseWalkingSpeed;
		result.transporterCapacity = Math.Round(floorPrice * num * Math.Pow((double)num2, (double)(level - 1)) * (double)multiplier);
		result.workingSpeed = Math.Round(result.transporterCapacity / (double)this.configuration.kitchen.baseCookingTime);
		result.totalExtraction = this.GetTotalPerformance(1, distance, result.walkingSpeed, result.workingSpeed, result.transporterCapacity);
		return result;
	}

	public ElevatorProperties GetElevatorProperties(float distance, int level)
	{
		ElevatorProperties result = default(ElevatorProperties);
		int baseUpgradePrice = this.configuration.elevator.baseUpgradePrice;
		float num = (this.configuration.elevator.loadFactor + 100f) / 100f;
		float num2 = (this.configuration.elevator.upgradeLoadBonusFactor + 100f) / 100f;
		result.load = Math.Round((double)(num * (float)baseUpgradePrice) * Math.Pow((double)num2, (double)(level - 1)));
		result.loadingSpeed = Math.Round(result.load / (double)this.configuration.elevator.baseLoadingTime);
		result.movementSpeed = (float)Math.Round((double)(this.configuration.elevator.baseMovementSpeed + (float)level * (this.configuration.elevator.baseMovementSpeed * (this.configuration.elevator.movementSpeedFactor / 100f))), 2);
		result.transportation = this.GetTotalPerformance(1, distance, result.movementSpeed, result.loadingSpeed, result.load);
		return result;
	}

	public RestaurantProperties GetRestaurantProperties(float distance, int level)
	{
		RestaurantProperties result = default(RestaurantProperties);
		int baseUpgradePrice = this.configuration.restaurant.baseUpgradePrice;
		float num = this.configuration.restaurant.loadFactor / 100f;
		float num2 = (this.configuration.restaurant.upgradeLoadBonusFactor + 100f) / 100f;
		int num3 = 0;
		for (int i = 0; i < this.configuration.restaurant.bonusWaiterLevel.Length; i++)
		{
			if (level >= this.configuration.restaurant.bonusWaiterLevel[i])
			{
				num3++;
			}
		}
		result.waiter = 1 + num3;
		result.walkingSpeed = this.configuration.restaurant.baseWalkingSpeed;
		result.loadPerWaiter = Math.Round((double)(num * (float)baseUpgradePrice) * Math.Pow((double)num2, (double)(level - 1)));
		result.loadingSpeed = Math.Round(result.loadPerWaiter / (double)this.configuration.restaurant.baseLoadingTime);
		result.transportation = this.GetTotalPerformance(2, distance, result.walkingSpeed, result.loadingSpeed, result.loadPerWaiter);
		return result;
	}
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Configuration")]
public class Configuration : ScriptableObject
{
	[Serializable]
	public struct General
	{
		public string dataName;

		public double startCash;

		public int startDiamond;

		public int idleCashRate;

		public int minOfflineTime;

		public int maxOfflineTime;

		public int baseInstantCoin;

		public int instantCoinTime;
	}

	[Serializable]
	public struct Boost
	{
		public int boostIncomeDuration;

		public int boostIncomeEffective;

		public int boostIncomeMaxDuration;

		public int onlineBoostEffective;

		public int offlineBoostEffective;
	}

	[Serializable]
	public struct Kitchen
	{
		public int maxLevel;

		public int maxFloor;

		public int barrierStep;

		public int baseUnlockPrice;

		public int baseManagerPrice;

		public int minSuperCashUnlock;

		public int maxSuperCashUnlock;

		public float baseCookingTime;

		public float baseWalkingSpeed;

		public float hireManagerFactor;

		public float unlockPriceFactor;

		public float upgradePriceFactor;

		public float upgradeBonusFactor;

		public float unlockBarrierFactor;

		public float transporterCapacityFactor;

		public int[] bonusTransporterLevel;

		public Configuration.NextBonusLevel[] bonus;
	}

	[Serializable]
	public struct Elevator
	{
		public int maxLevel;

		public int baseManagerPrice;

		public int baseUpgradePrice;

		public float loadFactor;

		public float baseLoadingTime;

		public float baseMovementSpeed;

		public float hireManagerFactor;

		public float upgradePriceFactor;

		public float movementSpeedFactor;

		public float upgradeLoadBonusFactor;

		public Configuration.NextBonusLevel[] bonus;
	}

	[Serializable]
	public struct Restaurant
	{
		public int maxLevel;

		public int baseManagerPrice;

		public int baseUpgradePrice;

		public float baseLoadingTime;

		public float baseWalkingSpeed;

		public float loadFactor;

		public float hireManagerFactor;

		public float upgradePriceFactor;

		public float upgradeLoadBonusFactor;

		public int[] bonusWaiterLevel;

		public Configuration.NextBonusLevel[] bonus;
	}

	[Serializable]
	public struct NextBonusLevel
	{
		public int level;

		public int superCash;
	}

	[Serializable]
	public struct Barrier
	{
		public int unlockDuration;

		public int diamondToUnlock;

		public int reduceProcessTime;
	}

	[Serializable]
	public struct FreeCash
	{
		public int diamondBonus;

		public int watchAdLimited;

		public int cooldownPerAds;
	}

	[Serializable]
	public struct SkillRate
	{
		public int rate;

		public ManagerSkill skill;

		public Sprite enableSprite;

		public Sprite disableSprite;
	}

	[Serializable]
	public struct ExperienceRate
	{
		public int rate;

		public Sprite avatar;

		public Color textColor;

		public Color borderColor;

		public Experience experience;
	}

	[Serializable]
	public struct SkillEffective
	{
		public int effective;

		public int cooldown;

		public int remaining;

		public ManagerSkill skill;

		public Experience experience;
	}

	public Configuration.General general;

	public Configuration.Boost boost;

	public Configuration.Kitchen kitchen;

	public Configuration.Elevator elevator;

	public Configuration.Restaurant restaurant;

	public Configuration.FreeCash freeCash;

	public Configuration.Barrier[] barrier;

	public Configuration.SkillRate[] skillRate;

	public Configuration.ExperienceRate[] experienceRate;

	public Configuration.SkillEffective[] skillEffective;
}

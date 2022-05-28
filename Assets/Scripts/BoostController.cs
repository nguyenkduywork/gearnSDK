using System;
using UnityEngine;

public class BoostController : MonoBehaviour
{
	[HideInInspector]
	public int upgradeCostReduced;

	[HideInInspector]
	public float walkingSpeedBoost;

	[HideInInspector]
	public float cookingSpeedBoost;

	[HideInInspector]
	public float loadingSpeedBoost;

	[HideInInspector]
	public float loadExpansionBoost;

	[HideInInspector]
	public float movementSpeedBoost;

	public void Refresh()
	{
		this.walkingSpeedBoost = 1f;
		this.cookingSpeedBoost = 1f;
		this.loadingSpeedBoost = 1f;
		this.loadExpansionBoost = 1f;
		this.movementSpeedBoost = 1f;
		this.upgradeCostReduced = 0;
	}
}

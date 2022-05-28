using System;
using System.Collections.Generic;

[Serializable]
public class BoostData
{
	public int boostRemaining;

	public List<Boost> boosts;

	public BoostData()
	{
		this.boosts = new List<Boost>();
	}
}

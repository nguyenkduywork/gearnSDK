using System;

[Serializable]
public class BarrierData
{
	public BarrierState state;

	public int unlockRemaining;

	public int nextBarrierFloor;
}

using System;

[Serializable]
public class FreeCoinData
{
	public bool free;

	public int watchAds;

	public string lastTimeWatchAd;

	public string lastTimeGetFree;

	public FreeCoinData()
	{
		this.free = true;
		this.watchAds = 0;
		this.lastTimeWatchAd = DateTime.Now.ToString();
		this.lastTimeGetFree = DateTime.Now.ToString();
	}
}

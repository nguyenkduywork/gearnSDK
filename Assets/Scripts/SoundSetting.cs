using System;

[Serializable]
public class SoundSetting
{
	public bool sound;

	public bool music;

	public SoundSetting()
	{
		this.sound = (this.music = false);
	}
}

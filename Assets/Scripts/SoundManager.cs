using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	[Serializable]
	public struct Audio
	{
		public float volume;

		public AudioClip clip;
	}

	private sealed class _Play_c__AnonStorey0
	{
		internal string audioClip;

		internal bool __m__0(SoundManager.Audio target)
		{
			return target.clip.name == this.audioClip;
		}
	}

	private SoundSetting soundSetting;

	[SerializeField]
	private SettingPopup setting;

	[SerializeField]
	private AudioSource musicSource;

	[SerializeField]
	private AudioSource soundSource;

	[SerializeField]
	private SoundManager.Audio[] soundtrack;

	private void Start()
	{
		this.soundSetting = Singleton<DataManager>.Instance.database.soundSetting;
		this.musicSource.mute = this.soundSetting.music;
		this.soundSource.mute = this.soundSetting.sound;
		this.setting.MusicChange(this.soundSetting.music);
		this.setting.SoundChange(this.soundSetting.sound);
	}

	public void MusicChange()
	{
		this.soundSetting.music = !this.soundSetting.music;
		this.musicSource.mute = this.soundSetting.music;
		this.setting.MusicChange(this.soundSetting.music);
	}

	public void SoundChange()
	{
		this.soundSetting.sound = !this.soundSetting.sound;
		this.soundSource.mute = this.soundSetting.sound;
		this.setting.SoundChange(this.soundSetting.sound);
	}

	public void Play(string audioClip)
	{
		SoundManager.Audio audio = Array.Find<SoundManager.Audio>(this.soundtrack, (SoundManager.Audio target) => target.clip.name == audioClip);
		if (audio.clip != null)
		{
			this.soundSource.PlayOneShot(audio.clip, audio.volume);
		}
	}
}

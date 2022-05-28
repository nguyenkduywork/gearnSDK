using System;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
	[NonSerialized]
	public Database database;

	[SerializeField]
	private Configuration configuration;

	private void Awake()
	{
		this.LoadDatabase();
	}

	private void LoadDatabase()
	{
		string text = string.Empty;
		if (!PlayerPrefs.HasKey(this.configuration.general.dataName))
		{
			text = JsonUtility.ToJson(new Database(this.configuration));
			PlayerPrefs.SetString(this.configuration.general.dataName, text);
		}
		text = PlayerPrefs.GetString(this.configuration.general.dataName);
		this.database = JsonUtility.FromJson<Database>(text);
	}

	private void SaveDatabase()
	{
		this.database.restaurant[this.database.targetRestaurant].dateTime = DateTime.Now.ToString();
		string value = JsonUtility.ToJson(this.database);
		PlayerPrefs.SetString(this.configuration.general.dataName, value);
	}

	private void OnApplicationQuit()
	{
		this.SaveDatabase();
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			this.SaveDatabase();
		}
	}
}

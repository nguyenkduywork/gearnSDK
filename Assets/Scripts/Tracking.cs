using Firebase.Analytics;
using System;
using UnityEngine;

public class Tracking : MonoBehaviour
{
	public static Tracking instance;

	private bool first;

	private void Awake()
	{
		if (Tracking.instance == null)
		{
			Tracking.instance = this;
		}
		else if (Tracking.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		this.first = !PlayerPrefs.HasKey("first_session");
		if (this.first)
		{
			PlayerPrefs.SetInt("first_session", 1);
		}
	}

	public void Tutorial_Start(string step)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", step),
			new Parameter("action_type", "game"),
			new Parameter("value", string.Empty)
		};
		FirebaseAnalytics.LogEvent("tutorial", parameters);
	}

	public void Tutorial_Done(string step)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", step),
			new Parameter("action_type", "user"),
			new Parameter("value", "completed")
		};
		FirebaseAnalytics.LogEvent("tutorial", parameters);
	}

	public void UI_Interaction(string position, string action)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", position),
			new Parameter("action_type", "user"),
			new Parameter("value", action),
			new Parameter("status_user_first_session", (!this.first) ? "0" : "1")
		};
		FirebaseAnalytics.LogEvent("ui_interaction", parameters);
	}

	public void Ads_Impress(string adsTYPE, string position)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", adsTYPE),
			new Parameter("action_type", "game"),
			new Parameter("value", string.Empty),
			new Parameter("status_game_Ad_position", position)
		};
		FirebaseAnalytics.LogEvent("ads_impress", parameters);
	}

	public void Ads_Status(string adsTYPE, string action, string position, string status)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", adsTYPE),
			new Parameter("action_type", action),
			new Parameter("status_game_Ad_position", position),
			new Parameter("status_ads", status)
		};
		FirebaseAnalytics.LogEvent("ads_status", parameters);
	}

	public void IAP(string product)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", product),
			new Parameter("action_type", "user"),
			new Parameter("value", "purchase")
		};
		FirebaseAnalytics.LogEvent("iap", parameters);
	}

	public void Rate_Show()
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", "show"),
			new Parameter("action_type", "game"),
			new Parameter("value", string.Empty)
		};
		FirebaseAnalytics.LogEvent("rate", parameters);
	}

	public void Rate_Action(string action)
	{
		Parameter[] parameters = new Parameter[]
		{
			new Parameter("action_name", "show"),
			new Parameter("action_type", "user"),
			new Parameter("value", action)
		};
		FirebaseAnalytics.LogEvent("rate", parameters);
	}
}

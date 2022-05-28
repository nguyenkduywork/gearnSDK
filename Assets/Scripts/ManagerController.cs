using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ManagerController : MonoBehaviour
{
	private sealed class _Cooldown_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ManagerController _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _Cooldown_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.boostEffect.SetActive(false);
				break;
			case 1u:
				if (this._this.managerProfile.remainingTime > 0)
				{
					this._this.managerProfile.remainingTime--;
				}
				break;
			default:
				return false;
			}
			if (this._this.managerProfile.remainingTime > 0)
			{
				GameUtilities.String.ToText(this._this.timeText, GameUtilities.DateTime.Convert(this._this.managerProfile.remainingTime));
				this._current = this._this.waitForSeconds;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.activeButton.SetActive(true);
			this._this.managerProfile.state = ManagerState.Ready;
			GameUtilities.String.ToText(this._this.timeText, string.Empty);
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _Boosting_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ManagerController _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _Boosting_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.boostEffect.SetActive(true);
				break;
			case 1u:
				if (this._this.managerProfile.remainingTime > 0)
				{
					this._this.managerProfile.remainingTime--;
				}
				break;
			default:
				return false;
			}
			if (this._this.managerProfile.remainingTime > 0)
			{
				GameUtilities.String.ToText(this._this.timeText, GameUtilities.DateTime.Convert(this._this.managerProfile.remainingTime));
				this._current = this._this.waitForSeconds;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.boostController.Refresh();
			this._this.managerProfile.state = ManagerState.Cooldown;
			this._this.managerProfile.lastActive = DateTime.Now.ToString();
			this._this.managerProfile.remainingTime = Singleton<GameProcess>.Instance.GetManagerSkillCooldown(this._this.managerProfile.experience, this._this.managerProfile.skill);
			if (this._this.managerProfile.skill == ManagerSkill.UpgradeCost)
			{
				Singleton<GameManager>.Instance.onCashChange(Singleton<GameManager>.Instance.database.cash);
			}
			this._this.cooldown = this._this.StartCoroutine(this._this.Cooldown());
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public Text timeText;

	public Image skillSprite;

	public GameObject activeButton;

	public GameObject emptyManager;

	public GameObject targetManager;

	public GameObject boostEffect;

	public BoostController boostController;

	public SkeletonGraphic skeletonGraphic;

	[HideInInspector]
	public ManagerProfile managerProfile;

	public bool hasManager;

	public Action managerAssign;

	private Coroutine cooldown;

	private Coroutine boosting;

	private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

	public void ManagerAssign()
	{
		this.hasManager = true;
		this.emptyManager.SetActive(false);
		this.targetManager.SetActive(true);
		Experience experience = this.managerProfile.experience;
		if (experience != Experience.Junior)
		{
			if (experience != Experience.Senior)
			{
				if (experience == Experience.Expert)
				{
					this.skeletonGraphic.Skeleton.SetSkin("Manager03");
				}
			}
			else
			{
				this.skeletonGraphic.Skeleton.SetSkin("Manager02");
			}
		}
		else
		{
			this.skeletonGraphic.Skeleton.SetSkin("Manager01");
		}
		this.skeletonGraphic.Skeleton.SetToSetupPose();
		this.skillSprite.sprite = Singleton<GameProcess>.Instance.GetManagerSkillSprite(this.managerProfile.skill, true);
		int num = GameUtilities.DateTime.Offline(this.managerProfile.lastActive);
		if (this.managerProfile.state == ManagerState.Cooldown && this.managerProfile.remainingTime > 0)
		{
			int managerSkillCooldown = Singleton<GameProcess>.Instance.GetManagerSkillCooldown(this.managerProfile.experience, this.managerProfile.skill);
			this.managerProfile.remainingTime = managerSkillCooldown - num;
			if (this.managerProfile.remainingTime <= 0)
			{
				this.managerProfile.remainingTime = 0;
				this.managerProfile.state = ManagerState.Ready;
			}
			else
			{
				this.cooldown = base.StartCoroutine(this.Cooldown());
			}
		}
		if (this.managerProfile.state == ManagerState.Boosting && this.managerProfile.remainingTime > 0)
		{
			int managerSkillDuration = Singleton<GameProcess>.Instance.GetManagerSkillDuration(this.managerProfile.experience, this.managerProfile.skill);
			this.managerProfile.remainingTime = managerSkillDuration - num;
			if (this.managerProfile.remainingTime <= 0)
			{
				this.managerProfile.remainingTime = Singleton<GameProcess>.Instance.GetManagerSkillCooldown(this.managerProfile.experience, this.managerProfile.skill);
				this.managerProfile.state = ManagerState.Cooldown;
				this.managerProfile.remainingTime += managerSkillDuration - num;
				if (this.managerProfile.remainingTime <= 0)
				{
					this.managerProfile.remainingTime = 0;
					this.managerProfile.state = ManagerState.Ready;
				}
				else
				{
					this.cooldown = base.StartCoroutine(this.Cooldown());
				}
			}
			else
			{
				this.ActiveSkill();
				this.boosting = base.StartCoroutine(this.Boosting());
			}
		}
		this.activeButton.SetActive(this.managerProfile.state == ManagerState.Ready);
		if (this.managerAssign != null)
		{
			this.managerAssign();
		}
	}

	public void ManagerUnassign()
	{
		this.hasManager = false;
		this.boostController.Refresh();
		this.emptyManager.SetActive(true);
		this.boostEffect.SetActive(false);
		this.targetManager.SetActive(false);
		if (this.cooldown != null)
		{
			base.StopCoroutine(this.cooldown);
		}
		if (this.boosting != null)
		{
			base.StopCoroutine(this.boosting);
		}
		if (this.managerProfile.state == ManagerState.Boosting)
		{
			this.managerProfile.state = ManagerState.Cooldown;
			this.managerProfile.lastActive = DateTime.Now.ToString();
			this.managerProfile.remainingTime = Singleton<GameProcess>.Instance.GetManagerSkillCooldown(this.managerProfile.experience, this.managerProfile.skill);
		}
		if (this.managerProfile.skill == ManagerSkill.UpgradeCost)
		{
			Singleton<GameManager>.Instance.onCashChange(Singleton<GameManager>.Instance.database.cash);
		}
		GameUtilities.String.ToText(this.timeText, string.Empty);
		this.managerProfile = null;
	}

	public void ManagerActivate()
	{
		this.activeButton.SetActive(false);
		this.managerProfile.state = ManagerState.Boosting;
		this.managerProfile.lastActive = DateTime.Now.ToString();
		this.managerProfile.remainingTime = Singleton<GameProcess>.Instance.GetManagerSkillDuration(this.managerProfile.experience, this.managerProfile.skill);
		this.ActiveSkill();
		Singleton<SoundManager>.Instance.Play("Boost");
		this.boosting = base.StartCoroutine(this.Boosting());
	}

	private void ActiveSkill()
	{
		int managerSkillEffective = Singleton<GameProcess>.Instance.GetManagerSkillEffective(this.managerProfile.experience, this.managerProfile.skill);
		switch (this.managerProfile.skill)
		{
		case ManagerSkill.UpgradeCost:
			this.boostController.upgradeCostReduced = managerSkillEffective;
			Singleton<GameManager>.Instance.onCashChange(Singleton<GameManager>.Instance.database.cash);
			break;
		case ManagerSkill.WalkingSpeed:
			this.boostController.walkingSpeedBoost = (float)managerSkillEffective;
			break;
		case ManagerSkill.CookingSpeed:
			this.boostController.cookingSpeedBoost = (float)managerSkillEffective;
			break;
		case ManagerSkill.LoadingSpeed:
			this.boostController.loadingSpeedBoost = (float)managerSkillEffective;
			break;
		case ManagerSkill.MovementSpeed:
			this.boostController.movementSpeedBoost = (float)managerSkillEffective;
			break;
		case ManagerSkill.LoadExpansion:
			this.boostController.loadExpansionBoost = (float)managerSkillEffective;
			break;
		}
		if (this.managerProfile.skill == ManagerSkill.UpgradeCost)
		{
			Singleton<GameManager>.Instance.onCashChange(Singleton<GameManager>.Instance.database.cash);
		}
	}

	private IEnumerator Cooldown()
	{
		ManagerController._Cooldown_c__Iterator0 _Cooldown_c__Iterator = new ManagerController._Cooldown_c__Iterator0();
		_Cooldown_c__Iterator._this = this;
		return _Cooldown_c__Iterator;
	}

	private IEnumerator Boosting()
	{
		ManagerController._Boosting_c__Iterator1 _Boosting_c__Iterator = new ManagerController._Boosting_c__Iterator1();
		_Boosting_c__Iterator._this = this;
		return _Boosting_c__Iterator;
	}
}

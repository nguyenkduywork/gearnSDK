using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour
{

	private sealed class _Transporting_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _i___1;

		internal Vector3 _target___2;

		internal float _timing___2;

		internal float _timing___3;

		internal ElevatorController _this;

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

		public _Transporting_c__Iterator0()
		{
		}

		public bool MoveNext()
		{

           
            uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.RefreshTransportData();
				this._this.movementSpeed = this._this.elevatorProperties.movementSpeed * this._this.boostController.movementSpeedBoost;
				this._i___1 = 0;
				goto IL_540;
			case 1u:
				//IL_13A:
				if (this._this.targetCabin.position.y != this._this.kitchenController[this._i___1].transform.position.y)
				{
					this._target___2 = new Vector3(this._this.targetCabin.position.x, this._this.kitchenController[this._i___1].transform.position.y, this._this.targetCabin.position.z);
					this._this.targetCabin.position = Vector3.MoveTowards(this._this.targetCabin.position, this._target___2, Time.deltaTime * this._this.movementSpeed);
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				this._this.cashBeforeTransfer = this._this.kitchenController[this._i___1].kitchenData.cash;
				if (this._this.cashBeforeTransfer == 0.0)
				{
					goto IL_532;
				}
				this._this.transferDuration = ((this._this.totalCashTransport + this._this.cashBeforeTransfer <= this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost) ? ((float)(this._this.cashBeforeTransfer / (this._this.elevatorProperties.loadingSpeed * (double)this._this.boostController.loadingSpeedBoost))) : ((float)((this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost - this._this.totalCashTransport) / (this._this.elevatorProperties.loadingSpeed * (double)this._this.boostController.loadingSpeedBoost))));
				this._timing___2 = 0f;
				this._this.progress.SetActive(true);
				break;
			case 2u:
				break;
			case 3u:
				//IL_5ED:
				if (this._this.targetCabin.position != this._this.cabinPoint.position)
				{
					this._this.targetCabin.position = Vector3.MoveTowards(this._this.targetCabin.position, this._this.cabinPoint.position, Time.deltaTime * this._this.movementSpeed);
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 3;
					}
					return true;
				}
				if (this._this.totalCashTransport > 0.0)
				{
					this._this.transferDuration = (float)(this._this.totalCashTransport / (this._this.elevatorProperties.loadingSpeed * (double)this._this.boostController.loadingSpeedBoost));
					this._timing___3 = 0f;
					this._this.progress.SetActive(true);
					goto IL_6DA;
				}
				goto IL_722;
			case 4u:
				goto IL_6DA;
			default:
				return false;
			}
			if (this._timing___2 < this._this.transferDuration)
			{
				this._this.processFill.fillAmount = this._timing___2 / this._this.transferDuration;
				this._timing___2 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.progress.SetActive(false);
			this._this.cashAfterTransfer = this._this.kitchenController[this._i___1].kitchenData.cash;
			if (this._this.totalCashTransport + this._this.cashAfterTransfer <= this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost)
			{
				this._this.kitchenController[this._i___1].SetCash(-this._this.cashAfterTransfer);
			}
			else
			{
				this._this.kitchenController[this._i___1].SetCash(-(this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost - this._this.totalCashTransport));
			}
			if (this._this.totalCashTransport + this._this.cashAfterTransfer > this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost)
			{
				this._this.totalCashTransport = this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost;
			}
			else
			{
				this._this.totalCashTransport = this._this.totalCashTransport + this._this.cashAfterTransfer;
			}
			this._this.FillProduct((float)(this._this.totalCashTransport / (this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost)));
			GameUtilities.String.ToText(this._this.cabinText, GameUtilities.Currencies.Convert(this._this.totalCashTransport));
			if (this._this.totalCashTransport == this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost)
			{
				goto IL_55B;
			}
			IL_532:
			this._i___1++;
			IL_540:
			if (this._i___1 < this._this.kitchenController.Count)
			{
				this._target___2 = Vector3.zero;
				goto IL_13A;
			}
			IL_55B:
			this._this.movementSpeed = this._this.elevatorProperties.movementSpeed * this._this.boostController.movementSpeedBoost;
			goto IL_5ED;
			IL_6DA:
			if (this._timing___3 < this._this.transferDuration)
			{
				this._this.processFill.fillAmount = this._timing___3 / this._this.transferDuration;
				this._timing___3 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 4;
				}
				return true;
			}
			this._this.progress.SetActive(false);
			this._this.SetCash(this._this.totalCashTransport);
			this._this.RefreshTransportData();
			IL_722:
			if (!this._this.managerController.hasManager)
			{
				this._this.transporting = false;
			}
			else
			{
				this._this.StartCoroutine(this._this.Transporting());
			}
			this._PC = -1;
			return false;



        IL_13A:
            if (this._this.targetCabin.position.y != this._this.kitchenController[this._i___1].transform.position.y)
            {
                this._target___2 = new Vector3(this._this.targetCabin.position.x, this._this.kitchenController[this._i___1].transform.position.y, this._this.targetCabin.position.z);
                this._this.targetCabin.position = Vector3.MoveTowards(this._this.targetCabin.position, this._target___2, Time.deltaTime * this._this.movementSpeed);
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 1;
                }
                return true;
            }
            this._this.cashBeforeTransfer = this._this.kitchenController[this._i___1].kitchenData.cash;
            if (this._this.cashBeforeTransfer == 0.0)
            {
                goto IL_532;
            }
            this._this.transferDuration = ((this._this.totalCashTransport + this._this.cashBeforeTransfer <= this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost) ? ((float)(this._this.cashBeforeTransfer / (this._this.elevatorProperties.loadingSpeed * (double)this._this.boostController.loadingSpeedBoost))) : ((float)((this._this.elevatorProperties.load * (double)this._this.boostController.loadExpansionBoost - this._this.totalCashTransport) / (this._this.elevatorProperties.loadingSpeed * (double)this._this.boostController.loadingSpeedBoost))));
            this._timing___2 = 0f;
            this._this.progress.SetActive(true);


        IL_5ED:
            if (this._this.targetCabin.position != this._this.cabinPoint.position)
            {
                this._this.targetCabin.position = Vector3.MoveTowards(this._this.targetCabin.position, this._this.cabinPoint.position, Time.deltaTime * this._this.movementSpeed);
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 3;
                }
                return true;
            }
            if (this._this.totalCashTransport > 0.0)
            {
                this._this.transferDuration = (float)(this._this.totalCashTransport / (this._this.elevatorProperties.loadingSpeed * (double)this._this.boostController.loadingSpeedBoost));
                this._timing___3 = 0f;
                this._this.progress.SetActive(true);
                goto IL_6DA;
            }
            goto IL_722;
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

	public Text cashText;

	public Text cabinText;

	public Text levelText;

	public Image processFill;

	public Transform cabinPoint;

	public Transform targetCabin;

	public GameObject productFX;

	public GameObject progress;

	public GameObject[] levelUp;

	public RectTransform productFill;

	public ElevatorPopup elevatorPopup;

	public BoostController boostController;

	public ManagerController managerController;

	public GameObject tutorial_3;

	public GameObject tutorial_4;

	[HideInInspector]
	public ElevatorData elevatorData;

	[HideInInspector]
	public ElevatorProperties elevatorProperties;

	[HideInInspector]
	public List<KitchenController> kitchenController;

	private int processCount;

	private bool transporting;

	private float movementSpeed;

	private float transferDuration;

	private double cashAfterTransfer;

	private double cashBeforeTransfer;

	private double totalCashTransport;

	public void Initialize()
	{
		this.managerController.managerAssign = new Action(this.StartTransport);
		GameManager expr_1C = Singleton<GameManager>.Instance;
		expr_1C.onCashChange = (Action<double>)Delegate.Combine(expr_1C.onCashChange, new Action<double>(this.OnCashChange));
		float distance = 2f * (float)((this.kitchenController.Count <= 0) ? 1 : this.kitchenController.Count);
		this.elevatorProperties = Singleton<GameProcess>.Instance.GetElevatorProperties(distance, this.elevatorData.level);
		GameUtilities.String.ToText(this.cashText, GameUtilities.Currencies.Convert(this.elevatorData.cash));
		GameUtilities.String.ToText(this.levelText, "Level \n" + this.elevatorData.level.ToString());
		this.boostController.Refresh();
		this.FillProduct(0f);
		this.tutorial_3.SetActive(!GameManager.IsDoneTutorial(3) && GameManager.IsDoneTutorial(2));
		this.tutorial_4.SetActive(!GameManager.IsDoneTutorial(4) && GameManager.IsDoneTutorial(3));
	}

	public void ShowManagerProfile()
	{
		Singleton<ManagerPopup>.Instance.Show(this);
	}

	public void ShowElevatorProperties()
	{
		this.elevatorPopup.Show();
	}

	public void SetCash(double cash)
	{
		this.elevatorData.cash += cash;
		GameUtilities.String.ToText(this.cashText, GameUtilities.Currencies.Convert(this.elevatorData.cash));
		if (!GameManager.IsDoneTutorial(4) && cash > 0.0)
		{
			this.tutorial_4.SetActive(true);
			Tracking.instance.Tutorial_Start("Step4");
		}
	}

	public void Upgrade()
	{
		float distance = 2f * (float)((this.kitchenController.Count <= 0) ? 1 : this.kitchenController.Count);
		this.elevatorProperties = Singleton<GameProcess>.Instance.GetElevatorProperties(distance, this.elevatorData.level);
		this.levelText.text = "Level \n" + this.elevatorData.level.ToString();
	}

	public void StartTransport()
	{
		if (this.kitchenController.Count == 0 || this.transporting)
		{
			return;
		}
		this.transporting = true;
		base.StartCoroutine(this.Transporting());
		if (!GameManager.IsDoneTutorial(3) && GameManager.IsDoneTutorial(2))
		{
			GameManager.TutorialDone(3);
			this.tutorial_3.SetActive(false);
			Tracking.instance.Tutorial_Done("Step3");
		}
	}

	public void Tutorial_3()
	{
		this.tutorial_3.SetActive(true);
		Tracking.instance.Tutorial_Start("Step3");
	}

	public void Process(int value)
	{
		this.processCount += value;
		if (this.processCount < 0)
		{
			this.processCount = 0;
		}
		if (this.processCount > 0)
		{
			this.productFX.SetActive(true);
		}
		else
		{
			this.productFX.SetActive(false);
		}
	}

	private IEnumerator Transporting()
	{
		ElevatorController._Transporting_c__Iterator0 _Transporting_c__Iterator = new ElevatorController._Transporting_c__Iterator0();
		_Transporting_c__Iterator._this = this;
		return _Transporting_c__Iterator;
	}

	private void RefreshTransportData()
	{
		this.cashAfterTransfer = 0.0;
		this.cashBeforeTransfer = 0.0;
		this.totalCashTransport = 0.0;
		this.FillProduct(0f);
		GameUtilities.String.ToText(this.cabinText, "0");
	}

	private void OnCashChange(double cash)
	{
		int maxUpgradeLevel = Singleton<GameProcess>.Instance.GetMaxUpgradeLevel(cash, this.boostController.upgradeCostReduced, this.elevatorData.level, Location.Elevator, 0);
		this.levelUp[0].SetActive(maxUpgradeLevel > 0);
		this.levelUp[1].SetActive(maxUpgradeLevel > 9);
		this.levelUp[2].SetActive(maxUpgradeLevel >= 50);
	}

	private void FillProduct(float value)
	{
		float num = 116f * value;
		if (num > 0f)
		{
			num = Mathf.Clamp(num, 30f, 116f);
		}
		this.productFill.sizeDelta = new Vector2(this.productFill.sizeDelta.x, num);
	}
}

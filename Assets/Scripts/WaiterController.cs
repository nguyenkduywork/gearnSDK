using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WaiterController : MonoBehaviour
{

	private sealed class _Transport_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal WaiterController _this;

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

		public _Transport_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.isIdle = false;
				this._this.ApplyAnimationSpeed("Run_01", this._this.restaurantController.boostController.walkingSpeedBoost);
				this._this.totalCashTransfer = 0.0;
				break;
			case 1u:
				break;
			case 2u:
				this._this.cashAfterTransfer = this._this.elevatorController.elevatorData.cash;
				if (this._this.cashAfterTransfer > 0.0)
				{
					this._this.totalCashTransfer = ((this._this.cashAfterTransfer <= this._this.restaurantController.restaurantProperties.loadPerWaiter * (double)this._this.restaurantController.boostController.loadExpansionBoost) ? this._this.cashAfterTransfer : (this._this.restaurantController.restaurantProperties.loadPerWaiter * (double)this._this.restaurantController.boostController.loadExpansionBoost));
					this._this.elevatorController.SetCash(-this._this.totalCashTransfer);
					this._this.cashText.gameObject.SetActive(true);
					GameUtilities.String.ToText(this._this.cashText, GameUtilities.Currencies.Convert(this._this.totalCashTransfer));
				}
				this._this.elevatorController.Process(-1);
				goto IL_383;
			case 3u:
				//IL_456:
				if (this._this.myselfTransform.localPosition != this._this.gatheringPoint)
				{
					this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.gatheringPoint, Time.deltaTime * this._this.walkingSpeed);
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 3;
					}
					return true;
				}
				if (this._this.totalCashTransfer > 0.0)
				{
					this._this.ApplyAnimationSpeed("Idle_02", 1f);
					this._current = new WaitForSeconds((float)(this._this.totalCashTransfer / (this._this.restaurantController.restaurantProperties.loadingSpeed * (double)this._this.restaurantController.boostController.loadingSpeedBoost)));
					if (!this._disposing)
					{
						this._PC = 4;
					}
					return true;
				}
				goto IL_547;
			case 4u:
				this._this.restaurantController.SetCash(this._this.totalCashTransfer);
				GameUtilities.String.ToText(this._this.cashText, string.Empty);
				this._this.cashText.gameObject.SetActive(false);
				goto IL_547;
			case 5u:
				//IL_5F7:
				if (!(this._this.myselfTransform.localPosition != this._this.restingPosition))
				{
					if (!this._this.restaurantController.managerController.hasManager)
					{
						this._this.animator.AnimationState.SetAnimation(0, "Idle_01", true);
						this._this.isIdle = true;
					}
					else
					{
						this._this.StartCoroutine(this._this.Transport());
					}
					this._PC = -1;
					return false;
				}
				this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.restingPosition, Time.deltaTime * this._this.walkingSpeed);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 5;
				}
				return true;
			default:
				return false;
			}
			if (this._this.myselfTransform.localPosition != this._this.exploitedPoint)
			{
				this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.exploitedPoint, Time.deltaTime * this._this.walkingSpeed);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.cashBeforeTransfer = this._this.elevatorController.elevatorData.cash;
			if (this._this.cashBeforeTransfer > 0.0)
			{
				this._this.elevatorController.Process(1);
				this._this.ApplyAnimationSpeed("Idle_02", 1f);
				this._this.loadingTime = ((this._this.cashBeforeTransfer > this._this.restaurantController.restaurantProperties.loadPerWaiter * (double)this._this.restaurantController.boostController.loadExpansionBoost) ? ((float)(this._this.restaurantController.restaurantProperties.loadPerWaiter * (double)this._this.restaurantController.boostController.loadExpansionBoost / (this._this.restaurantController.restaurantProperties.loadingSpeed * (double)this._this.restaurantController.boostController.loadingSpeedBoost))) : ((float)(this._this.cashBeforeTransfer / (this._this.restaurantController.restaurantProperties.loadingSpeed * (double)this._this.restaurantController.boostController.loadingSpeedBoost))));
				this._current = new WaitForSeconds(this._this.loadingTime);
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			IL_383:
			this._this.animatorTransform.eulerAngles += Vector3.up * 180f;
			this._this.ApplyAnimationSpeed((this._this.totalCashTransfer <= 0.0) ? "Run_01" : "Run_02", this._this.restaurantController.boostController.walkingSpeedBoost);
			goto IL_456;
			IL_547:
			this._this.animatorTransform.eulerAngles += Vector3.up * 180f;
			this._this.ApplyAnimationSpeed("Run_01", this._this.restaurantController.boostController.walkingSpeedBoost);
			goto IL_5F7;

           IL_456:
            if (this._this.myselfTransform.localPosition != this._this.gatheringPoint)
            {
                this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.gatheringPoint, Time.deltaTime * this._this.walkingSpeed);
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 3;
                }
                return true;
            }
            if (this._this.totalCashTransfer > 0.0)
            {
                this._this.ApplyAnimationSpeed("Idle_02", 1f);
                this._current = new WaitForSeconds((float)(this._this.totalCashTransfer / (this._this.restaurantController.restaurantProperties.loadingSpeed * (double)this._this.restaurantController.boostController.loadingSpeedBoost)));
                if (!this._disposing)
                {
                    this._PC = 4;
                }
                return true;
            }
            goto IL_547;

          IL_5F7:
            if (!(this._this.myselfTransform.localPosition != this._this.restingPosition))
            {
                if (!this._this.restaurantController.managerController.hasManager)
                {
                    this._this.animator.AnimationState.SetAnimation(0, "Idle_01", true);
                    this._this.isIdle = true;
                }
                else
                {
                    this._this.StartCoroutine(this._this.Transport());
                }
                this._PC = -1;
                return false;
            }
            this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.restingPosition, Time.deltaTime * this._this.walkingSpeed);
            this._current = null;
            if (!this._disposing)
            {
                this._PC = 5;
            }
            return true;
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

	private SkeletonGraphic animator;

	private Vector3 gatheringPoint;

	private Vector3 exploitedPoint;

	private Vector3 restingPosition;

	private Transform myselfTransform;

	private Transform animatorTransform;

	private ElevatorController elevatorController;

	private RestaurantController restaurantController;

	private float loadingTime;

	private float walkingSpeed;

	private double totalCashTransfer;

	private double cashAfterTransfer;

	private double cashBeforeTransfer;

	public bool isIdle = true;

	public Text cashText;

	public float movement;

	private void Awake()
	{
		this.myselfTransform = base.transform;
		this.animator = base.GetComponentInChildren<SkeletonGraphic>();
		this.animatorTransform = this.animator.transform;
	}

	public void Initialize(ElevatorController elevatorController, RestaurantController restaurantController)
	{
		this.elevatorController = elevatorController;
		this.restaurantController = restaurantController;
		this.restingPosition = base.transform.localPosition;
		this.gatheringPoint = restaurantController.gatheringPoint.localPosition;
		this.exploitedPoint = restaurantController.exploitedPoint.localPosition;
	}

	public void StartTransport()
	{
		if (!this.isIdle)
		{
			return;
		}
		base.StartCoroutine(this.Transport());
	}

	private IEnumerator Transport()
	{
		WaiterController._Transport_c__Iterator0 _Transport_c__Iterator = new WaiterController._Transport_c__Iterator0();
		_Transport_c__Iterator._this = this;
		return _Transport_c__Iterator;
	}

	private void ApplyAnimationSpeed(string clip, float speed = 1f)
	{
		if (clip.Equals("Run_01") || clip.Equals("Run_02"))
		{
			this.walkingSpeed = this.restaurantController.restaurantProperties.walkingSpeed * speed * this.movement;
		}
		this.animator.timeScale = speed;
		this.animator.AnimationState.SetAnimation(0, clip, true);
	}
}

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TransporterController : MonoBehaviour
{
	private sealed class _Working_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal TransporterController _this;

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

		public _Working_c__Iterator0()
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
				this._this.ApplyAnimationSpeed("Run_01", this._this.kitchenController.boostController.walkingSpeedBoost);
				break;
			case 1u:
				break;
			case 2u:
				this._this.animatorTransform.eulerAngles += Vector3.up * 180f;
				this._this.ApplyAnimationSpeed("Run_02", this._this.kitchenController.boostController.walkingSpeedBoost);
				goto IL_1E3;
			case 3u:
				goto IL_1E3;
			case 4u:
				goto IL_2DD;
			default:
				return false;
			}
			if (!(this._this.myselfTransform.localPosition != this._this.exploitedPoint))
			{
				this._this.ApplyAnimationSpeed("Idle_02", this._this.kitchenController.boostController.cookingSpeedBoost);
				this._current = new WaitForSeconds(this._this.cookingTime);
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.exploitedPoint, Time.deltaTime * this._this.walkingSpeed);
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
			IL_1E3:
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
			this._this.animatorTransform.eulerAngles += Vector3.up * 180f;
			this._this.kitchenController.SetCash(this._this.kitchenController.kitchenProperties.transporterCapacity);
			this._this.ApplyAnimationSpeed("Run_01", this._this.kitchenController.boostController.walkingSpeedBoost);
			IL_2DD:
			if (this._this.myselfTransform.localPosition != this._this.restingPosition)
			{
				this._this.myselfTransform.localPosition = Vector3.MoveTowards(this._this.myselfTransform.localPosition, this._this.restingPosition, Time.deltaTime * this._this.walkingSpeed);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 4;
				}
				return true;
			}
			if (!this._this.kitchenController.managerController.hasManager)
			{
				this._this.ApplyAnimationSpeed("Idle_01", 1f);
				this._this.isIdle = true;
			}
			else
			{
				this._this.StartCoroutine(this._this.Working());
			}
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

	private SkeletonGraphic animator;

	private Vector3 gatheringPoint;

	private Vector3 exploitedPoint;

	private Vector3 restingPosition;

	private Transform myselfTransform;

	private Transform animatorTransform;

	private KitchenController kitchenController;

	private float cookingTime;

	private float walkingSpeed;

	public float movement;

	public bool isIdle = true;

	private void Awake()
	{
		this.myselfTransform = base.transform;
		this.animator = base.GetComponentInChildren<SkeletonGraphic>();
		this.animatorTransform = this.animator.transform;
	}

	public void Initialize(KitchenController kitchenController)
	{
		this.kitchenController = kitchenController;
		this.restingPosition = base.transform.localPosition;
		this.exploitedPoint = kitchenController.exploitedPoint.localPosition;
		this.gatheringPoint = kitchenController.gatheringPoint.localPosition;
	}

	public void StartWorking()
	{
		if (!this.isIdle)
		{
			return;
		}
		base.StartCoroutine(this.Working());
	}

	private IEnumerator Working()
	{
		TransporterController._Working_c__Iterator0 _Working_c__Iterator = new TransporterController._Working_c__Iterator0();
		_Working_c__Iterator._this = this;
		return _Working_c__Iterator;
	}

	private void ApplyAnimationSpeed(string clip, float speed = 1f)
	{
		if (clip != null)
		{
			if (!(clip == "Run_01") && !(clip == "Run_02"))
			{
				if (clip == "Idle_02")
				{
					this.cookingTime = (float)(this.kitchenController.kitchenProperties.transporterCapacity / (this.kitchenController.kitchenProperties.workingSpeed * (double)speed));
				}
			}
			else
			{
				this.walkingSpeed = this.kitchenController.kitchenProperties.walkingSpeed * speed * this.movement;
			}
		}
		this.animator.timeScale = speed;
		this.animator.AnimationState.SetAnimation(0, clip, true);
	}
}

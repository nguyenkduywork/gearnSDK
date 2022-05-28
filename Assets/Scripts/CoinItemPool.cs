using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoinItemPool : MonoBehaviour
{
	private sealed class _Spawn_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _count___0;

		internal float _x___1;

		internal float _y___1;

		internal GameObject _coinItem___1;

		internal Transform target;

		internal double cash;

		internal CoinItemPool _this;

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

		public _Spawn_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._count___0 = this._this.itemCount;
				break;
			case 1u:
				this._count___0--;
				break;
			default:
				return false;
			}
			if (this._count___0 > 0)
			{
				this._x___1 = UnityEngine.Random.Range(this._this.transform.position.x - this._this.randomRadius, this._this.transform.position.x + this._this.randomRadius);
				this._y___1 = UnityEngine.Random.Range(this._this.transform.position.y - this._this.randomRadius, this._this.transform.position.y + this._this.randomRadius);
				this._coinItem___1 = ObjectPool.Spawn(this._this.coinPrefab, this._this.transform.position, Quaternion.identity);
				this._coinItem___1.transform.SetParent(this._this.coinParent);
				this._coinItem___1.transform.localScale = Vector3.one;
				this._coinItem___1.GetComponent<CoinItem>().Init(new Vector3[]
				{
					new Vector3(this._x___1, this._y___1, 0f),
					this.target.position
				}, Math.Round(this.cash / (double)this._this.itemCount));
				this._current = this._this.waitForSeconds;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
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

	private WaitForSeconds waitForSeconds;

	[SerializeField]
	private int itemCount;

	[SerializeField]
	private float randomRadius;

	[SerializeField]
	private float spawnDeltaTime;

	[SerializeField]
	private Transform coinParent;

	[SerializeField]
	private GameObject coinPrefab;

	private void Start()
	{
		this.waitForSeconds = new WaitForSeconds(this.spawnDeltaTime);
	}

	public void Pool(Transform target, double cash)
	{
		Singleton<SoundManager>.Instance.Play("Collect");
		base.StartCoroutine(this.Spawn(target, cash));
	}

	private IEnumerator Spawn(Transform target, double cash)
	{
		CoinItemPool._Spawn_c__Iterator0 _Spawn_c__Iterator = new CoinItemPool._Spawn_c__Iterator0();
		_Spawn_c__Iterator.target = target;
		_Spawn_c__Iterator.cash = cash;
		_Spawn_c__Iterator._this = this;
		return _Spawn_c__Iterator;
	}
}

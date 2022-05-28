using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class JumpCash : MonoBehaviour
{
	private sealed class _DestroySelf_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float time;

		internal float _timing___0;

		internal JumpCash _this;

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

		public _DestroySelf_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timing___0 = this.time;
				break;
			case 1u:
				this._timing___0 -= Time.deltaTime;
				break;
			default:
				return false;
			}
			if (this._timing___0 > 0f)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			ObjectPool.Despawn(this._this.gameObject);
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

	[SerializeField]
	private Text cashText;

	[SerializeField]
	private Animation anim;

	public void Init(double cash)
	{
		GameUtilities.String.ToText(this.cashText, GameUtilities.Currencies.Convert(cash));
		this.DestroySelf(this.anim.clip.length);
	}

	private IEnumerator DestroySelf(float time)
	{
		JumpCash._DestroySelf_c__Iterator0 _DestroySelf_c__Iterator = new JumpCash._DestroySelf_c__Iterator0();
		_DestroySelf_c__Iterator.time = time;
		_DestroySelf_c__Iterator._this = this;
		return _DestroySelf_c__Iterator;
	}
}

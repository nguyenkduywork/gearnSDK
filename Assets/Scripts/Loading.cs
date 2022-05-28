using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	private sealed class _Start_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal Loading _this;

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

		public _Start_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.asyncOperation = SceneManager.LoadSceneAsync("Game");
				this._this.asyncOperation.allowSceneActivation = false;
				this._timer___0 = 0f;
				break;
			case 1u:
				this._timer___0 += Time.deltaTime;
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.loadingTime)
			{
				GameUtilities.String.ToText(this._this.progressText, Mathf.RoundToInt(this._timer___0 / this._this.loadingTime * 100f).ToString() + "%");
				this._this.progressFill.fillAmount = this._timer___0 / this._this.loadingTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.asyncOperation.allowSceneActivation = true;
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

	private AsyncOperation asyncOperation;

	[SerializeField]
	private float loadingTime;

	[SerializeField]
	private Text progressText;

	[SerializeField]
	private Image progressFill;

	private IEnumerator Start()
	{
        Loading._Start_c__Iterator0 _Start_c__Iterator = new Loading._Start_c__Iterator0();
		_Start_c__Iterator._this = this;
		return _Start_c__Iterator;
	}
}

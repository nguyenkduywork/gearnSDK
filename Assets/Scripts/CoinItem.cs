using System;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
	private double cash;

	private int nodeIndex;

	private bool movement;

	private Vector3[] path;

	[SerializeField]
	private float movementSpeed;

	public void Init(Vector3[] path, double cash)
	{
		this.path = path;
		this.cash = cash;
		this.movement = true;
	}

	private void Update()
	{
		if (!this.movement)
		{
			return;
		}
		base.transform.position = Vector3.Slerp(base.transform.position, this.path[this.nodeIndex], Time.deltaTime * this.movementSpeed);
		if (Vector3.Distance(base.transform.position, this.path[this.nodeIndex]) < 0.1f)
		{
			this.nodeIndex++;
			if (this.nodeIndex < this.path.Length)
			{
				return;
			}
			Singleton<GameManager>.Instance.SetCash(this.cash);
			Singleton<SoundManager>.Instance.Play("Cash");
			ObjectPool.Despawn(base.gameObject);
		}
	}

	private void OnDisable()
	{
		this.cash = 0.0;
		this.nodeIndex = 0;
		this.movement = false;
	}
}

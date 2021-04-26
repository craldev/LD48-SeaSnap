using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LD48.Utils
{
	public class RandomAnimationOffset : MonoBehaviour
	{
		private static readonly int offset = Animator.StringToHash("Offset");

		[SerializeField]
		private Animator animator;

		private void Awake()
		{
			var offsetValue = Random.Range(0f, 1f);
			animator.SetFloat(offset, offsetValue);
		}
	}
}
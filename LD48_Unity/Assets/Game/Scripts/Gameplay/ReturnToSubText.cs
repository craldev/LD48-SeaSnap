using UnityEngine;

namespace LD48.Gameplay
{
	public class ReturnToSubText : MonoBehaviour
	{
		[SerializeField]
		private GameObject text;

		[SerializeField]
		private float distanceThreshold = 200f;

		private void Update()
		{
			if (transform.position.x > distanceThreshold || transform.position.z > distanceThreshold)
			{
				text.SetActive(true);
			}
			else
			{
				text.SetActive(false);
			}
		}
	}
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD48.Gameplay.UI
{
	public class JournalEntryPreview : MonoBehaviour
	{
		[SerializeField]
		private RawImage picture;
		public RawImage Picture => picture;

		[SerializeField]
		private TextMeshProUGUI text;
		public TextMeshProUGUI Text => text;

		[SerializeField]
		private Journal journal;

		public Entity.Entity Entity { get; set; }

		public void Select()
		{
			journal.Display(this);
		}
	}
}
using System;
using LD48.Utils;
using UnityEngine;

namespace LD48.Save
{
	[Serializable]
	public class FileData
	{
		[SerializeField]
		private string fileName;
		public string FileName { get => fileName; set => fileName = value; }

		[SerializeField]
		private int totalPlayTime;
		public int TotalPlayTime { get => totalPlayTime; set => totalPlayTime = value; }

		[SerializeField]
		private string dateCreated;

		[SerializeField]
		private string lastSaveDate;
		public string LastSaveDate { get => lastSaveDate; set => lastSaveDate = value; }

		public FileData()
		{
			dateCreated = TimeUtils.GetCurrentDateTime;
		}
	}
}
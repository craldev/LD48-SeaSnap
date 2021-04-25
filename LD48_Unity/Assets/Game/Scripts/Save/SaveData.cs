using System;
using System.IO;
using Cysharp.Threading.Tasks;
using LD48.Gameplay.Entity;
using UnityEngine;

namespace LD48.Save
{
	[Serializable]
	public class SaveData
	{
		private const string SAVE_SUFFIX = "save.json";

		private string SaveFilePath => $"{Application.persistentDataPath}/";

		private static SaveData instance;
		public static SaveData Instance => instance ??= new SaveData();

		[SerializeField]
		private FileData fileData = new FileData();
		public FileData FileData => fileData;

		[SerializeField]
		private PlayerData playerData = new PlayerData();
		public PlayerData PlayerData => playerData;

		[SerializeField]
		private JournalData journalData = new JournalData();
		public JournalData JournalData => journalData;

		public CurrentSessionData CurrentSessionData { get; } = new CurrentSessionData();

		private bool isLoaded;

		public void NewGame(string fileName)
		{
			Reset();
			FileData.FileName = fileName;

			isLoaded = true;
			SaveGame();
		}

		public SaveData PreviewGame(string filePath)
		{
			var previewSaveData = new SaveData();
			return LoadSaveData(filePath, previewSaveData) ? previewSaveData : null;
		}

		public bool LoadGame(string filePath)
		{
			var result = LoadSaveData(filePath, this);
			if (result)
			{
				CurrentSessionData.LastSaveTime = Time.time;
				journalData.Initialize();
			}

			return isLoaded = result;
		}

		private bool LoadSaveData(string filePath, SaveData saveData)
		{
			var jsonString = File.ReadAllText(filePath);
			try
			{
				JsonUtility.FromJsonOverwrite(jsonString, saveData);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void Reset()
		{
			fileData = new FileData();
			playerData = new PlayerData();
			journalData = new JournalData();
			isLoaded = false;
		}

		public async void SaveGame()
		{
			if (!isLoaded) return;

			await UniTask.WaitForEndOfFrame();

			FileData.LastSaveDate = DateTime.Now.ToString("HH:mm dd MMMM, yyyy");
			FileData.TotalPlayTime += Mathf.RoundToInt(Time.time - CurrentSessionData.LastSaveTime);
			var path = SaveFilePath;
			await UniTask.Run(() => SerializeSaveData(path));
			CurrentSessionData.LastSaveTime = Time.time;
		}

		private async void SerializeSaveData(string filePath)
		{
			await UniTask.SwitchToMainThread();
			var jsonString = JsonUtility.ToJson(instance);
			
			if (!Directory.Exists(filePath + fileData.FileName))
			{
				Directory.CreateDirectory(filePath + fileData.FileName);
			}

			File.WriteAllText(filePath + fileData.FileName + "/" + SAVE_SUFFIX, jsonString);
		}

		public void SaveCapture(Entity entity, Texture2D imageTexture)
		{
			var bytes = imageTexture.EncodeToPNG();
			var folderPath = SaveFilePath + fileData.FileName + "/Snaps/";
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			var savePath = folderPath + entity.EntityName + ".png";
			File.WriteAllBytes(savePath, bytes);

			if (journalData.journalDictionary.ContainsKey(entity))
			{
				journalData.journalDictionary[entity].pictureFilePath = savePath;
			}
			else
			{
				journalData.Add(entity, new JournalData.JournalEntry(entity, savePath));
			}
		}
	}
}
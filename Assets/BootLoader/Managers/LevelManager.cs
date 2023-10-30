using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BootLoader.Base;
using Enhancements;
using Enhancements.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BootLoader.Managers
{
	public class LevelManager : SingletonDontDestroy<LevelManager>
	{
		private const string SCENE_FOLDER_PATH = "Assets/Scenes/";
		[SerializeField] private Scene mainMenu;
		private LoadingScreen loadingScreen;

		private void Awake()
		{
			loadingScreen = LoadingScreen.Instance;
			LoadScene(mainMenu);
		}
		#region API
		public void LoadScene(Scene scene)
		{
			var path = new[] { SCENE_FOLDER_PATH, scene.ToString(), ".unity" }.Join();
			loadingScreen.BringIn();
			LoadSceneAsync(path);
		}
		#endregion

		#region Private Methods
		private async void LoadSceneAsync(string scenePath)
		{
			var scene = SceneManager.LoadSceneAsync(scenePath);
			scene.allowSceneActivation = false;
			loadingScreen.ResetProgress();
			while (scene.progress < 0.9f)
			{
				await Task.Delay(100);
				loadingScreen.target = scene.progress;
			}
			await Task.Delay(300);
			loadingScreen.FinishProgress();
			scene.allowSceneActivation = true;
		}
		#endregion
		
		#region Editor
		#if UNITY_EDITOR
		private static void CheckScenes()
		{
			var enumValues = Enum.GetNames(typeof(Scene));
			var buildNames = EditorBuildSettings.scenes.Select(s => s.path).GetFileNames();
			var sceneFiles = Directory.GetFiles(SCENE_FOLDER_PATH, "*.unity").GetFileNames();

			var missingScenes = enumValues.Except(sceneFiles).ToList();
			var exceptionsCount = 0;
			foreach (var scene in missingScenes)
			{
				Debug.LogError(new[] { "No scene named ", scene, " in scenes folder." }.Join());
				exceptionsCount++;
			}
			foreach (var exception in enumValues.Except(buildNames).Except(missingScenes))
			{
				var scene = new EditorBuildSettingsScene( new[] { SCENE_FOLDER_PATH, exception, ".unity" }.Join(), true);
				var buildScenes = EditorBuildSettings.scenes;
				ArrayUtility.Add(ref buildScenes, scene);
				EditorBuildSettings.scenes = buildScenes;
				Debug.Log(new[] { "Scene: ", exception, " has been added to build settings." }.Join());
			}
			foreach (var exception in sceneFiles.Except(enumValues))
			{
				Debug.LogWarning(new[] { "Scene: ", exception, " hasn't been added to enum." }.Join());
			}
		
			if (exceptionsCount == 0)
				Debug.LogFormat("<color=green>Ready to Go!</color>");
		}
		[CustomEditor(typeof(LevelManager))] private class LevelManagerEditor : ButtonedEditor<LevelManager>
		{
			protected override Action[] DefineActions() => new Action[] { CheckScenes };
		}
		#endif
		#endregion
	}
}
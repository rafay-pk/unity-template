#if UNITY_EDITOR
using Enhancements.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace Enhancements.UnityExtensions
{
	public static class CopyFullPath
	{
		[MenuItem("Assets/Copy Full Path", false, 19)]
		private static void CopyPathToClipboard()
		{
			var obj = Selection.activeObject;
        
			if (obj == null) return;
			if (!AssetDatabase.Contains(obj)) return;
			
			GUIUtility.systemCopyBuffer = new[]
				{ Application.dataPath, AssetDatabase.GetAssetPath(obj)[6..].Replace('/', '\\') }.Join();
		}
	}
}
#endif
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EDITOR = UnityEditor.Editor;

namespace Enhancements.UnityExtensions
{
	[CustomPropertyDrawer(typeof(ScriptableObject), true)]
	public class ScriptableObjectDrawer : PropertyDrawer
	{
		private static readonly Dictionary<System.Type, bool> foldoutByType = new Dictionary<System.Type, bool>();

		private EDITOR editor;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label, true);
        
			var foldout = false;
			if (property.objectReferenceValue != null)
			{
				// Store foldout values in a dictionary per object type
				var foldout_exists = foldoutByType.TryGetValue(property.objectReferenceValue.GetType(), out foldout);
				foldout = EditorGUI.Foldout(position, foldout, GUIContent.none);
				if (foldout_exists)
					foldoutByType[property.objectReferenceValue.GetType()] = foldout;
				else
					foldoutByType.Add(property.objectReferenceValue.GetType(), foldout);
			}

			if (!foldout) return;
			EditorGUI.indentLevel++;
			if (!editor)
				EDITOR.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
			editor.OnInspectorGUI();
			EditorGUI.indentLevel--;
		}
	}
}
#endif
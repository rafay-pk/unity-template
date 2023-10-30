namespace Enhancements
{
using System;
using UnityEngine;
#if UNITY_EDITOR
public abstract class ButtonedEditor<T> : UnityEditor.Editor where T : MonoBehaviour
{
	protected T self;
	private Action[] actions;
	private bool defined;
	protected abstract Action[] DefineActions();
	public override void OnInspectorGUI()
	{
		if (!defined)
		{
			self = (T)target;
			actions = DefineActions();
			defined = true;
		}
		DrawDefaultInspector();
		foreach (var action in actions) 
			Button(action);
	}
	private static void Button(Action action)
	{
		if (GUILayout.Button(action.Method.Name)) 
			action.Invoke();
	}
}
#endif
}

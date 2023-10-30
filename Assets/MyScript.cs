using System;
using System.Linq;
using System.Text;
using Enhancements;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class MyScript : MonoBehaviour
{
	[SerializeField] private Transform reference;
	[SerializeField] private int passwordLength;
	private const string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	private void GameObjectSpecificFunction() => print(reference);
	private static void StaticFunction() => print("Hello World");
	private void ComplexFunctionCaller() => print(GeneratePassword(passwordLength));
	private static string GeneratePassword(int len)
	{
		var rand = new Random();
		var pass = new StringBuilder();
		Enumerable.Range(0, len).ToList().ForEach(i => pass.Append(charset[rand.Next(charset.Length)]));
		return pass.ToString();
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(MyScript))]
	private class MyEditor : ButtonedEditor<MyScript>
	{
		protected override Action[] DefineActions() => new Action[]
			{ self.GameObjectSpecificFunction, StaticFunction, self.ComplexFunctionCaller };
	}
	#endif
}
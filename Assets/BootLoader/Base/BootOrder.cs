using System.Collections.Generic;
using UnityEngine;

namespace BootLoader.Base
{
	public class BootOrder :  MonoBehaviour
	{
		[SerializeField] private List<SingletonBase> singletons;
		private void Awake() => singletons.ForEach(singleton => { singleton.Initialize(); });
	}
}
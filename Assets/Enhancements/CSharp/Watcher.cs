using UnityEngine.Events;

namespace Enhancements.CSharp
{
	public class Watcher <T>
	{
		private T _value;
		private readonly UnityEvent valueChanged;

		public T Value
		{
			get => _value;
			set
			{
				_value = value;
				valueChanged.Invoke();
			}
		}

		public Watcher(ref T value)
		{
			_value = value;
			valueChanged = new UnityEvent();
		}

		public void AddListener(UnityAction unityAction) => valueChanged.AddListener(unityAction);
		public void RemoveListener(UnityAction unityAction) => valueChanged.RemoveListener(unityAction);
	}
}
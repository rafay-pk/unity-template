using System;
using System.Threading;

namespace Enhancements.CSharp
{
	public class RepeatingAction
	{
		private readonly Action action;
		private readonly int interval, stopAfter;
		private bool continueExecution;
		public RepeatingAction(Action action, float interval, float stopAfter = 0f)
		{
			this.action = action;
			this.interval = (int)(interval * 1000);
			this.stopAfter = (int)(stopAfter * 1000);
			new Thread(Execute).Start();
		}
		
		public void Start()
		{
			continueExecution = true;
			if (stopAfter == 0) return;
			new Timer(Stop, null, stopAfter, Timeout.Infinite);
		}
		public void Stop(object state = null) => continueExecution = false;
		public void Restart()
		{
			new Thread(Execute).Start();
			Start();
		}
		
		private void Execute()
		{
			while (!continueExecution) {}
			while (continueExecution)
			{
				action.Invoke();
				Thread.Sleep(interval);
			}
		}
	}
}
namespace EM.UI
{
using System;
using Foundation;

public interface IViewContainerBatch
{
	IViewContainerBatch Open<T>(Modes mode = Modes.None)
		where T : View;

	IViewContainerBatch Close<T>()
		where T : View;

	IViewContainerSequence InParallel();

	IViewContainerComplete OnComplete(Action command);

	ICommand GetCommand();
}

}
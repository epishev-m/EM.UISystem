namespace EM.UI
{
using System;
using Foundation;

public interface IViewContainerSequence
{
	IViewContainerSequence Open<T>(Modes mode = Modes.None)
		where T : View;

	IViewContainerSequence Close<T>()
		where T : View;

	IViewContainerBatch InParallel();

	IViewContainerComplete OnComplete(Action command);

	ICommand GetCommand();
}

}
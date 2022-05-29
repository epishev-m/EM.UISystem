namespace EM.UI
{

using System;
using Foundation;

public interface IUiContainerBatch
{
	IUiContainerBatch Open<T>(Modes mode = Modes.None)
		where T : Panel;

	IUiContainerBatch Close<T>()
		where T : Panel;

	IUiContainerSequence InParallel();

	IUiContainerContainer OnComplete(Action command);

	ICommand GetCommand();

	void Execute();
}

}
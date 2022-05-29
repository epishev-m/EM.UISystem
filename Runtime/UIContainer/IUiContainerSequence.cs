namespace EM.UI
{

using System;
using Foundation;

public interface IUiContainerSequence
{
	IUiContainerSequence Open<T>(Modes mode = Modes.None)
		where T : Panel;

	IUiContainerSequence Close<T>()
		where T : Panel;

	IUiContainerBatch InParallel();

	IUiContainerContainer OnComplete(Action command);

	ICommand GetCommand();

	void Execute();
}

}
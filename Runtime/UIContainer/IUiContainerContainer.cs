namespace EM.UI
{

using Foundation;

public interface IUiContainerContainer
{
	IUiContainerSequence InSequence();

	IUiContainerBatch InParallel();

	ICommand GetCommand();

	void Execute();
}

}
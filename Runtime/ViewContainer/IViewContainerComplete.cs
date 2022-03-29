namespace EM.UI
{

using Foundation;

public interface IViewContainerComplete
{
	IViewContainerSequence InSequence();

	IViewContainerBatch InParallel();

	ICommand GetCommand();

	void Execute();
}

}
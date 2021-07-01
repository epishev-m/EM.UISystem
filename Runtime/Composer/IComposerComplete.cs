namespace EM.UI
{
using Foundation;

public interface IComposerComplete
{
	IComposerSequence InSequence();

	IComposerBatch InParallel();

	ICommand GetCommand();
}

}
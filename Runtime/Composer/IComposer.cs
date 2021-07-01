namespace EM.UI
{

public interface IComposer
{
	IComposerBatch InParallel();

	IComposerSequence InSequence();
}

}
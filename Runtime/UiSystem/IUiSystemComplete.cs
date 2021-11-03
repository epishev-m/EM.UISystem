namespace EM.UI
{
using Foundation;

public interface IUiSystemComplete
{
	IUiSystemSequence InSequence();

	IUiSystemBatch InParallel();

	ICommand GetCommand();
}

}
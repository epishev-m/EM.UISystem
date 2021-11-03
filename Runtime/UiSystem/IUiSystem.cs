namespace EM.UI
{

public interface IUiSystem
{
	void CreatePanels();

	IUiSystemBatch InParallel();

	IUiSystemSequence InSequence();
}

}
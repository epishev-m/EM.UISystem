namespace EM.UI
{
using Foundation;
using System;

public interface IUiSystemBatch
{
	IUiSystemBatch Open(
		string name,
		Modes mode = Modes.None);

	IUiSystemBatch Close(
		string name);

	IUiSystemSequence InSequence();

	IUiSystemComplete OnComplete(
		Action command);

	ICommand GetCommand();
}

}

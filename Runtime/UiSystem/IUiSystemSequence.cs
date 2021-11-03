namespace EM.UI
{
using Foundation;
using System;

public interface IUiSystemSequence
{
	IUiSystemSequence Open(
		string name,
		Modes mode = Modes.None);

	IUiSystemSequence Close(
		string name);

	IUiSystemBatch InParallel();

	IUiSystemComplete OnComplete(
		Action command);

	ICommand GetCommand();
}

}

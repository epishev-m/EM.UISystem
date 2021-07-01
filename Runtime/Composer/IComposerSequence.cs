namespace EM.UI
{
using Foundation;
using System;

public interface IComposerSequence
{
	IComposerSequence Open(
		string name,
		Modes mode = Modes.None);

	IComposerSequence Close(
		string name);

	IComposerBatch InParallel();

	IComposerComplete OnComplete(
		Action command);

	ICommand GetCommand();
}

}

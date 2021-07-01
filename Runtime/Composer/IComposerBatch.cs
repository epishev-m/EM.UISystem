namespace EM.UI
{
using Foundation;
using System;

public interface IComposerBatch
{
	IComposerBatch Open(
		string name,
		Modes mode = Modes.None);

	IComposerBatch Close(
		string name);

	IComposerSequence InSequence();

	IComposerComplete OnComplete(
		Action command);

	ICommand GetCommand();
}

}

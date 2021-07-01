namespace EM.UI
{
using Foundation;
using System;

public sealed class ComposerSequence :
	IComposerSequence
{
	private readonly CommandSequence root;

	private readonly IModesController modesController;

	private readonly Composer composer;

	private readonly CommandSequence sequence;

	#region IComposerSequence

	public IComposerSequence Open(
		string name,
		Modes mode = Modes.None)
	{
		var panel = composer.GetPanel(name);
		Requires.NotNull(panel, nameof(panel));

		ICommand command;

		if (mode == Modes.Modal)
		{
			command = new CommandOpenPanelModal(panel, modesController);
		}
		else
		{
			command = new CommandOpenPanelNone(panel, modesController);
		}

		sequence.Add(command);

		return this;
	}

	public IComposerSequence Close(
		string name)
	{
		var panel = composer.GetPanel(name);
		Requires.NotNull(panel, nameof(panel));

		if (modesController.TryGetPanelInfo(panel, out var panelInfo) == false)
		{
			return this;
		}

		ICommand command;

		if (panelInfo.Mode == Modes.Modal)
		{
			command = new CommandClosePanelModal(panel, modesController);
		}
		else
		{
			command = new CommandClosePanelNone(panel, modesController);
		}

		sequence.Add(command);

		return this;
	}

	public IComposerBatch InParallel()
	{
		var batch = new ComposerBatch(root, modesController, composer);

		return batch;
	}

	public IComposerComplete OnComplete(
		Action command)
	{
		Requires.NotNull(command, nameof(command));

		sequence.Done += command;
		var composerComplete = new ComposerComplete(root, modesController, composer);

		return composerComplete;
	}

	public ICommand GetCommand()
	{
		return root;
	}

	#endregion
	#region ComposerSequence

	public ComposerSequence(
		CommandSequence root,
		IModesController modesController,
		Composer composer)
	{
		Requires.NotNull(root, nameof(root));
		Requires.NotNull(modesController, nameof(modesController));
		Requires.NotNull(composer, nameof(composer));

		this.root = root;
		this.modesController = modesController;
		this.composer = composer;
		sequence = new CommandSequence();
		root.Add(sequence);
	}

	#endregion
}

}

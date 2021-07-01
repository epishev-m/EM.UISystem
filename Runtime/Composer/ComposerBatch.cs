namespace EM.UI
{
using System;
using System.Collections.Generic;
using Foundation;

public sealed class ComposerBatch :
	IComposerBatch
{
	private readonly CommandSequence root;

	private readonly IModesController modesController;

	private readonly Composer composer;

	private readonly CommandBatch batch;

	private readonly HashSet<string> openingPanels = new HashSet<string>();

	private readonly List<string> openingModalPanels = new List<string>();

	private readonly HashSet<string> closingPanels = new HashSet<string>();

	#region IComposerParallel

	public IComposerBatch Open(
		string name,
		Modes mode = Modes.None)
	{
		openingPanels.Add(name);

		if (mode == Modes.Modal)
		{
			openingModalPanels.Add(name);
		}

		return this;
	}

	public IComposerBatch Close(
		string name)
	{
		closingPanels.Add(name);

		return this;
	}

	public IComposerSequence InSequence()
	{
		var sequence = new ComposerSequence(root, modesController, composer);

		return sequence;
	}

	public IComposerComplete OnComplete(
		Action command)
	{
		Requires.NotNull(command, nameof(command));

		CreateCommands();
		batch.Done += command;
		var composerComplete = new ComposerComplete(root, modesController, composer);

		return composerComplete;
	}

	public ICommand GetCommand()
	{
		CreateCommands();

		return root;
	}

	#endregion
	#region ComposerBatch

	public ComposerBatch(
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
		batch = new CommandBatch();
		root.Add(batch);
	}

	private void CreateCommands()
	{
		closingPanels.ExceptWith(openingPanels);
		CreateCloseCommands();
		CreateOpenCommands();
	}

	private void CreateCloseCommands()
	{
		foreach (var name in closingPanels)
		{
			var panel = composer.GetPanel(name);
			Requires.NotNull(panel, nameof(panel));

			if (modesController.TryGetPanelInfo(panel, out var panelInfo) == false)
			{
				continue;
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

			batch.Add(command);
		}
	}

	private void CreateOpenCommands()
	{
		foreach (var name in openingPanels)
		{
			var panel = composer.GetPanel(name);
			Requires.NotNull(panel, nameof(panel));

			ICommand command;
			var mode = openingModalPanels.Contains(name) ? Modes.Modal : Modes.None;

			if (mode == Modes.Modal)
			{
				command = new CommandOpenPanelModal(panel, modesController);
			}
			else
			{
				command = new CommandOpenPanelNone(panel, modesController);
			}

			batch.Add(command);
		}
	}

	#endregion
}

}

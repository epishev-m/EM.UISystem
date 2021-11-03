namespace EM.UI
{
using System;
using System.Collections.Generic;
using Foundation;

public sealed partial class UiSystem
{
	public sealed class Batch :
		IUiSystemBatch
	{
		private readonly CommandSequence root;

		private readonly IModesController modesController;

		private readonly UiSystem uiSystem;

		private readonly CommandBatch batch;

		private readonly HashSet<string> openingPanels = new HashSet<string>();

		private readonly List<string> openingModalPanels = new List<string>();

		private readonly HashSet<string> closingPanels = new HashSet<string>();

		#region IUiSystemBatch

		public IUiSystemBatch Open(
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

		public IUiSystemBatch Close(
			string name)
		{
			closingPanels.Add(name);

			return this;
		}

		public IUiSystemSequence InSequence()
		{
			var sequence = new Sequence(root, modesController, uiSystem);

			return sequence;
		}

		public IUiSystemComplete OnComplete(
			Action command)
		{
			Requires.NotNull(command, nameof(command));

			CreateCommands();
			batch.Done += command;
			var composerComplete = new Complete(root, modesController, uiSystem);

			return composerComplete;
		}

		public ICommand GetCommand()
		{
			CreateCommands();

			return root;
		}

		#endregion
		#region ComposerBatch

		public Batch(
			CommandSequence root,
			IModesController modesController,
			UiSystem uiSystem)
		{
			Requires.NotNull(root, nameof(root));
			Requires.NotNull(modesController, nameof(modesController));
			Requires.NotNull(uiSystem, nameof(uiSystem));

			this.root = root;
			this.modesController = modesController;
			this.uiSystem = uiSystem;
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
				var panel = uiSystem.GetPanel(name);
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
				var panel = uiSystem.GetPanel(name);
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

}

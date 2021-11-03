namespace EM.UI
{
using Foundation;
using System;

public sealed partial class UiSystem
{
	public sealed class Sequence :
		IUiSystemSequence
	{
		private readonly CommandSequence root;

		private readonly IModesController modesController;

		private readonly UiSystem uiSystem;

		private readonly CommandSequence sequence;

		#region IUiSystemSequence

		public IUiSystemSequence Open(
			string name,
			Modes mode = Modes.None)
		{
			var panel = uiSystem.GetPanel(name);
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

		public IUiSystemSequence Close(
			string name)
		{
			var panel = uiSystem.GetPanel(name);
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

		public IUiSystemBatch InParallel()
		{
			var batch = new Batch(root, modesController, uiSystem);

			return batch;
		}

		public IUiSystemComplete OnComplete(
			Action command)
		{
			Requires.NotNull(command, nameof(command));

			sequence.Done += command;
			var composerComplete = new Complete(root, modesController, uiSystem);

			return composerComplete;
		}

		public ICommand GetCommand()
		{
			return root;
		}

		#endregion
		#region Sequence

		public Sequence(
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
			sequence = new CommandSequence();
			root.Add(sequence);
		}

		#endregion
	}
}

}

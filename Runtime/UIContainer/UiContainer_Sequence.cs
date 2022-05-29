namespace EM.UI
{

using System;
using Foundation;

public partial class UiContainer
{
	public sealed class Sequence :
		IUiContainerSequence
	{
		private readonly CommandSequence root;

		private readonly IModalLogicController modalLogicController;

		private readonly IUiContainer uiContainer;

		private readonly CommandSequence sequence;

		#region IUiContainerSequence

		public IUiContainerSequence Open<T>(Modes mode = Modes.None)
			where T : Panel
		{
			var key = typeof(T);
			ICommand command = new CommandLoadPanel(uiContainer, key);
			sequence.Add(command);
			command = new CommandOpenView(uiContainer, modalLogicController, key, mode);
			sequence.Add(command);

			return this;
		}

		public IUiContainerSequence Close<T>()
			where T : Panel
		{
			var view = uiContainer.GetPanel<T>();
			Requires.NotNull(view, nameof(view));

			if (modalLogicController.TryGetViewInfo(view, out var viewInfo) == false)
			{
				return this;
			}

			ICommand command = new CommandClosePanel(modalLogicController, view, viewInfo.Mode);
			sequence.Add(command);

			return this;
		}

		public IUiContainerBatch InParallel()
		{
			var batch = new Batch(root, modalLogicController, uiContainer);

			return batch;
		}

		public IUiContainerContainer OnComplete(Action command)
		{
			Requires.NotNull(command, nameof(command));

			sequence.Done += command;
			var complete = new Container(uiContainer, modalLogicController, root);

			return complete;
		}

		public ICommand GetCommand()
		{
			return root;
		}

		public void Execute()
		{
			root.Execute();
		}

		#endregion

		#region Sequence

		public Sequence(CommandSequence root,
			IModalLogicController modalLogicController,
			IUiContainer uiContainer)
		{
			Requires.NotNull(root, nameof(root));
			Requires.NotNull(modalLogicController, nameof(modalLogicController));
			Requires.NotNull(uiContainer, nameof(uiContainer));

			this.root = root;
			this.modalLogicController = modalLogicController;
			this.uiContainer = uiContainer;
			sequence = new CommandSequence();
			root.Add(sequence);
		}

		#endregion
	}
}

}
namespace EM.UI
{
using System;
using Foundation;

public partial class ViewContainer
{
	public sealed class Sequence :
		IViewContainerSequence
	{
		private readonly CommandSequence root;

		private readonly IModalLogicController modalLogicController;

		private readonly IViewContainer viewContainer;

		private readonly CommandSequence sequence;

		#region IViewContainerSequence

		public IViewContainerSequence Open<T>(Modes mode = Modes.None)
			where T : View
		{
			var key = typeof(T);
			ICommand command = new CommandLoadView(viewContainer, key);
			sequence.Add(command);
			command = new CommandOpenView(viewContainer, modalLogicController, key, mode);
			sequence.Add(command);

			return this;
		}

		public IViewContainerSequence Close<T>()
			where T : View
		{
			var view = viewContainer.GetView<T>();
			Requires.NotNull(view, nameof(view));

			if (modalLogicController.TryGetViewInfo(view, out var viewInfo) == false)
			{
				return this;
			}

			ICommand command = new CommandCloseView(modalLogicController, view, viewInfo.Mode);
			sequence.Add(command);

			return this;
		}

		public IViewContainerBatch InParallel()
		{
			var batch = new Batch(root, modalLogicController, viewContainer);

			return batch;
		}

		public IViewContainerComplete OnComplete(Action command)
		{
			Requires.NotNull(command, nameof(command));

			sequence.Done += command;
			var complete = new Complete(viewContainer, modalLogicController, root);

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
			IViewContainer viewContainer)
		{
			Requires.NotNull(root, nameof(root));
			Requires.NotNull(modalLogicController, nameof(modalLogicController));
			Requires.NotNull(viewContainer, nameof(viewContainer));

			this.root = root;
			this.modalLogicController = modalLogicController;
			this.viewContainer = viewContainer;
			sequence = new CommandSequence();
			root.Add(sequence);
		}

		#endregion
	}
}

}
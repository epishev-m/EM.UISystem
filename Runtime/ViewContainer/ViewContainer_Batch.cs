namespace EM.UI
{
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

public partial class ViewContainer
{
	public sealed class Batch :
		IViewContainerBatch
	{
		private readonly CommandSequence root;

		private readonly IModalLogicController modalLogicController;

		private readonly IViewContainer viewContainer;

		private readonly CommandBatch batch;
		
		private readonly HashSet<(Type Key, Modes Mode)> openingViews = new();

		private readonly HashSet<Type> closingViews = new();

		#region IViewContainerBatch

		public IViewContainerBatch Open<T>(Modes mode = Modes.None)
			where T : View
		{
			openingViews.Add((typeof(T), mode));

			return this;
		}

		public IViewContainerBatch Close<T>()
			where T : View
		{
			closingViews.Add(typeof(T));

			return this;
		}

		public IViewContainerSequence InParallel()
		{
			CreateCommands();
			var sequence = new Sequence(root, modalLogicController, viewContainer);

			return sequence;
		}

		public IViewContainerComplete OnComplete(Action command)
		{
			Requires.NotNull(command, nameof(command));

			CreateCommands();
			batch.Done += command;
			var complete = new Complete(viewContainer, modalLogicController, root);

			return complete;
		}

		public ICommand GetCommand()
		{
			CreateCommands();

			return root;
		}

		public void Execute()
		{
			root.Execute();
		}

		#endregion

		#region Batch

		public Batch(CommandSequence root,
			IModalLogicController modalLogicController,
			IViewContainer viewContainer)
		{
			Requires.NotNull(root, nameof(root));
			Requires.NotNull(modalLogicController, nameof(modalLogicController));
			Requires.NotNull(viewContainer, nameof(viewContainer));

			this.root = root;
			this.modalLogicController = modalLogicController;
			this.viewContainer = viewContainer;
			batch = new CommandBatch();
			root.Add(batch);
		}

		private void CreateCommands()
		{
			var views = openingViews.Select(x => x.Key);
			closingViews.ExceptWith(views);
			CreateCloseCommands();
			CreateOpenCommands();
		}

		private void CreateCloseCommands()
		{
			foreach (var view in closingViews.Select(key => viewContainer.GetView(key)))
			{
				Requires.NotNull(view, nameof(view));

				if (modalLogicController.TryGetViewInfo(view, out var viewInfo) == false)
				{
					continue;
				}

				ICommand command = new CommandCloseView(modalLogicController, view, viewInfo.Mode);
				batch.Add(command);
			}
		}

		private void CreateOpenCommands()
		{
			foreach (var (key, mode) in openingViews)
			{
				var sequence = new CommandSequence();
				ICommand command = new CommandLoadView(viewContainer, key);
				sequence.Add(command);
				command = new CommandOpenView(viewContainer, modalLogicController, key, mode);
				sequence.Add(command);
				batch.Add(sequence);
			}
		}

		#endregion
	}
}

}
namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

public partial class UiContainer
{
	public sealed class Batch :
		IUiContainerBatch
	{
		private readonly CommandSequence root;

		private readonly IModalLogicController modalLogicController;

		private readonly IUiContainer uiContainer;

		private readonly CommandBatch batch;

		private readonly HashSet<(Type Key, Modes Mode)> openingViews = new();

		private readonly HashSet<Type> closingViews = new();

		#region IUiContainerBatch

		public IUiContainerBatch Open<T>(Modes mode = Modes.None)
			where T : Panel
		{
			openingViews.Add((typeof(T), mode));

			return this;
		}

		public IUiContainerBatch Close<T>()
			where T : Panel
		{
			closingViews.Add(typeof(T));

			return this;
		}

		public IUiContainerSequence InParallel()
		{
			CreateCommands();
			var sequence = new Sequence(root, modalLogicController, uiContainer);

			return sequence;
		}

		public IUiContainerContainer OnComplete(Action command)
		{
			Requires.NotNull(command, nameof(command));

			CreateCommands();
			batch.Done += command;
			var complete = new Container(uiContainer, modalLogicController, root);

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
			IUiContainer uiContainer)
		{
			Requires.NotNull(root, nameof(root));
			Requires.NotNull(modalLogicController, nameof(modalLogicController));
			Requires.NotNull(uiContainer, nameof(uiContainer));

			this.root = root;
			this.modalLogicController = modalLogicController;
			this.uiContainer = uiContainer;
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
			foreach (var view in closingViews.Select(key => uiContainer.GetPanel(key)))
			{
				Requires.NotNull(view, nameof(view));

				if (modalLogicController.TryGetViewInfo(view, out var viewInfo) == false)
				{
					continue;
				}

				ICommand command = new CommandClosePanel(modalLogicController, view, viewInfo.Mode);
				batch.Add(command);
			}
		}

		private void CreateOpenCommands()
		{
			foreach (var (key, mode) in openingViews)
			{
				var sequence = new CommandSequence();
				ICommand command = new CommandLoadPanel(uiContainer, key);
				sequence.Add(command);
				command = new CommandOpenView(uiContainer, modalLogicController, key, mode);
				sequence.Add(command);
				batch.Add(sequence);
			}
		}

		#endregion
	}
}

}
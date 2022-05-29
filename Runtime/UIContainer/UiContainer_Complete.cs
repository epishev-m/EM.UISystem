namespace EM.UI
{

using Foundation;

public sealed partial class UiContainer
{
	public sealed class Container :
		IUiContainerContainer
	{
		private readonly CommandSequence root;

		private readonly IModalLogicController modalLogicController;

		private readonly IUiContainer uiContainer;

		#region IUiContainerContainer

		public IUiContainerSequence InSequence()
		{
			var sequence = new Sequence(root, modalLogicController, uiContainer);

			return sequence;
		}

		public IUiContainerBatch InParallel()
		{
			var batch = new Batch(root, modalLogicController, uiContainer);

			return batch;
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

		#region ComposerComplete

		public Container(IUiContainer uiContainer,
			IModalLogicController modalLogicController,
			CommandSequence root)
		{
			Requires.NotNull(uiContainer, nameof(uiContainer));
			Requires.NotNull(modalLogicController, nameof(modalLogicController));
			Requires.NotNull(root, nameof(root));

			this.root = root;
			this.uiContainer = uiContainer;
			this.modalLogicController = modalLogicController;
		}

		#endregion
	}
}

}
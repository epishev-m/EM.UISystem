namespace EM.UI
{
using Foundation;

public sealed partial class ViewContainer
{
	public sealed class Complete :
		IViewContainerComplete
	{
		private readonly CommandSequence root;

		private readonly IModalLogicController modalLogicController;

		private readonly IViewContainer viewContainer;

		#region IUiSystemComplete

		public IViewContainerSequence InSequence()
		{
			var sequence = new Sequence(root, modalLogicController, viewContainer);

			return sequence;
		}

		public IViewContainerBatch InParallel()
		{
			var batch = new Batch(root, modalLogicController, viewContainer);

			return batch;
		}

		public ICommand GetCommand()
		{
			return root;
		}

		#endregion
		#region ComposerComplete

		public Complete(IViewContainer viewContainer,
			IModalLogicController modalLogicController,
			CommandSequence root)
		{
			Requires.NotNull(viewContainer, nameof(viewContainer));
			Requires.NotNull(modalLogicController, nameof(modalLogicController));
			Requires.NotNull(root, nameof(root));

			this.root = root;
			this.viewContainer = viewContainer;
			this.modalLogicController = modalLogicController;
		}

		#endregion
	}
}

}
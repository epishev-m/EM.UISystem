namespace EM.UI
{
using Foundation;

public sealed partial class UiSystem
{
	public sealed class Complete :
		IUiSystemComplete
	{
		private readonly CommandSequence root;

		private readonly IModesController modesController;

		private readonly UiSystem uiSystem;

		#region IUiSystemComplete

		public IUiSystemSequence InSequence()
		{
			var sequence = new Sequence(root, modesController, uiSystem);

			return sequence;
		}

		public IUiSystemBatch InParallel()
		{
			var batch = new Batch(root, modesController, uiSystem);

			return batch;
		}

		public ICommand GetCommand()
		{
			return root;
		}

		#endregion
		#region ComposerComplete

		public Complete(
			CommandSequence root,
			IModesController modesController,
			UiSystem uiSystem)
		{
			Requires.NotNull(root, nameof(root));
			Requires.NotNull(modesController, nameof(modesController));
			Requires.NotNull(uiSystem, nameof(uiSystem));

			this.root = root;
			this.uiSystem = uiSystem;
			this.modesController = modesController;
		}

		#endregion
	}
}

}
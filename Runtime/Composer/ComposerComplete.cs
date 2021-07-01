namespace EM.UI
{
using Foundation;

public class ComposerComplete
	: IComposerComplete
{
	private readonly CommandSequence root;

	private readonly IModesController modesController;

	private readonly Composer composer;

	#region IComposerComplete

	public IComposerSequence InSequence()
	{
		var sequence = new ComposerSequence(root, modesController, composer);

		return sequence;
	}

	public IComposerBatch InParallel()
	{
		var batch = new ComposerBatch(root, modesController, composer);

		return batch;
	}

	public ICommand GetCommand()
	{
		return root;
	}

	#endregion
	#region ComposerComplete

	public ComposerComplete(
		CommandSequence root,
		IModesController modesController,
		Composer composer)
	{
		Requires.NotNull(root, nameof(root));
		Requires.NotNull(modesController, nameof(modesController));
		Requires.NotNull(composer, nameof(composer));

		this.root = root;
		this.composer = composer;
		this.modesController = modesController;
	}

	#endregion
}

}
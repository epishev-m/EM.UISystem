namespace EM.UI
{
using System.Linq;
using System.Collections.Generic;
using Foundation;

public sealed partial class UiSystem :
	IUiSystem
{
	private readonly IPanelsCreator panelsCreator;

	private readonly IModesController modesController;

	private IEnumerable<IPanel> panels;

	#region IUiSystem

	public void CreatePanels()
	{
		Requires.ValidOperation(panels == null, this, nameof(panels));

		panels = panelsCreator.CreatePanels();
	}

	public IUiSystemBatch InParallel()
	{
		var root = new CommandSequence();
		var batch = new Batch(root, modesController, this);

		return batch;
	}

	public IUiSystemSequence InSequence()
	{
		var root = new CommandSequence();
		var sequence = new Sequence(root, modesController, this);

		return sequence;
	}

	#endregion
	#region UiSystem

	public UiSystem(
		IPanelsCreator panelsCreator,
		IModesController modesController)
	{
		Requires.NotNull(panelsCreator, nameof(panelsCreator));
		Requires.NotNull(modesController, nameof(modesController));

		this.panelsCreator = panelsCreator;
		this.modesController = modesController;
	}

	public IPanel GetPanel(
		string name)
	{
		Requires.ValidOperation(panels != null, this, nameof(panels));

		var panel = panels?.FirstOrDefault(p => p.Name == name);

		return panel;
	}

	#endregion
}

}

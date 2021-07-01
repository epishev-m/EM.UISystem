namespace EM.UI
{
using System.Linq;
using System.Collections.Generic;
using Foundation;

public sealed class Composer :
	IComposer
{
	private readonly IEnumerable<IPanel> panels;

	private readonly IModesController modesController;

	#region IComposer

	public IComposerBatch InParallel()
	{
		var root = new CommandSequence();
		var sequence = new ComposerBatch(root, modesController, this);

		return sequence;
	}

	public IComposerSequence InSequence()
	{
		var root = new CommandSequence();
		var sequence = new ComposerSequence(root, modesController, this);

		return sequence;
	}

	#endregion
	#region Composer

	public Composer(
		IEnumerable<IPanel> panels,
		IModesController modesController)
	{
		Requires.NotNull(panels, nameof(panels));
		Requires.NotNull(modesController, nameof(modesController));

		this.panels = panels;
		this.modesController = modesController;
	}

	public IPanel GetPanel(
		string name)
	{
		var panel = panels.FirstOrDefault(p => p.Name == name);

		return panel;
	}

	#endregion
}

}

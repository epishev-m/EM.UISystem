namespace EM.UI
{
using Foundation;

public abstract class CommandClosePanelBase :
	CommandBase
{
	private readonly IPanel panel;

	private readonly IModesController modesController;

	#region CommandBase

	public override void Execute()
	{
		modesController.PrepareRemove(Mode, () => panel.Close(() =>
		{
			modesController.Remove(panel, Mode);
			DoneInvoke();
		}));
	}

	#endregion
	#region CommandOpenPanel

	protected CommandClosePanelBase(
		IPanel panel,
		IModesController modesController)
	{
		Requires.NotNull(panel, nameof(panel));
		Requires.NotNull(modesController, nameof(modesController));

		this.panel = panel;
		this.modesController = modesController;
	}

	protected abstract Modes Mode
	{
		get;
	}

	#endregion
}

}

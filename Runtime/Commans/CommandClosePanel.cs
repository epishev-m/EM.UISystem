namespace EM.UI
{

using Foundation;

public sealed class CommandClosePanel :
	CommandBase
{
	private readonly IModalLogicController modalLogicController;

	private readonly Panel panel;

	private readonly Modes mode;

	#region CommandBase

	public override void Execute()
	{
		modalLogicController.PrepareRemove(mode,
			() => panel.Close(() =>
			{
				modalLogicController.Remove(panel, mode);
				DoneInvoke();
			}));
	}

	#endregion

	#region CommandOpenPanel

	public CommandClosePanel(IModalLogicController modalLogicController,
		Panel panel,
		Modes mode)
	{
		Requires.NotNull(modalLogicController, nameof(modalLogicController));
		Requires.NotNull(panel, nameof(panel));

		this.modalLogicController = modalLogicController;
		this.panel = panel;
		this.mode = mode;
	}

	#endregion
}

}

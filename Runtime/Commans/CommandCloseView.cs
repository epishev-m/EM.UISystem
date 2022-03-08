namespace EM.UI
{
using Foundation;

public sealed class CommandCloseView :
	CommandBase
{
	private readonly IModalLogicController modalLogicController;

	private readonly View view;

	private readonly Modes mode;

	#region CommandBase

	public override void Execute()
	{
		modalLogicController.PrepareRemove(mode, () => view.Close(() =>
		{
			modalLogicController.Remove(view, mode);
			DoneInvoke();
		}));
	}

	#endregion
	#region CommandOpenPanel

	public CommandCloseView(IModalLogicController modalLogicController,
		View view,
		Modes mode)
	{
		Requires.NotNull(modalLogicController, nameof(modalLogicController));
		Requires.NotNull(view, nameof(view));
		
		this.modalLogicController = modalLogicController;
		this.view = view;
		this.mode = mode;
	}

	#endregion
}

}

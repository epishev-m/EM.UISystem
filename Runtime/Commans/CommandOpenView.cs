namespace EM.UI
{
using System;
using Foundation;

public sealed class CommandOpenView
	: CommandBase
{
	private readonly IViewContainer viewContainer;

	private readonly IModalLogicController modalLogicController;

	private readonly Type key;

	private readonly Modes mode;

	#region CommandBase

	public override void Execute()
	{
		var view = viewContainer.GetView(key);

		modalLogicController.PrepareAdd(mode,
			() => view.Open(() =>
			{
				modalLogicController.Add(view, mode);
				DoneInvoke();
			}));
	}

	#endregion

	#region CommandOpenView

	public CommandOpenView(IViewContainer viewContainer,
		IModalLogicController modalLogicController,
		Type key,
		Modes mode)
	{
		Requires.NotNull(viewContainer, nameof(viewContainer));
		Requires.NotNull(modalLogicController, nameof(modalLogicController));
		Requires.NotNull(key, nameof(key));

		this.viewContainer = viewContainer;
		this.modalLogicController = modalLogicController;
		this.key = key;
		this.mode = mode;
	}

	#endregion
}

}
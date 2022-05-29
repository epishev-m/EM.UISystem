namespace EM.UI
{

using System;
using Foundation;

public sealed class CommandOpenView
	: CommandBase
{
	private readonly IUiContainer uiContainer;

	private readonly IModalLogicController modalLogicController;

	private readonly Type key;

	private readonly Modes mode;

	#region CommandBase

	public override void Execute()
	{
		var view = uiContainer.GetPanel(key);

		modalLogicController.PrepareAdd(mode,
			() => view.Open(() =>
			{
				modalLogicController.Add(view, mode);
				DoneInvoke();
			}));
	}

	#endregion

	#region CommandOpenView

	public CommandOpenView(IUiContainer uiContainer,
		IModalLogicController modalLogicController,
		Type key,
		Modes mode)
	{
		Requires.NotNull(uiContainer, nameof(uiContainer));
		Requires.NotNull(modalLogicController, nameof(modalLogicController));
		Requires.NotNull(key, nameof(key));

		this.uiContainer = uiContainer;
		this.modalLogicController = modalLogicController;
		this.key = key;
		this.mode = mode;
	}

	#endregion
}

}
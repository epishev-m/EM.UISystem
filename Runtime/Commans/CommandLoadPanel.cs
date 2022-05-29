namespace EM.UI
{

using System;
using Foundation;

public class CommandLoadPanel :
	CommandBase
{
	private readonly IUiContainer uiContainer;

	private readonly Type key;

	#region CommandBase

	public override void Execute()
	{
		LoadView(DoneInvoke);
	}

	#endregion

	#region CommandLoadPanel

	public CommandLoadPanel(IUiContainer uiContainer,
		Type key)
	{
		Requires.NotNull(uiContainer, nameof(uiContainer));
		Requires.NotNull(key, nameof(key));

		this.uiContainer = uiContainer;
		this.key = key;
	}

	private async void LoadView(Action onCompleted)
	{
		var result = await uiContainer.Load(key);

		Requires.ValidOperation(result, $"Failed to load {key}");

		onCompleted?.Invoke();
	}

	#endregion
}

}
namespace EM.UI
{
using System;
using Foundation;

public class CommandLoadView :
	CommandBase
{
	private readonly IViewContainer viewContainer;

	private readonly Type key;

	#region CommandBase

	public override void Execute()
	{
		LoadView(DoneInvoke);
	}

	#endregion

	#region CommandLoadView

	public CommandLoadView(IViewContainer viewContainer,
		Type key)
	{
		Requires.NotNull(viewContainer, nameof(viewContainer));
		Requires.NotNull(key, nameof(key));

		this.viewContainer = viewContainer;
		this.key = key;
	}

	private async void LoadView(Action onCompleted)
	{
		var result = await viewContainer.Load(key);

		Requires.ValidOperation(result, $"Failed to load {key}");

		onCompleted?.Invoke();
	}

	#endregion
}

}
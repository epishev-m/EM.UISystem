namespace EM.UI
{
using Foundation;

public abstract class UiCommandBase : CommandBase
{
	private ICommand command;

	#region CommandBase

	public sealed override void Execute()
	{
		if (command == null)
		{
			command = CreateCommand();
			command.Done += DoneInvoke;
		}

		command.Execute();
	}

	#endregion
	#region UiCommandBase

	protected abstract ICommand CreateCommand();

	#endregion
}

}

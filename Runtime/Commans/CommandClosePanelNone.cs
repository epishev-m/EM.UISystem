namespace EM.UI
{

public sealed class CommandClosePanelNone :
	CommandClosePanelBase
{
	#region CommandClosePanelBase

	protected override Modes Mode => Modes.None;

	#endregion
	#region CommandClosePanelModal

	public CommandClosePanelNone(
		IPanel panel,
		IModesController modesController) :
		base(
			panel,
			modesController)
	{
	}

	#endregion
}

}

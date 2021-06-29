namespace EM.UI
{

public sealed class CommandOpenPanelNone :
	CommandOpenPanelBase
{
	#region CommandOpenPanelBase

	protected override Modes Mode => Modes.None;

	#endregion
	#region CommandOpenPanelNone

	public CommandOpenPanelNone(
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

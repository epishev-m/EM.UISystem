namespace EM.UI
{

public sealed class CommandClosePanelModal :
	CommandClosePanelBase
{
	#region CommandClosePanelBase

	protected override Modes Mode => Modes.Modal;

	#endregion
	#region CommandClosePanelModal

	public CommandClosePanelModal(
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

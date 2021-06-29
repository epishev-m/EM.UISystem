namespace EM.UI
{

public sealed class CommandOpenPanelModal :
	CommandOpenPanelBase
{
	#region CommandOpenPanelBase

	protected override Modes Mode => Modes.Modal;

	#endregion
	#region CommandOpenPanelModal

	public CommandOpenPanelModal(
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

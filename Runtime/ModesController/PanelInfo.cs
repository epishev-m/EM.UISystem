namespace EM.UI
{
using Foundation;

public sealed class PanelInfo
{
	public PanelInfo(
		IPanel panel,
		Modes mode)
	{
		Requires.NotNull(panel, nameof(panel));

		Panel = panel;
		Mode = mode;
	}

	public IPanel Panel
	{
		get;
	}

	public Modes Mode
	{
		get;
	}
}

}

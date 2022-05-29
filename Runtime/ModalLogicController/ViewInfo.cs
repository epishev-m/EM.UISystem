namespace EM.UI
{

using Foundation;

public sealed class ViewInfo
{
	public readonly IPanel Panel;

	public readonly Modes Mode;

	public ViewInfo(IPanel panel,
		Modes mode)
	{
		Requires.NotNull(panel, nameof(panel));

		Panel = panel;
		Mode = mode;
	}
}

}

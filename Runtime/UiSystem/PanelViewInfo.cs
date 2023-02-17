namespace EM.UI
{

using Foundation;

public sealed class PanelViewInfo
{
	public readonly UIView View;

	public readonly Modes Mode;

	public PanelViewInfo(UIView view,
		Modes mode)
	{
		Requires.NotNull(view, nameof(view));

		View = view;
		Mode = mode;
	}
}

}
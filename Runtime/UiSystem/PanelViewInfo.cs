namespace EM.UI
{

using Foundation;

public sealed class PanelViewInfo
{
	public readonly PanelView PanelView;

	public readonly Modes Mode;

	public PanelViewInfo(PanelView panelView,
		Modes mode)
	{
		Requires.NotNull(panelView, nameof(panelView));

		PanelView = panelView;
		Mode = mode;
	}
}

}
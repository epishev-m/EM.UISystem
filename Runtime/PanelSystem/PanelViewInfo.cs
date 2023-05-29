namespace EM.UI
{

using Foundation;

public sealed class PanelViewInfo
{
	public readonly View View;

	public readonly Modes Mode;

	public PanelViewInfo(View view,
		Modes mode)
	{
		Requires.NotNullParam(view, nameof(view));

		View = view;
		Mode = mode;
	}
}

}
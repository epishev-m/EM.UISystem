namespace EM.UI
{
using Foundation;

public sealed class ViewInfo
{
	public readonly IView View;

	public readonly Modes Mode;
	
	public ViewInfo(IView view,
		Modes mode)
	{
		Requires.NotNull(view, nameof(view));

		View = view;
		Mode = mode;
	}
}

}

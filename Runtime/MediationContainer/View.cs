namespace EM.UI
{

using System.Collections.Generic;
using System.Linq;
using Foundation;

public abstract class View :
	IView
{
	private readonly List<IView> children = new();

	#region IView

	public abstract bool IsEnabled
	{
		get;
	}

	public IEnumerable<IView> Children => children;

	public void AddView(IView view)
	{
		Requires.NotNull(view, nameof(view));

		if (children.Any(v => v == view))
		{
			children.Add(view);
		}
	}

	public void RemoveView(IView view)
	{
		Requires.NotNull(view, nameof(view));

		children.Remove(view);
	}

	#endregion
}

}
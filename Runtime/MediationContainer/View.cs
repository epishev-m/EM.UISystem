namespace EM.UI
{

using System.Collections.Generic;
using System.Linq;
using Foundation;
using UnityEngine;

public abstract class View : MonoBehaviour,
	IView
{
	private readonly List<IView> _children = new();

	#region IView

	public abstract bool IsEnabled
	{
		get;
	}

	public IEnumerable<IView> Children => _children;

	public void AddView(IView view)
	{
		Requires.NotNull(view, nameof(view));

		if (_children.Any(v => v == view))
		{
			_children.Add(view);
		}
	}

	public void RemoveView(IView view)
	{
		Requires.NotNull(view, nameof(view));

		_children.Remove(view);
	}

	#endregion
}

}
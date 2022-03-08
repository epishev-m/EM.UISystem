namespace EM.UI
{
using Foundation;
using System;
using System.Collections.Generic;

public sealed class ModalLogicController :
	IModalLogicController
{
	private readonly List<ViewInfo> openViews = new(16);

	#region IModalLogicController

	public bool TryGetViewInfo(IView view,
		out ViewInfo viewInfo)
	{
		var result = false;

		if (view != null)
		{
			viewInfo = openViews.Find(info => info.View == view);
			result = viewInfo != null;
		}
		else
		{
			viewInfo = null;
		}

		return result;
	}

	public void PrepareAdd(Modes mode,
		Action onCompleted)
	{
		onCompleted?.Invoke();
	}

	public void PrepareRemove(Modes mode,
		Action onCompleted)
	{
		onCompleted?.Invoke();
	}

	public void Add(IView view,
		Modes mode)
	{
		Requires.NotNull(view, nameof(view));

		if (mode == Modes.Modal)
		{
			AddModal(view);
		}
		else
		{
			Add(view);
		}
	}

	public void Remove(IView view,
		Modes mode)
	{
		Requires.NotNull(view, nameof(view));

		if (mode == Modes.Modal)
		{
			RemoveModal(view);
		}
		else
		{
			Remove(view);
		}
	}

	#endregion
	
	#region ModalLogicController

	private void Add(IView view)
	{
		Requires.NotNull(view, nameof(view));

		var panelInfo = openViews.Find(info => info.View == view);

		if (panelInfo != null)
		{
			openViews.Remove(panelInfo);
			openViews.Add(panelInfo);
		}
		else
		{
			panelInfo = new ViewInfo(view, Modes.None);
			openViews.Add(panelInfo);
		}
	}

	private void AddModal(IView view)
	{
		Requires.NotNull(view, nameof(view));

		var viewInfo = openViews.Find(info => info.View == view);

		if (viewInfo != null)
		{
			openViews.Remove(viewInfo);
		}

		viewInfo = new ViewInfo(view, Modes.Modal);

		foreach (var info in openViews)
		{
			info.View.IsInteractable = false;
		}

		openViews.Add(viewInfo);
	}

	private void Remove(IView view)
	{
		Requires.NotNull(view, nameof(view));

		var index = openViews.FindIndex(info => info.View == view);

		if (index >= 0)
		{
			openViews.RemoveAt(index);
		}
	}

	private void RemoveModal(IView view)
	{
		Requires.NotNull(view, nameof(view));

		var index = openViews.FindIndex(info => info.View == view);

		if (index < 0)
		{
			return;
		}

		openViews.RemoveAt(index);

		foreach (var viewInfo in openViews)
		{
			viewInfo.View.IsInteractable = true;

			if (viewInfo.Mode == Modes.Modal)
			{
				break;
			}
		}
	}

	#endregion
}

}

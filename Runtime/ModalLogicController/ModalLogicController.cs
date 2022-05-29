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

	public bool TryGetViewInfo(IPanel panel,
		out ViewInfo viewInfo)
	{
		var result = false;

		if (panel != null)
		{
			viewInfo = openViews.Find(info => info.Panel == panel);
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

	public void Add(IPanel panel,
		Modes mode)
	{
		Requires.NotNull(panel, nameof(panel));

		if (mode == Modes.Modal)
		{
			AddModal(panel);
		}
		else
		{
			Add(panel);
		}
	}

	public void Remove(IPanel panel,
		Modes mode)
	{
		Requires.NotNull(panel, nameof(panel));

		if (mode == Modes.Modal)
		{
			RemoveModal(panel);
		}
		else
		{
			Remove(panel);
		}
	}

	#endregion

	#region ModalLogicController

	private void Add(IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var panelInfo = openViews.Find(info => info.Panel == panel);

		if (panelInfo != null)
		{
			openViews.Remove(panelInfo);
			openViews.Add(panelInfo);
		}
		else
		{
			panelInfo = new ViewInfo(panel, Modes.None);
			openViews.Add(panelInfo);
		}
	}

	private void AddModal(IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var viewInfo = openViews.Find(info => info.Panel == panel);

		if (viewInfo != null)
		{
			openViews.Remove(viewInfo);
		}

		viewInfo = new ViewInfo(panel, Modes.Modal);

		foreach (var info in openViews)
		{
			info.Panel.IsInteractable = false;
		}

		openViews.Add(viewInfo);
	}

	private void Remove(IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var index = openViews.FindIndex(info => info.Panel == panel);

		if (index >= 0)
		{
			openViews.RemoveAt(index);
		}
	}

	private void RemoveModal(IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var index = openViews.FindIndex(info => info.Panel == panel);

		if (index < 0)
		{
			return;
		}

		openViews.RemoveAt(index);

		foreach (var viewInfo in openViews)
		{
			viewInfo.Panel.IsInteractable = true;

			if (viewInfo.Mode == Modes.Modal)
			{
				break;
			}
		}
	}

	#endregion
}

}
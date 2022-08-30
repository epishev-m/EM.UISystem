namespace EM.UI
{

using System.Collections.Generic;
using System.Linq;
using Foundation;

public class ModalLogicController
{
	private readonly List<PanelViewInfo> _openPanelsViews = new(16);

	#region ModalLogicController

	public bool TryGetPanelView<T>(out PanelView panelView)
		where T : PanelView
	{
		var panelViewInfo = _openPanelsViews.LastOrDefault(pv => pv.PanelView as T);
		panelView = panelViewInfo?.PanelView;

		return panelViewInfo != null;
	}

	public void Add(PanelView panel,
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

	public void Remove(PanelView panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var panelInfo = _openPanelsViews.Find(info => info.PanelView == panel);

		if (panelInfo.Mode == Modes.Modal)
		{
			RemoveModal(panelInfo);
		}
		else
		{
			Remove(panelInfo);
		}
	}

	private void Add(PanelView panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var panelInfo = _openPanelsViews.Find(info => info.PanelView == panel);

		if (panelInfo != null)
		{
			_openPanelsViews.Remove(panelInfo);
			_openPanelsViews.Add(panelInfo);
		}
		else
		{
			panelInfo = new PanelViewInfo(panel, Modes.None);
			_openPanelsViews.Add(panelInfo);
		}
	}

	private void AddModal(PanelView panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var viewInfo = _openPanelsViews.Find(info => info.PanelView == panel);

		if (viewInfo != null)
		{
			_openPanelsViews.Remove(viewInfo);
		}

		viewInfo = new PanelViewInfo(panel, Modes.Modal);

		foreach (var info in _openPanelsViews)
		{
			info.PanelView.IsInteractable = false;
		}

		_openPanelsViews.Add(viewInfo);
	}

	private void Remove(PanelViewInfo panelViewInfo)
	{
		Requires.NotNull(panelViewInfo, nameof(panelViewInfo));

		_openPanelsViews.Remove(panelViewInfo);
	}

	private void RemoveModal(PanelViewInfo panelViewInfo)
	{
		Requires.NotNull(panelViewInfo, nameof(panelViewInfo));

		if (!_openPanelsViews.Remove(panelViewInfo))
		{
			return;
		}

		foreach (var viewInfo in _openPanelsViews)
		{
			viewInfo.PanelView.IsInteractable = true;

			if (viewInfo.Mode == Modes.Modal)
			{
				break;
			}
		}
	}

	#endregion
}

}
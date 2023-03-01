namespace EM.UI
{

using System.Collections.Generic;
using System.Linq;
using Foundation;

public class ModalLogicController
{
	private readonly List<PanelViewInfo> _openPanelsViews = new(16);

	#region ModalLogicController

	public bool TryGetPanelView<TView>(out View panelView)
		where TView : View
	{
		var panelViewInfo = _openPanelsViews.LastOrDefault(pv => pv.View is TView);
		panelView = panelViewInfo?.View;

		return panelViewInfo != null;
	}

	public void Add(View panel,
		Modes mode)
	{
		Requires.NotNullParam(panel, nameof(panel));

		if (mode == Modes.Modal)
		{
			AddModal(panel);
		}
		else
		{
			Add(panel);
		}
	}

	public void Remove(View panel)
	{
		Requires.NotNullParam(panel, nameof(panel));

		var panelInfo = _openPanelsViews.Find(info => info.View == panel);

		if (panelInfo.Mode == Modes.Modal)
		{
			RemoveModal(panelInfo);
		}
		else
		{
			Remove(panelInfo);
		}
	}

	private void Add(View panel)
	{
		Requires.NotNullParam(panel, nameof(panel));

		var panelInfo = _openPanelsViews.Find(info => info.View == panel);

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

	private void AddModal(View panel)
	{
		Requires.NotNullParam(panel, nameof(panel));

		var viewInfo = _openPanelsViews.Find(info => info.View == panel);

		if (viewInfo != null)
		{
			_openPanelsViews.Remove(viewInfo);
		}

		viewInfo = new PanelViewInfo(panel, Modes.Modal);

		foreach (var info in _openPanelsViews)
		{
			info.View.IsInteractable = false;
		}

		_openPanelsViews.Add(viewInfo);
	}

	private void Remove(PanelViewInfo panelViewInfo)
	{
		Requires.NotNullParam(panelViewInfo, nameof(panelViewInfo));

		_openPanelsViews.Remove(panelViewInfo);
	}

	private void RemoveModal(PanelViewInfo panelViewInfo)
	{
		Requires.NotNullParam(panelViewInfo, nameof(panelViewInfo));

		if (!_openPanelsViews.Remove(panelViewInfo))
		{
			return;
		}

		foreach (var viewInfo in _openPanelsViews)
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
namespace EM.UI
{
using Foundation;
using System;
using System.Collections.Generic;

public sealed class ModesController :
	IModesController
{
	private readonly List<PanelInfo> openPanels;

	#region IModesController

	public bool TryGetPanelInfo(
		IPanel panel,
		out PanelInfo panelInfo)
	{
		var result = false;

		if (panel != null)
		{
			panelInfo = openPanels.Find(info => info.Panel == panel);
			result = panelInfo != null;
		}
		else
		{
			panelInfo = null;
		}

		return result;
	}

	public void PrepareAdd(
		Modes mode,
		Action onCompleted)
	{
		onCompleted?.Invoke();
	}

	public void PrepareRemove(
		Modes mode,
		Action onCompleted)
	{
		onCompleted?.Invoke();
	}

	public void Add(
		IPanel panel,
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

	public void Remove(
		IPanel panel,
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
	#region ModesController

	public ModesController()
	{
		openPanels = new List<PanelInfo>(16);
	}

	private void Add(
		IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var panelInfo = openPanels.Find(info => info.Panel == panel);

		if (panelInfo != null)
		{
			openPanels.Remove(panelInfo);
			openPanels.Add(panelInfo);
		}
		else
		{
			panelInfo = new PanelInfo(panel, Modes.None);
			openPanels.Add(panelInfo);
		}
	}

	private void AddModal(
		IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var panelInfo = openPanels.Find(info => info.Panel == panel);

		if (panelInfo != null)
		{
			openPanels.Remove(panelInfo);
		}

		panelInfo = new PanelInfo(panel, Modes.Modal);

		foreach (var info in openPanels)
		{
			info.Panel.IsInteractable = false;
		}

		openPanels.Add(panelInfo);
	}

	private void Remove(
		IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var index = openPanels.FindIndex(info => info.Panel == panel);

		if (index >= 0)
		{
			openPanels.RemoveAt(index);
		}
	}

	private void RemoveModal(
		IPanel panel)
	{
		Requires.NotNull(panel, nameof(panel));

		var index = openPanels.FindIndex(info => info.Panel == panel);

		if (index < 0)
		{
			return;
		}

		openPanels.RemoveAt(index);

		foreach (var infoPanel in openPanels)
		{
			infoPanel.Panel.IsInteractable = true;

			if (infoPanel.Mode == Modes.Modal)
			{
				break;
			}
		}
	}

	#endregion
}

}

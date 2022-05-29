namespace EM.UI
{

using System;

public interface IPanel
{
	bool IsOpened
	{
		get;
	}

	bool IsInteractable
	{
		get;
		set;
	}

	void Open(Action onPanelOpened);

	void Close(Action onPanelClosed);
}

}
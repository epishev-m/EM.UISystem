namespace EM.UI
{
using System;

public interface IPanel
{
	string Name
	{
		get;
	}

	bool IsOpened
	{
		get;
	}

	bool IsInteractable
	{
		get;
		set;
	}

	void Open(
		Action onPanelOpened);

	void Close(
		Action onPanelClosed);
}

}

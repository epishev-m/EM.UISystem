namespace EM.UI
{
using Foundation;
using System;
using UnityEngine;

public sealed class Panel :
	IPanel
{
	private readonly GameObject panel;

	private readonly IPanelAnimation animation;

	private readonly Canvas canvas;

	private readonly CanvasGroup canvasGroup;

	#region IPanel

	public string Name => panel.name;

	public bool IsOpened => canvas.enabled;

	public bool IsInteractable
	{
		get => canvasGroup.blocksRaycasts;
		set => canvasGroup.blocksRaycasts = value;
	}

	public void Open(
		Action onPanelOpened)
	{
		panel.transform.SetAsLastSibling();

		if (canvas.enabled)
		{
			onPanelOpened?.Invoke();
		}
		else
		{
			canvas.enabled = true;

			if (animation == null)
			{
				OnPanelShowed();
			}
			else
			{
				animation.Show(panel, OnPanelShowed);
			}
		}

		void OnPanelShowed()
		{
			canvasGroup.blocksRaycasts = true;
			onPanelOpened?.Invoke();
		}
	}

	public void Close(
		Action onPanelClosed)
	{
		if (canvas.enabled == false)
		{
			onPanelClosed?.Invoke();
		}
		else
		{
			canvasGroup.blocksRaycasts = false;

			if (animation == null)
			{
				OnPanelHided();
			}
			else
			{
				animation.Hide(panel, OnPanelHided);
			}
		}

		void OnPanelHided()
		{
			canvas.enabled = false;
			onPanelClosed?.Invoke();
		}
	}

	#endregion
	#region UIPanel

	public Panel(
		GameObject panel,
		IPanelAnimation animation)
	{
		Requires.NotNull(panel, nameof(panel));

		this.panel = panel;
		this.animation = animation;

		canvas = panel.GetComponent<Canvas>();
		canvasGroup = panel.GetComponent<CanvasGroup>();

		Requires.NotNull(canvas, nameof(canvas));
		Requires.NotNull(canvasGroup, nameof(canvasGroup));

		canvas.enabled = false;
		canvasGroup.blocksRaycasts = false;
	}

	#endregion
}

}

namespace EM.UI
{
using Foundation;
using System;
using UnityEngine;

public sealed class Panel :
	IPanel
{
	private readonly Canvas canvas;

	private readonly CanvasGroup canvasGroup;

	private readonly IPanelAnimation animation;

	#region IPanel

	public string Name => canvas.name;

	public bool IsOpened => canvas.enabled;

	public bool IsInteractable
	{
		get => canvasGroup.blocksRaycasts;
		set => canvasGroup.blocksRaycasts = value;
	}

	public void Open(
		Action onPanelOpened)
	{
		canvas.transform.SetAsLastSibling();

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
				animation.Show(canvas.gameObject, OnPanelShowed);
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
				OnPanelHidden();
			}
			else
			{
				animation.Hide(canvas.gameObject, OnPanelHidden);
			}
		}

		void OnPanelHidden()
		{
			canvas.enabled = false;
			onPanelClosed?.Invoke();
		}
	}

	#endregion
	#region UIPanel

	public Panel(
		Canvas canvas)
	{
		Requires.NotNull(canvas, nameof(canvas));

		this.canvas = canvas;

		canvasGroup = canvas.GetComponent<CanvasGroup>();
		animation = canvas.GetComponent<IPanelAnimation>();

		Requires.NotNull(canvasGroup, nameof(canvasGroup));

		canvas.enabled = false;
		canvasGroup.blocksRaycasts = false;
	}

	#endregion
}

}

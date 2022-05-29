namespace EM.UI
{

using System;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public abstract class Panel :
	MonoBehaviour,
	IPanel
{
	[Header(nameof(Panel))]

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private CanvasGroup canvasGroup;

	private bool isInitialized;

	#region IPanel

	public bool IsOpened => canvas.enabled;

	public bool IsInteractable
	{
		get => canvasGroup.blocksRaycasts;
		set => canvasGroup.blocksRaycasts = value;
	}

	public void Open(Action onPanelOpened)
	{
		Initialize();
		transform.SetAsLastSibling();

		if (canvas.enabled == false)
		{
			canvas.enabled = true;
			canvasGroup.blocksRaycasts = true;
		}

		onPanelOpened?.Invoke();
	}

	public void Close(Action onPanelClosed)
	{
		if (canvas.enabled)
		{
			canvasGroup.blocksRaycasts = false;
			canvas.enabled = false;
		}

		Release();
		onPanelClosed?.Invoke();
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		if (canvas == null)
		{
			canvas = GetComponent<Canvas>();
		}

		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		canvas.enabled = false;
		canvasGroup.blocksRaycasts = false;
	}

	#endregion

	#region Panel

	private void Initialize()
	{
		if (isInitialized)
		{
			Release();
		}

		OnInitialize();
		isInitialized = true;
	}

	private void Release()
	{
		if (!isInitialized)
		{
			return;
		}

		OnRelease();
		isInitialized = false;
	}

	protected abstract void OnInitialize();

	protected abstract void OnRelease();

	#endregion
}

}
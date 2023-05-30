namespace EM.UI
{

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public abstract class View : MonoBehaviour
{
	[Header(nameof(View))]

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	#region MonoBehaviour

	protected virtual void Awake()
	{
		if (_canvas == null)
		{
			_canvas = GetComponent<Canvas>();
		}

		if (_canvasGroup == null)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		_canvas.enabled = false;
		_canvasGroup.blocksRaycasts = false;
	}

	#endregion

	#region View

	public bool IsOpened => _canvas.enabled;

	public bool IsInteractable
	{
		get => _canvasGroup.blocksRaycasts;
		set => _canvasGroup.blocksRaycasts = value;
	}

	public virtual void SetViewModel(object viewModel)
	{
		throw new NotImplementedException();
	}

	public virtual UniTask OpenAsync(CancellationToken ct)
	{
		transform.SetAsLastSibling();

		if (_canvas.enabled)
		{
			return UniTask.CompletedTask;
		}

		_canvas.enabled = true;
		_canvasGroup.blocksRaycasts = true;

		return UniTask.CompletedTask;
	}

	public virtual UniTask CloseAsync(CancellationToken ct)
	{
		if (!_canvas.enabled)
		{
			return UniTask.CompletedTask;
		}

		_canvasGroup.blocksRaycasts = false;
		_canvas.enabled = false;

		return UniTask.CompletedTask;
	}

	#endregion
}

public abstract class View<T> : View
	where T : IViewModel
{
	protected T ViewModel;

	protected CancellationTokenSource CtsInstance;

	#region View

	public override void SetViewModel(object viewModel)
	{
		Requires.NotNullParam(viewModel, nameof(viewModel));

		ViewModel = (T) viewModel;
	}

	public override async UniTask OpenAsync(CancellationToken ct)
	{
		await base.OpenAsync(ct);
		CtsInstance = new CancellationTokenSource();
		OnInitialize();
		ViewModel.Initialize();
	}

	public override async UniTask CloseAsync(CancellationToken ct)
	{
		await base.CloseAsync(ct);
		CtsInstance.Cancel();
		CtsInstance = null;
		OnRelease();
		ViewModel.Release();
	}

	#endregion

	#region View<T>

	protected virtual void OnInitialize()
	{
	}

	protected virtual void OnRelease()
	{
	}

	#endregion
}

}
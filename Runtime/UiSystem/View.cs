namespace EM.UI
{

using System;
using System.Collections.Generic;
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

	public abstract void SetViewModel(object viewModel);

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
	where T : class
{
	protected T ViewModel;

	private readonly PropertyHandlerPool _propertyHandlerPool = new();

	private readonly List<IDisposable> _listOfDisposeAtClosing  = new();

	#region View

	public override void SetViewModel(object viewModel)
	{
		Requires.NotNullParam(viewModel, nameof(viewModel));

		ViewModel = (T)viewModel;
		OnInitialize();
	}

	public override async UniTask CloseAsync(CancellationToken ct)
	{
		await base.CloseAsync(ct);
		ListDispose();
		ViewModelDispose();
	}

	#endregion

	protected abstract void OnInitialize();

	protected void Subscribe<TValue>(IRxProperty<TValue> property,
		Action<TValue> handler)
	{
		SubscribeInternal(property, () => handler(property.Value));
	}

	private void SubscribeInternal(IEventProvider eventProvider,
		Action handler)
	{
		var propertyHandler = _propertyHandlerPool.Get(eventProvider, handler);
		_listOfDisposeAtClosing.Add(propertyHandler);
	}

	private void ListDispose()
	{
		foreach (var disposable in _listOfDisposeAtClosing)
		{
			disposable.Dispose();
		}

		_listOfDisposeAtClosing.Clear();
	}

	private void ViewModelDispose()
	{
		if (ViewModel is IDisposable disposable)
		{
			disposable.Dispose();
		}

		ViewModel = null;
	}
}

}
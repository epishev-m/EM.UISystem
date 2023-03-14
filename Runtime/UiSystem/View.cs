namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using UnityEngine;
using UnityEngine.Events;

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

	private readonly EventProviderHandlerPool _eventProviderHandlerPool = new();

	private readonly List<EventProviderHandler> _eventProviderHandlers  = new();

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
		OnRelease();
		ListDispose();
		ViewModelDispose();
	}

	#endregion

	#region View<T>

	public void Subscribe<TValue>(IRxProperty<TValue> property,
		Action<TValue> handler)
	{
		Requires.NotNullParam(property, nameof(property));
		Requires.NotNullParam(handler, nameof(handler));

		SubscribeInternal(property, () => handler(property.Value));
	}

	public void Subscribe(UnityEvent unityEvent,
		Action handler)
	{
		Requires.NotNullParam(unityEvent, nameof(unityEvent));
		Requires.NotNullParam(handler, nameof(handler));

		SubscribeInternal(unityEvent, handler);
	}

	public void Subscribe<TValue>(UnityEvent<TValue> unityEvent,
		Action<TValue> handler)
	{
		Requires.NotNullParam(unityEvent, nameof(unityEvent));
		Requires.NotNullParam(handler, nameof(handler));

		SubscribeInternal(unityEvent, handler);
	}

	protected virtual void OnInitialize()
	{
	}

	protected virtual void OnRelease()
	{
	}

	private void SubscribeInternal(IEventProvider eventProvider,
		Action handler)
	{
		var eventProviderHandler = _eventProviderHandlerPool.Get(eventProvider, handler);
		_eventProviderHandlers.Add(eventProviderHandler);
	}

	private void SubscribeInternal(UnityEvent unityEvent,
		Action handler)
	{
		var unityEventProviderHandler = new UnityEventProvider(unityEvent);
		var eventProviderHandler = _eventProviderHandlerPool.Get(unityEventProviderHandler, handler);
		_eventProviderHandlers.Add(eventProviderHandler);
	}

	private void SubscribeInternal<TValue>(UnityEvent<TValue> unityEvent,
		Action<TValue> handler)
	{
		unityEvent.AddListener(handler.Invoke);
		var unityEventProviderHandler = new UnityEventProvider<TValue>(unityEvent);
		var eventProviderHandler = _eventProviderHandlerPool.Get(unityEventProviderHandler, null);
		_eventProviderHandlers.Add(eventProviderHandler);
	}

	private void ListDispose()
	{
		foreach (var propertyHandler in _eventProviderHandlers)
		{
			propertyHandler.Dispose();
			_eventProviderHandlerPool.Put(propertyHandler);
		}

		_eventProviderHandlers.Clear();
	}

	private void ViewModelDispose()
	{
		if (ViewModel is IDisposable disposable)
		{
			disposable.Dispose();
		}

		ViewModel = null;
	}

	#endregion
}

}
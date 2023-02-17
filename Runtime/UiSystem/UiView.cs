namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class UIView : MonoBehaviour
{
	[Header(nameof(UIView))]
	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	protected IViewModel ViewModelBase;

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

	#region UIView

	public bool IsOpened => _canvas.enabled;

	public bool IsInteractable
	{
		get => _canvasGroup.blocksRaycasts;
		set => _canvasGroup.blocksRaycasts = value;
	}

	public void Initialize(IViewModel viewModel)
	{
		ViewModelBase = viewModel;
		ViewModelBase?.Initialize();
		OnInitialize();
	}

	public void Release()
	{
		OnRelease();
		ViewModelBase?.Release();
		ViewModelBase = null;
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

	protected abstract void OnInitialize();

	protected abstract void OnRelease();

	#endregion
}

public abstract class UiView<T> : UIView
	where T : class, IViewModel
{
	protected T ViewModel => ViewModelBase as T;
}

}
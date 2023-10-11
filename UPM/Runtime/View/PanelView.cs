namespace EM.UI
{

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public abstract class PanelView : MonoBehaviour
{
	[Header(nameof(PanelView))]

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	protected List<WidgetView> Widgets = new(4);

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

	#region PanelView

	public bool IsOpened => _canvas.enabled;

	public bool IsInteractable
	{
		get => _canvasGroup.blocksRaycasts;
		set => _canvasGroup.blocksRaycasts = value;
	}

	public virtual void SetViewModel(object viewModel)
	{
		OnSettingViewModel();
	}

	public virtual UniTask OpenAsync(CancellationToken ct)
	{
		transform.SetAsLastSibling();

		if (_canvas.enabled)
		{
			return UniTask.CompletedTask;
		}

		InitializeWidgets();
		OnInitialize();
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
		OnRelease();
		ReleaseWidgets();

		return UniTask.CompletedTask;
	}

	protected void AddWidget(WidgetView widget,
		IViewModel viewModel)
	{
		Requires.NotNullParam(widget, nameof(widget));
		Requires.NotNullParam(viewModel, nameof(viewModel));

		widget.SetViewModel(viewModel);
		AddWidget(widget);
	}

	protected void AddWidget(WidgetView widget)
	{
		Requires.NotNullParam(widget, nameof(widget));
		Requires.ValidOperation(Widgets.All(view => view != widget), this);

		Widgets.Add(widget);
	}

	protected virtual void OnSettingViewModel()
	{
	}

	protected virtual void OnInitialize()
	{
	}

	protected virtual void OnRelease()
	{
	}

	private void InitializeWidgets()
	{
		foreach (var widget in Widgets)
		{
			widget.Initialize();
		}
	}

	private void ReleaseWidgets()
	{
		foreach (var widget in Widgets)
		{
			widget.Release();
		}

		Widgets.Clear();
	}

	#endregion
}

public abstract class PanelView<T> : PanelView
	where T : IViewModel
{
	protected T ViewModel;

	protected CancellationTokenSource CtsInstance;

	#region PanelView

	public override void SetViewModel(object viewModel)
	{
		Requires.NotNullParam(viewModel, nameof(viewModel));
		Requires.Type<T>(viewModel, nameof(viewModel));

		CtsInstance = new CancellationTokenSource();
		ViewModel = (T) viewModel;
		base.SetViewModel(viewModel);
	}

	public override async UniTask OpenAsync(CancellationToken ct)
	{
		ViewModel.Initialize();
		await base.OpenAsync(ct);
	}

	public override async UniTask CloseAsync(CancellationToken ct)
	{
		CtsInstance.Cancel();
		CtsInstance = null;
		await base.CloseAsync(ct);
		ViewModel.Release();
		ViewModel = default;
	}

	#endregion
}

}


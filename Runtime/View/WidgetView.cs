namespace EM.UI
{

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Foundation;
using UnityEngine;

public abstract class WidgetView : MonoBehaviour
{
	protected List<WidgetView> Widgets = new(4);

	#region WidgetView

	public virtual void SetViewModel(object viewModel)
	{
		OnSettingViewModel();
	}

	public virtual void Initialize()
	{
		InitializeWidgets();
		OnInitialize();
	}

	public virtual void Release()
	{
		OnRelease();
		ReleaseWidgets();
	}
	
	protected bool TryAddWidget(WidgetView widget,
		IViewModel viewModel)
	{
		if (widget == null || viewModel == null)
		{
			return false;
		}

		Requires.ValidOperation(Widgets.All(view => view != widget), this);

		widget.SetViewModel(viewModel);
		AddWidget(widget);

		return true;
	}

	protected bool TryAddWidget(WidgetView widget)
	{
		if (widget == null)
		{
			return false;
		}

		Requires.ValidOperation(Widgets.All(view => view != widget), this);

		Widgets.Add(widget);

		return true;
	}

	protected void AddWidget(WidgetView widget,
		IViewModel viewModel)
	{
		Requires.NotNullParam(widget, nameof(widget));
		Requires.NotNullParam(viewModel, nameof(viewModel));
		Requires.ValidOperation(Widgets.All(view => view != widget), this);

		widget.SetViewModel(viewModel);
		AddWidget(widget);
	}

	protected void AddWidget(WidgetView widget)
	{
		Requires.NotNullParam(widget, nameof(widget));
		Requires.ValidOperation(Widgets.All(view => view != widget), this);

		Widgets.Add(widget);
	}

	protected virtual void OnInitialize()
	{
	}

	protected virtual void OnRelease()
	{
	}

	protected virtual void OnSettingViewModel()
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

public abstract class WidgetView<T> : WidgetView
	where T : IViewModel
{
	protected T ViewModel;

	protected CancellationTokenSource CtsInstance;

	#region WidgetView

	public override void SetViewModel(object viewModel)
	{
		Requires.NotNullParam(viewModel, nameof(viewModel));

		ViewModel = (T) viewModel;
		CtsInstance = new CancellationTokenSource();
		base.SetViewModel(viewModel);
	}

	public override void Initialize()
	{
		ViewModel.Initialize();
		base.Initialize();
	}

	public override void Release()
	{
		CtsInstance.Cancel();
		CtsInstance = null;
		base.Release();
		ViewModel.Release();
		ViewModel = default;
	}

	#endregion
}

}
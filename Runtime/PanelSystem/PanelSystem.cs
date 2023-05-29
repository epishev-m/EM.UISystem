namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;

public sealed class PanelSystem : IPanelSystem
{
	private readonly IViewModelFactory _viewModelFactory;

	private readonly ModalLogicController _modalLogicController = new();

	private readonly IUiRoot _uiRoot;

	#region IPanelSystem

	public async UniTask OpenAsync<TView>(CancellationToken ct)
		where TView : View
	{
		await OpenWithoutViewModelAsync<TView>(Modes.None, ct);
	}

	public async UniTask OpenAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : View
	{
		await OpenWithoutViewModelAsync<TView>(mode, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel>(CancellationToken ct)
		where TView : View
		where TViewModel : class
	{
		var viewModel = _viewModelFactory.Get<TViewModel>();
		await OpenWithViewModelAsync<TView, TViewModel>(Modes.None, viewModel, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel, TData>(TData data,
		CancellationToken ct)
		where TView : View
		where TViewModel : ViewModel<TData>
	{
		var viewModel = _viewModelFactory.Get<TViewModel>();
		viewModel.SetData(data);
		await OpenWithViewModelAsync<TView, TViewModel>(Modes.None, viewModel, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel>(Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : class
	{
		var viewModel = _viewModelFactory.Get<TViewModel>();
		await OpenWithViewModelAsync<TView, TViewModel>(mode, viewModel, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel, TData>(TData data,
		Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : ViewModel<TData>
	{
		var viewModel = _viewModelFactory.Get<TViewModel>();
		viewModel.SetData(data);
		await OpenWithViewModelAsync<TView, TViewModel>(mode, viewModel, ct);
	}

	public async UniTask CloseAsync<TView>(CancellationToken ct)
		where TView : View
	{
		if (!_modalLogicController.TryGetPanelView<TView>(out var panel))
		{
			return;
		}

		await panel.CloseAsync(ct);
		_modalLogicController.Remove(panel);
	}

	#endregion

	#region UiViewController

	public PanelSystem(IUiRoot uiRoot,
		IViewModelFactory viewModelFactory)
	{
		_uiRoot = uiRoot;
		_viewModelFactory = viewModelFactory;
	}

	private async UniTask OpenWithoutViewModelAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : View
	{
		var panel = await _uiRoot.GetPanelViewAsync(typeof(TView), ct);
		await panel.OpenAsync(ct);
		_modalLogicController.Add(panel, mode);
	}

	private async UniTask OpenWithViewModelAsync<TView, TViewModel>(Modes mode,
		TViewModel viewModel,
		CancellationToken ct)
		where TView : View
		where TViewModel : class
	{
		var panel = await _uiRoot.GetPanelViewAsync(typeof(TView), ct);
		panel.SetViewModel(viewModel);
		await panel.OpenAsync(ct);
		_modalLogicController.Add(panel, mode);
	}

	#endregion
}

}
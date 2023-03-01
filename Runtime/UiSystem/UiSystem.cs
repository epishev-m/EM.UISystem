namespace EM.UI
{

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using IoC;

public sealed class UiSystem : IUiSystem
{
	private readonly IDiContainer _diContainer;

	private readonly IAssetsManager _assetsManager;

	private readonly ModalLogicController _modalLogicController = new();

	private IUiRoot _uiRoot;

	#region IUiSystem

	public async UniTask CreateUiRootAsync(string assetId,
		CancellationToken ct)
	{
		Requires.ValidOperation(_uiRoot == null, this);

		_uiRoot = new UiRoot(_assetsManager);
		await _uiRoot.CreateRootTransform(assetId, ct);
	}

	public async UniTask LoadAsync<TView>(CancellationToken ct)
		where TView : View
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (_uiRoot != null)
		{
			await _uiRoot.LoadPanelViewAsync<TView>(ct);
		}
	}

	public void Unload(LifeTime lifeTime)
	{
		if (_uiRoot != null)
		{
			_uiRoot.UnloadPanelView(lifeTime);
		}
	}

	public async UniTask OpenAsync<TView>(CancellationToken ct)
		where TView : View
	{
		await OpenWithoutViewModelAsync<TView>(Modes.None, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel>(Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : class
	{
		var viewModel = _diContainer.Resolve<TViewModel>();
		await OpenWithViewModelAsync<TView, TViewModel>(mode, viewModel, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel>(CancellationToken ct)
		where TView : View
		where TViewModel : class
	{
		var viewModel = _diContainer.Resolve<TViewModel>();
		await OpenWithViewModelAsync<TView, TViewModel>(Modes.None, viewModel, ct);
	}

	public async UniTask OpenAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : View
	{
		await OpenWithoutViewModelAsync<TView>(mode, ct);
	}

	public async UniTask CloseAsync<TView>(CancellationToken ct)
		where TView : View
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (!_modalLogicController.TryGetPanelView<TView>(out var panel))
		{
			return;
		}

		await panel.CloseAsync(ct);
		_modalLogicController.Remove(panel);

		if (panel is IDisposable dispose)
		{
			dispose.Dispose();
		}
	}

	private async UniTask OpenWithoutViewModelAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : View
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (_uiRoot == null)
		{
			return;
		}

		var panel = await _uiRoot.GetPanelViewAsync<TView>(ct);
		await panel.OpenAsync(ct);
		_modalLogicController.Add(panel, mode);
	}

	private async UniTask OpenWithViewModelAsync<TView, TViewModel>(Modes mode,
		TViewModel viewModel,
		CancellationToken ct)
		where TView : View
		where TViewModel : class
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (_uiRoot == null)
		{
			return;
		}

		var panel = await _uiRoot.GetPanelViewAsync<TView>(ct);

		Requires.NotNull(panel, nameof(panel));

		if (panel == null)
		{
			return;
		}

		panel.SetViewModel(viewModel);
		await panel.OpenAsync(ct);
		_modalLogicController.Add(panel, mode);
	}

	#endregion

	#region UiSystem

	public UiSystem(IDiContainer diContainer,
		IAssetsManager assetsManager)
	{
		_diContainer = diContainer;
		_assetsManager = assetsManager;
	}

	#endregion
}

}
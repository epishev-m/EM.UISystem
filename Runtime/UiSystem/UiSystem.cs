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
		where TView : UIView
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
		where TView : UIView
	{
		await OpenAsync<TView>(Modes.None, null, ct);
	}

	public async UniTask OpenAsync<TView, TViewModel>(Modes mode,
		CancellationToken ct)
		where TView : UIView
		where TViewModel : IViewModel
	{
		await OpenAsync<TView>(mode, typeof(TViewModel), ct);
	}

	public async UniTask OpenAsync<TView, TViewModel>(CancellationToken ct)
		where TView : UIView
		where TViewModel : IViewModel
	{
		await OpenAsync<TView>(Modes.None, typeof(TViewModel), ct);
	}

	public async UniTask OpenAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : UIView
	{
		await OpenAsync<TView>(mode, null, ct);
	}

	public async UniTask CloseAsync<TView>(CancellationToken ct)
		where TView : UIView
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (!_modalLogicController.TryGetPanelView<TView>(out var panel))
		{
			return;
		}

		await panel.CloseAsync(ct);
		panel.Release();
		_modalLogicController.Remove(panel);
	}

	private async UniTask OpenAsync<TView>(Modes mode,
		Type viewModelType,
		CancellationToken ct)
		where TView : UIView
	{
		Requires.ValidOperation(_uiRoot != null, this);

		var panel = await _uiRoot.GetPanelViewAsync<TView>(ct);
		IViewModel viewModel = default;

		if (viewModelType != null)
		{
			viewModel = _diContainer.Resolve(viewModelType) as IViewModel;
		}

		panel.Initialize(viewModel);
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
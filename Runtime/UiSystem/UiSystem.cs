namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public sealed class UiSystem : IUiSystem
{
	private readonly IMediationContainer _mediationContainer;

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

	public async UniTask LoadAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (_uiRoot != null)
		{
			await _uiRoot.LoadPanelViewAsync<T>(ct);
		}
	}

	public void Unload(LifeTime lifeTime)
	{
		if (_uiRoot != null)
		{
			_uiRoot.UnloadPanelView(lifeTime);
		}
	}

	public async UniTask OpenAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		await OpenAsync<T>(Modes.None, ct);
	}

	public async UniTask OpenAsync<T>(Modes mode,
		CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(_uiRoot != null, this);

		var panel = await _uiRoot.GetPanelViewAsync<T>(ct);

		if (panel.IsOpened)
		{
			return;
		}

		await panel.OpenAsync(ct);
		_modalLogicController.Add(panel, mode);
		_mediationContainer.Trigger(MediationTrigger.Initialise, panel);
	}

	public async UniTask CloseAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(_uiRoot != null, this);

		if (!_modalLogicController.TryGetPanelView<T>(out var panel))
		{
			return;
		}

		await panel.CloseAsync(ct);
		_modalLogicController.Remove(panel);
		_mediationContainer.Trigger(MediationTrigger.Release, panel);
	}

	#endregion

	#region UiSystem

	public UiSystem(IMediationContainer mediationContainer,
		IAssetsManager assetsManager)
	{
		_mediationContainer = mediationContainer;
		_assetsManager = assetsManager;
	}

	#endregion
}

}
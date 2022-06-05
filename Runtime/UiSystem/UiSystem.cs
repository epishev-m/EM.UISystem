namespace EM.UI
{

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using IoC;

public sealed class UiSystem :
	IUiSystem
{
	private readonly IMediationContainer mediationContainer;

	private readonly IAssetsManager assetsManager;

	private readonly ModalLogicController modalLogicController = new();

	private IUiRoot uiRoot;

	#region IUiSystem

	public async UniTask CreateUiRootAsync(string assetId,
		CancellationToken ct)
	{
		Requires.ValidOperation(uiRoot == null, this, nameof(CreateUiRootAsync));

		uiRoot = new UiRoot(assetsManager);
		await uiRoot.CreateRootTransform(assetId, ct);
	}

	public async UniTask LoadAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(uiRoot != null, this, nameof(LoadAsync));

		if (uiRoot != null)
		{
			await uiRoot.LoadPanelViewAsync<T>(ct);
		}
	}

	public void Unload(LifeTime lifeTime)
	{
		if (uiRoot != null)
		{
			uiRoot.UnloadPanelView(lifeTime);
		}
	}

	public async UniTask OpenAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		await OpenAsync<T>(Modes.None, ct);
	}

	public async UniTask OpenAsync<T>(Modes mode, CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(uiRoot != null, this, nameof(OpenAsync));

		var panel = await uiRoot.GetPanelViewAsync<T>(ct);

		if (panel.IsOpened)
		{
			return;
		}

		await panel.OpenAsync(ct);
		modalLogicController.Add(panel, mode);
		mediationContainer.Trigger(MediationTrigger.Initialise, panel);
	}

	public async UniTask CloseAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(uiRoot != null, this, nameof(CloseAsync));

		if (!modalLogicController.TryGetPanelView<T>(out var panel))
		{
			return;
		}

		await panel.CloseAsync(ct);
		modalLogicController.Remove(panel);
		mediationContainer.Trigger(MediationTrigger.Release, panel);
	}

	#endregion

	#region UiSystem

	public UiSystem(IMediationContainer mediationContainer,
		IAssetsManager assetsManager)
	{
		this.mediationContainer = mediationContainer;
		this.assetsManager = assetsManager;
	}

	#endregion
}

}
namespace EM.UI
{

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiRoot
{
	void CreateRootTransform(string assetId);

	UniTask LoadPanelViewAsync(Type type,
		CancellationToken ct);

	void UnloadPanelView(LifeTime lifeTime);

	UniTask<PanelView> GetPanelViewAsync(Type type,
		CancellationToken ct);
}

}
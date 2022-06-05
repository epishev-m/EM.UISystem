namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiRoot
{
	UniTask CreateRootTransform(string id,
		CancellationToken ct);

	UniTask LoadPanelViewAsync<T>(CancellationToken ct)
		where T : PanelView;

	void UnloadPanelView(LifeTime lifeTime);

	UniTask<PanelView> GetPanelViewAsync<T>(CancellationToken ct)
		where T : PanelView;
}

}
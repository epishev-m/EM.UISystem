namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiRoot
{
	UniTask CreateRootTransform(string id,
		CancellationToken ct);

	UniTask LoadPanelViewAsync<TView>(CancellationToken ct)
		where TView : View;

	void UnloadPanelView(LifeTime lifeTime);

	UniTask<TView> GetPanelViewAsync<TView>(CancellationToken ct)
		where TView : View;
}

}
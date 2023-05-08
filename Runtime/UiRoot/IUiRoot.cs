namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiRoot
{
	void CreateRootTransform(string id);

	UniTask LoadPanelViewAsync<TView>(CancellationToken ct)
		where TView : View;

	void UnloadPanelView(LifeTime lifeTime);

	UniTask<TView> GetPanelViewAsync<TView>(CancellationToken ct)
		where TView : View;
}

}
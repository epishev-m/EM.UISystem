namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiSystem
{
	UniTask CreateUiRootAsync(string id,
		CancellationToken ct);

	UniTask LoadAsync<T>(CancellationToken ct)
		where T : PanelView;

	void Unload(LifeTime lifeTime);
	
	UniTask OpenAsync<T>(CancellationToken ct)
		where T : PanelView;

	UniTask OpenAsync<T>(Modes mode,
		CancellationToken ct)
		where T : PanelView;

	UniTask CloseAsync<T>(CancellationToken ct)
		where T : PanelView;
}

}
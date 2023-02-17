namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiSystem
{
	UniTask CreateUiRootAsync(string id,
		CancellationToken ct);

	UniTask LoadAsync<TView>(CancellationToken ct)
		where TView : UIView;

	void Unload(LifeTime lifeTime);

	UniTask OpenAsync<TView>(CancellationToken ct)
		where TView : UIView;

	UniTask OpenAsync<TView, TViewModel>(CancellationToken ct)
		where TView : UIView
		where TViewModel : IViewModel;

	UniTask OpenAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : UIView;

	UniTask OpenAsync<TView, TViewModel>(Modes mode,
		CancellationToken ct)
		where TView : UIView
		where TViewModel : IViewModel;

	UniTask CloseAsync<TView>(CancellationToken ct)
		where TView : UIView;
}

}
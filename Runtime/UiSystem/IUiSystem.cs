namespace EM.UI
{
	using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IUiSystem
{
	void CreateUiRootAsync(string id);

	UniTask LoadAsync<TView>(CancellationToken ct)
		where TView : View;

	void Unload(LifeTime lifeTime);

	UniTask OpenAsync<TView>(CancellationToken ct)
		where TView : View;

	UniTask OpenAsync<TView>(Modes mode,
		CancellationToken ct)
		where TView : View;

	UniTask OpenAsync<TView, TViewModel>(CancellationToken ct)
		where TView : View
		where TViewModel : class;

	UniTask OpenAsync<TView, TViewModel, TData>(TData data,
		CancellationToken ct)
		where TView : View
		where TViewModel : PayloadViewModel<TData>
		where TData : class;

	UniTask OpenAsync<TView, TViewModel>(Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : class;

	UniTask OpenAsync<TView, TViewModel, TData>(TData data,
		Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : PayloadViewModel<TData>
		where TData : class;

	UniTask CloseAsync<TView>(CancellationToken ct)
		where TView : View;
}

}
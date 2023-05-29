namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;

public interface IPanelSystem
{
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
		where TViewModel : ViewModel<TData>;

	UniTask OpenAsync<TView, TViewModel>(Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : class;

	UniTask OpenAsync<TView, TViewModel, TData>(TData data,
		Modes mode,
		CancellationToken ct)
		where TView : View
		where TViewModel : ViewModel<TData>;

	UniTask CloseAsync<TView>(CancellationToken ct)
		where TView : View;
}

}
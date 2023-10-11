namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;

public interface IScreenSystem
{
	IScreenSystemLifeTimeBinding Bind(object key);

	void Unbind(LifeTime lifeTime);

	UniTask OpenAsync(object key,
		CancellationToken ct);

	UniTask OpenAsync(object key,
		object data,
		CancellationToken ct);

	UniTask BackAsync(CancellationToken ct);
}

}
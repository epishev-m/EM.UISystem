namespace EM.UI
{

using System.Threading.Tasks;
using IoC;

public interface IViewContainer
{
	Task<bool> Load<T>()
		where T : View;

	Task<bool> Load(object key);

	View GetView<T>()
		where T : View;

	View GetView(object key);

	IViewBindingLifeTime Bind<T>()
		where T : View;

	bool Unbind<T>()
		where T : View;

	void Unbind(LifeTime lifeTime);

	void UnbindAll();

	IViewContainerBatch InParallel();

	IViewContainerSequence InSequence();
}

}
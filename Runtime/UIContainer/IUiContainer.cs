namespace EM.UI
{

using System.Threading.Tasks;
using Foundation;
using IoC;

public interface IUiContainer
{
	Task<bool> Load<T>()
		where T : Panel;

	Task<bool> Load(object key);

	Panel GetPanel<T>()
		where T : Panel;

	Panel GetPanel(object key);

	IUiBindingLifeTime Bind<T>()
		where T : Panel;

	bool Unbind<T>()
		where T : Panel;

	void Unbind(LifeTime lifeTime);

	void UnbindAll();

	IUiContainerBatch InParallel();

	IUiContainerSequence InSequence();
}

}
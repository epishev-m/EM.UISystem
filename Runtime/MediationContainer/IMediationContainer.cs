namespace EM.UI
{

using IoC;

public interface IMediationContainer
{
	void Trigger(MediationTrigger trigger,
		IView view);

	IMediationBindingLifeTime Bind<T>()
		where T : IView;

	bool Unbind<T>();

	void Unbind(LifeTime lifeTime);

	void UnbindAll();
}

}
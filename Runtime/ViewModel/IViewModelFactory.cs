namespace EM.UI
{

using System;

public interface IViewModelFactory
{
	TViewModel Get<TViewModel>()
		where TViewModel : class;

	IViewModel Get(Type type);
}

}
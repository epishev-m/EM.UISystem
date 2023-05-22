namespace EM.UI
{

public interface IViewModelFactory
{
	TViewModel Get<TViewModel>()
		where TViewModel : class;
}

}
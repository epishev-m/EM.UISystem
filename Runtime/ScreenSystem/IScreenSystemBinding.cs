namespace EM.UI
{

public interface IScreenSystemBinding
{
	IScreenSystemBinding To<TView, TViewModel>()
		where TView : View
		where TViewModel : IViewModel;
}

}
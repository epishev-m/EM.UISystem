namespace EM.UI
{

public interface IScreenSystemBinding
{
	IScreenSystemBinding To<TView, TViewModel>()
		where TView : PanelView
		where TViewModel : IViewModel;

	IScreenSystemBinding To<TView, TViewModel>(bool condition)
		where TView : PanelView
		where TViewModel : IViewModel;
}

}
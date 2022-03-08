namespace EM.UI
{

public interface IViewBindingLifeTime
{
	IViewBinding InGlobal();

	IViewBinding InLocal();
}

}
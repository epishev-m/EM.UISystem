namespace EM.UI
{

public interface IMediator
{
	void Initialize(IView view);

	void Release();

	void Enable();

	void Disable();
}

}
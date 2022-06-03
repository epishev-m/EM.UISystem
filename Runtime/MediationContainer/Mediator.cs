namespace EM.UI
{

using Foundation;

public abstract class Mediator :
	IMediator
{
	private IView View;

	#region IMediator

	public virtual void Initialize(IView view)
	{
		Requires.NotNull(view, nameof(view));
		Requires.ValidOperation(View == null, nameof(view));

		View = view;
	}

	public virtual void Release()
	{
		View = null;
	}

	public abstract void Enable();

	public abstract void Disable();

	#endregion
}

}
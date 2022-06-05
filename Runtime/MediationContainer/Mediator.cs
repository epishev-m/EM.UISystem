namespace EM.UI
{

using Foundation;

public abstract class Mediator<T> :
	IMediator
	where T : class, IView
{
	protected T View;

	#region IMediator

	public virtual void Initialize(IView view)
	{
		Requires.NotNull(view, nameof(view));
		Requires.ValidOperation(View == null, nameof(view));
		Requires.Type<T>(view, nameof(view));

		View = (T) view;
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
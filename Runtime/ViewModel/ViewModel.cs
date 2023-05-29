namespace EM.UI
{

using Foundation;

public abstract class ViewModel<T> : IViewModel
{
	protected T Data;

	#region ViewModel

	public virtual void Initialize()
	{
	}

	public virtual void Release()
	{
	}

	public void SetData(object obj)
	{
		Requires.NotNullParam(obj, nameof(obj));

		if (obj is T data)
		{
			Data = data;
		}
	}

	#endregion
}

}
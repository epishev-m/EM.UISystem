namespace EM.UI
{

public abstract class PayloadViewModel<T>
	where T : class
{
	protected T Data;

	public void SetData(T data)
	{
		Data = data;
	}
}

}
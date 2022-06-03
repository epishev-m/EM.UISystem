namespace EM.UI
{

using System.Collections.Generic;

public interface IView
{
	bool IsEnabled
	{
		get;
	}

	IEnumerable<IView> Children
	{
		get;
	}

	void AddView(IView view);

	void RemoveView(IView view);
}

}
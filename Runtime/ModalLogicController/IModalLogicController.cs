namespace EM.UI
{
using System;

public interface IModalLogicController
{
	bool TryGetViewInfo(IView view,
		out ViewInfo viewInfo);

	void PrepareAdd(Modes mode,
		Action onCompleted);

	void PrepareRemove(Modes mode,
		Action onCompleted);

	void Add(IView view,
		Modes mode);

	void Remove(IView view,
		Modes mode);
}

}

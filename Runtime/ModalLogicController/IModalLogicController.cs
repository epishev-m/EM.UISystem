namespace EM.UI
{

using System;

public interface IModalLogicController
{
	bool TryGetViewInfo(IPanel panel,
		out ViewInfo viewInfo);

	void PrepareAdd(Modes mode,
		Action onCompleted);

	void PrepareRemove(Modes mode,
		Action onCompleted);

	void Add(IPanel panel,
		Modes mode);

	void Remove(IPanel panel,
		Modes mode);
}

}

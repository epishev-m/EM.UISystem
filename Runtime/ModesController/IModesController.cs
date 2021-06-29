namespace EM.UI
{
using System;

public interface IModesController
{
	bool TryGetPanelInfo(
		IPanel panel,
		out PanelInfo panelInfo);

	void PrepareAdd(
		Modes mode,
		Action onCompleted);

	void PrepareRemove(
		Modes mode,
		Action onCompleted);

	void Add(
		IPanel panel,
		Modes mode);

	void Remove(
		IPanel panel,
		Modes mode);
}

}

namespace EM.UI
{
using System;
using UnityEngine;

public interface IPanelAnimation
{
	void Show(
		GameObject panel,
		Action onPanelShowed);

	void Hide(
		GameObject panel,
		Action onPanelHidden);
}

}

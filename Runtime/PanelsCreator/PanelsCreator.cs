using System.Linq;

namespace EM.UI
{
using System.Collections.Generic;
using Foundation;
using UnityEngine;

public sealed class PanelsCreator :
	IPanelsCreator
{
	private readonly Transform container;

	private readonly IEnumerable<Canvas> prefabsPanels;

	#region IPanelsCreator

	public IEnumerable<IPanel> CreatePanels()
	{
		var panelsPool = new Dictionary<string, IPanel>();
		CreatePanelsFromGameObjects(panelsPool);
		CreatePanelsFromPrefabs(panelsPool);

		return panelsPool.Values;
	}

	#endregion
	#region PanelsCreator

	public PanelsCreator(
		Transform container,
		IEnumerable<Canvas> prefabsPanels)
	{
		Requires.NotNull(container, nameof(container));
		Requires.NotNull(prefabsPanels, nameof(prefabsPanels));

		this.container = container;
		this.prefabsPanels = prefabsPanels;
	}

	private void CreatePanelsFromGameObjects(
		Dictionary<string, IPanel> panelsPool)
	{
		var canvasArray = container.GetComponentsInChildren<Canvas>(true)
			.Where(canvas => canvas.gameObject != container.gameObject)
			.ToArray();

		foreach (var canvas in canvasArray)
		{
			if (ContainsKey(panelsPool, canvas.name))
			{
				continue;
			}

			var panel = new Panel(canvas);
			panelsPool.Add(panel.Name, panel);
		}
	}

	private void CreatePanelsFromPrefabs(
		Dictionary<string, IPanel> panelsPool)
	{
		foreach (var prefab in prefabsPanels)
		{
			if (ContainsKey(panelsPool, prefab.name))
			{
				continue;
			}

			var panel = CreatePanel(prefab);
			panelsPool.Add(panel.Name, panel);
		}
	}

	private static bool ContainsKey(
		IReadOnlyDictionary<string, IPanel> panelsPool,
		string name)
	{
		var isContains = panelsPool.ContainsKey(name);

		Requires.ValidOperation(!isContains, "TODO");

		return isContains;
	}

	private IPanel CreatePanel(
		Canvas prefab)
	{
		var canvas = Object.Instantiate(prefab, container, false);
		canvas.name = prefab.name;
		var panel = new Panel(canvas);

		return panel;
	}

	#endregion
}

}
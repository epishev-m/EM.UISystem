namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using IoC;
using UnityEngine;

public partial class UiContainer :
	Binder,
	IUiContainer
{
	private readonly string rootAsset;

	private readonly IDiContainer container;

	private readonly IAssetsManager assetsManager;

	private readonly IModalLogicController modalLogicController = new ModalLogicControllerOld();

	private readonly Dictionary<object, Panel> panels = new();

	private Transform uiRoot;

	#region IUiContainer

	public Task<bool> Load<T>()
		where T : Panel
	{
		return Load(typeof(T));
	}

	public async Task<bool> Load(object key)
	{
		if (panels.ContainsKey(key))
		{
			return true;
		}

		var binding = GetBinding(key);

		if (binding == null)
		{
			return false;
		}

		var valuesArray = binding.Values.ToArray();

		if (valuesArray.Length <= 0)
		{
			return false;
		}

		var root = await GetUiRoot();
		var assetPath = (string) valuesArray.First();
		var gameObject = await assetsManager.InstantiateAsync(assetPath, root, new CancellationToken());

		if (gameObject == null)
		{
			return false;
		}

		if (!gameObject.TryGetComponent((Type) key, out var panelComponent))
		{
			assetsManager.ReleaseInstance(gameObject);

			return false;
		}

		Requires.ValidOperation(panelComponent is Panel,
			$"The supplied type {panelComponent.GetType()} " +
			$"is not a given type {typeof(Panel)}. Only given type are supported.");

		var panel = (Panel) panelComponent;
		//container.Inject(panel);
		panels.Add(key, panel);

		return true;
	}

	public Panel GetPanel<T>()
		where T : Panel
	{
		return GetPanel(typeof(T));
	}

	public Panel GetPanel(object key)
	{
		panels.TryGetValue(key, out var result);

		return result;
	}

	public IUiBindingLifeTime Bind<T>()
		where T : Panel
	{
		return base.Bind<T>() as IUiBindingLifeTime;
	}

	public bool Unbind<T>()
		where T : Panel
	{
		var binding = GetBinding(typeof(T));
		var result = Unbind(binding);

		return result;
	}

	public void Unbind(LifeTime lifeTime)
	{
		var bindingsList = bindings.Select(keyValuePair => (UiBinding) keyValuePair.Value)
			.Where(viewBinding => viewBinding.LifeTime == lifeTime);

		foreach (var viewBinding in bindingsList)
		{
			Unbind(viewBinding);
		}
	}

	public IUiContainerBatch InParallel()
	{
		var root = new CommandSequence();
		var batch = new Batch(root, modalLogicController, this);

		return batch;
	}

	public IUiContainerSequence InSequence()
	{
		var root = new CommandSequence();
		var sequence = new Sequence(root, modalLogicController, this);

		return sequence;
	}

	#endregion

	#region Binder

	protected override IBinding GetRawBinding(object key,
		object name)
	{
		return new UiBinding(key, name, BindingResolver);
	}

	#endregion

	#region UiContainer

	public UiContainer(string rootAsset,
		IDiContainer container,
		IAssetsManager assetsManager)
	{
		Requires.ValidArgument(!string.IsNullOrWhiteSpace(rootAsset), nameof(rootAsset));
		Requires.NotNull(container, nameof(container));
		Requires.NotNull(assetsManager, nameof(assetsManager));

		this.rootAsset = rootAsset;
		this.container = container;
		this.assetsManager = assetsManager;
	}

	private async Task<Transform> GetUiRoot()
	{
		if (uiRoot != null)
		{
			return uiRoot;
		}

		uiRoot = await assetsManager.InstantiateAsync<Transform>(rootAsset, new CancellationToken());
		UnityEngine.Object.DontDestroyOnLoad(uiRoot);

		return uiRoot;
	}

	private bool Unbind(IBinding binding)
	{
		if (!panels.Remove((Type) binding.Key, out var panel))
		{
			return false;
		}

		//TODO Requires.ValidOperation(panel.IsOpened == false, "");
		if (!panel.IsOpened)
		{
			return false;
		}

		assetsManager.ReleaseInstance(panel.gameObject);

		return base.Unbind(binding.Key);
	}

	#endregion
}

}
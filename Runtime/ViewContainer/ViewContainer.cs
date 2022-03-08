namespace EM.UI
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using IoC;
using UnityEngine;

public partial class ViewContainer :
	Binder,
	IViewContainer
{
	private readonly string rootAsset;

	private readonly IDiContainer container;

	private readonly IAssetsManager assetsManager;

	private readonly IModalLogicController modalLogicController = new ModalLogicController();

	private readonly Dictionary<object, View> views = new();

	private Transform uiRoot;

	#region ViewContainer

	public Task<bool> Load<T>()
		where T : View
	{
		return Load(typeof(T));
	}

	public async Task<bool> Load(object key)
	{
		if (views.ContainsKey(key))
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
		var gameObject = await assetsManager.InstantiateAsync(assetPath, root);

		if (gameObject == null)
		{
			assetsManager.ReleaseInstance(gameObject);
			return false;
		}

		if (!gameObject.TryGetComponent((Type) key, out var viewComponent))
		{
			assetsManager.ReleaseInstance(gameObject);
			return false;
		}

		Requires.ValidOperation(viewComponent is View,
			$"The supplied type {viewComponent.GetType()} " +
			$"is not a given type {typeof(View)}. Only given type are supported.");

		var view = (View) viewComponent;
		container.Inject(view);
		views.Add(key, view);

		return true;
	}

	public View GetView<T>()
		where T : View
	{
		return  GetView(typeof(T));
	}

	public View GetView(object key)
	{
		views.TryGetValue(key, out var result);

		return result;
	}

	public IViewBindingLifeTime Bind<T>()
		where T : View
	{
		return base.Bind<T>() as IViewBindingLifeTime;
	}

	public bool Unbind<T>()
		where T : View
	{
		var binding = GetBinding(typeof(T));
		var result = Unbind(binding);

		return result;
	}

	public void Unbind(LifeTime lifeTime)
	{
		var bindingsList = bindings.Select(keyValuePair => (ViewBinding) keyValuePair.Value)
			.Where(viewBinding => viewBinding.LifeTime == lifeTime);

		foreach (var viewBinding in bindingsList)
		{
			Unbind(viewBinding);
		}
	}

	public IViewContainerBatch InParallel()
	{
		var root = new CommandSequence();
		var batch = new Batch(root, modalLogicController, this);

		return batch;
	}

	public IViewContainerSequence InSequence()
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
		return new ViewBinding(key, name, BindingResolver);
	}

	#endregion

	#region UiContainer

	public ViewContainer(string rootAsset,
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

		uiRoot = await assetsManager.InstantiateAsync<Transform>(rootAsset);
		UnityEngine.Object.DontDestroyOnLoad(uiRoot);

		return uiRoot;
	}

	private bool Unbind(IBinding binding)
	{
		if (!views.TryGetValue((Type) binding.Key, out var view))
		{
			return false;
		}

		//TODO Requires.ValidOperation(view.IsOpened == false, "");
		if (!view.IsOpened)
		{
			return false;
		}

		assetsManager.ReleaseInstance(view.gameObject);

		return base.Unbind(binding.Key);
	}

	#endregion
}

}
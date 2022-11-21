namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using IoC;

public sealed class MediationContainer : Binder,
	IMediationContainer
{
	private readonly IDiContainer _container;

	private readonly Dictionary<IView, IMediator> _mediators = new();

	#region IMediationContainer

	public void Trigger(MediationTrigger trigger,
		IView view)
	{
		Requires.NotNull(view, nameof(view));

		TriggerInternal(trigger, view);

		foreach (var child in view.Children)
		{
			TriggerInternal(trigger, child);
		}
	}

	public IMediationBindingLifeTime Bind<T>()
		where T : IView
	{
		return base.Bind<T>() as IMediationBindingLifeTime;
	}

	public bool Unbind<T>()
	{
		return base.Unbind<T>();
	}

	public void Unbind(LifeTime lifeTime)
	{
		Unbind(binding =>
		{
			var result = binding is MediationBinding diBinding && diBinding.LifeTime == lifeTime;

			return result;
		});
	}

	#endregion

	#region Binder

	protected override IBinding GetRawBinding(object key,
		object name)
	{
		return new MediationBinding(key, name, BindingResolver);
	}

	#endregion

	#region MediationContainer

	public MediationContainer(IDiContainer container)
	{
		Requires.NotNull(container, nameof(container));

		_container = container;
	}

	private void TriggerInternal(MediationTrigger trigger,
		IView view)
	{
		switch (trigger)
		{
			case MediationTrigger.Initialise:
				Initialise(view);
				break;
			case MediationTrigger.Release:
				Release(view);
				break;
			case MediationTrigger.Enabled:
				Enable(view);
				break;
			case MediationTrigger.Disabled:
				Disable(view);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null);
		}
	}

	private void Initialise(IView view)
	{
		if (_mediators.TryGetValue(view, out var mediator))
		{
			return;
		}

		var key = view.GetType();

		if (GetBinding(key) is not MediationBinding binding)
		{
			return;
		}

		var values = binding.Values.ToArray();

		if (!values.Any())
		{
			return;
		}

		var type = (Type) values.First();
		mediator = (IMediator) _container.Resolve(type);

		if (mediator == null)
		{
			return;
		}

		mediator.Initialize(view);

		if (view.IsEnabled)
		{
			mediator.Enable();
		}

		_mediators.Add(view, mediator);
	}

	private void Release(IView view)
	{
		if (_mediators.Remove(view, out var mediator))
		{
			mediator.Release();
		}
	}

	private void Enable(IView view)
	{
		if (_mediators.TryGetValue(view, out var mediator))
		{
			mediator.Enable();
		}
	}

	private void Disable(IView view)
	{
		if (_mediators.TryGetValue(view, out var mediator))
		{
			mediator.Disable();
		}
	}

	#endregion
}

}
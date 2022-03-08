using EM.Foundation;
using EM.IoC;

namespace EM.UI
{

public sealed class ViewBinding :
	Binding,
	IViewBinding,
	IViewBindingLifeTime
{
	#region IViewBindingLifeTime

	public IViewBinding InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InGlobal));

		LifeTime = LifeTime.Global;

		return this;
	}

	public IViewBinding InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InLocal));

		LifeTime = LifeTime.Local;

		return this;
	}

	#endregion

	#region IViewBinding

	public IViewBinding To(string asset)
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(To));

		return base.To(asset) as IViewBinding;
	}

	#endregion

	#region ViewBinding

	public ViewBinding(object key,
		object name,
		Resolver resolver)
		: base(key, name, resolver)
	{
	}

	public LifeTime LifeTime
	{
		get;
		set;
	} = LifeTime.External;

	#endregion
}

}
namespace EM.UI
{

using Foundation;
using IoC;

public sealed class UiBinding :
	Binding,
	IUiBinding,
	IUiBindingLifeTime
{
	#region IUiBindingLifeTime

	public IUiBinding InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InGlobal));

		LifeTime = LifeTime.Global;

		return this;
	}

	public IUiBinding InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InLocal));

		LifeTime = LifeTime.Local;

		return this;
	}

	#endregion

	#region IUiBinding

	public IUiBinding To(string asset)
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(To));

		return base.To(asset) as IUiBinding;
	}

	#endregion

	#region UiBinding

	public UiBinding(object key,
		object name,
		Resolver resolver)
		: base(key, name, resolver)
	{
	}

	public LifeTime LifeTime
	{
		get;
		private set;
	} = LifeTime.External;

	#endregion
}

}
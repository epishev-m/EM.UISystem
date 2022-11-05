namespace EM.UI
{

using Foundation;

public sealed class MediationBinding : Binding,
	IMediationBinding,
	IMediationBindingLifeTime
{
	#region IMediationBindingLifeTime

	public IMediationBinding InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this);

		LifeTime = LifeTime.Global;

		return this;
	}

	public IMediationBinding InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this);

		LifeTime = LifeTime.Local;

		return this;
	}

	#endregion

	#region IMediationBinding

	public new void To<T>()
		where T : IMediator
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this);

		base.To<T>();
	}

	#endregion

	#region MediationBinding

	public MediationBinding(object key,
		object name,
		Resolver resolver)
		: base(key,
			name,
			resolver)
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
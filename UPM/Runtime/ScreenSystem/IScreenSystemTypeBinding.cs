namespace EM.UI
{

public interface IScreenSystemTypeBinding
{
	UiTypes Type { get; }

	IScreenSystemBinding AsScreen();

	IScreenSystemBinding AsPopup();

	IScreenSystemBinding AsTooltip();

	IScreenSystemBinding SetType(UiTypes type);
}

}
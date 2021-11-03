namespace EM.UI
{
using System.Collections.Generic;

public interface IPanelsCreator
{
	IEnumerable<IPanel> CreatePanels();
}

}
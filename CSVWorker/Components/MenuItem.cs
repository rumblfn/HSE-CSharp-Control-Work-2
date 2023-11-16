using Lib;

namespace CSVWorker.Components;

/// <summary>
/// Menu item to select.
/// </summary>
public class MenuItem
{
    private string Name { get; }
    public Action SelectAction { get; }
    public bool Selected = false;

    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="name">Element name.</param>
    /// <param name="selectAction">The method being called.</param>
    public MenuItem(string name, Action selectAction)
    {
        Name = name;
        SelectAction = selectAction;
    }

    /// <summary>
    /// Responsible for the output of the menu item.
    /// </summary>
    public void Write()
    {
        ConsoleMethod.NicePrint(" " + Name + ";", 
            Selected ? CustomColor.Primary : CustomColor.Secondary, "");
    }
}
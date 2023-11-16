using Lib;

namespace CSVWorker.Components;

/// <summary>
/// Menu item to select.
/// </summary>
public class MenuGroup
{
    private string Name { get; }
    public MenuItem[] Items { get; }
    public bool Selected = false;

    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="name">Group name.</param>
    /// <param name="items">Items in Group.</param>
    public MenuGroup(string name, MenuItem[] items)
    {
        Name = name;
        Items = items;
    }
    
    /// <summary>
    /// Responsible for the output of the menu group.
    /// </summary>
    public void Write()
    {
        if (Selected)
        {
            ConsoleMethod.NicePrint("?", CustomColor.Primary, " ");
        }

        Console.Write(Name + ":");
        foreach (MenuItem item in Items)
        {
            item.Write();
        }
        Console.WriteLine("  ");
    }
}
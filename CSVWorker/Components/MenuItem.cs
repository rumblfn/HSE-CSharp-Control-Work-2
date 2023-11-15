using Lib;

namespace CSVWorker.Components;

public class MenuItem
{
    private string Name { get; }
    public Action SelectAction { get; }
    public bool Selected = false;

    public MenuItem(string name, Action selectAction)
    {
        Name = name;
        SelectAction = selectAction;
    }

    public void Write()
    {
        ConsoleMethod.NicePrint(" " + Name + ";", 
            Selected ? CustomColor.Primary : CustomColor.Secondary, "");
    }
}
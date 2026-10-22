using Godot;
using Godot.Collections;
using LanguageExt;
using static LanguageExt.Prelude;

[GlobalClass]
public partial class RadialMenu : Control
{
    Dictionary<Button, int> _buttonColorIndexDict = [];

    [Signal]
    public delegate void SelectedColorIndexEventHandler(int colorIndex);

    Option<Button> _selectedButton = new();

    public override void _Ready()
    {
        Array<ColorAndShapeResource> colorAndShapeResources = [];
        GameManagerSingleton
            .GetGameManagerSingleton()
            .IfSome(
                (gameManager) =>
                {
                    colorAndShapeResources = gameManager.GetColorAndShapeResources();
                }
            );

        Array<Node> children = GetChildren();

        for (int i = 0; i < children.Length(); i++)
        {
            Node childNode = children[i];
            int colorAndShapeResourcesIndex = i % colorAndShapeResources.Length();

            if (childNode is Button buttonNode)
            {
                Color color = colorAndShapeResources[colorAndShapeResourcesIndex].Color;

                buttonNode.Modulate = color;

                _buttonColorIndexDict.Add(
                    buttonNode,
                    colorAndShapeResourcesIndex
                );

                buttonNode.MouseEntered += () => SetSelectedButton(buttonNode);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            Visible = true;
        }

        if (@event.IsActionReleased("ui_accept"))
        {
            _selectedButton.IfSome(
                (button) =>
                {
                    EmitSignalSelectedColorIndex(_buttonColorIndexDict[button]);
                    _selectedButton = new();
                }
            );
            Visible = false;
        }
    }

    private void SetSelectedButton(Button selectedButton) => _selectedButton = Optional(selectedButton);
}

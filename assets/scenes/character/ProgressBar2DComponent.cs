using System;
using Godot;
using static LanguageExt.Prelude;

[GlobalClass]
public partial class ProgressBar2DComponent : Sprite2D
{
    [Export]
    ProgressBar _progressBar;

    public void Update(float setToValue, float remapStart = 0, float remapStop = 100)
    {
        float remapMin = Math.Min(remapStart, remapStop);
        float remapMax = Math.Max(remapStart, remapStop);

        setToValue = Math.Clamp(setToValue, remapMin, remapMax);

        var remap = Mathf.Remap(setToValue, remapStart, remapStop, 0, 100);

        Optional(_progressBar).IfSome((progressBar) => progressBar.Value = remap);
    }
}

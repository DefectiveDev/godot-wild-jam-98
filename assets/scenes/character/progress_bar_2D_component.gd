class_name ProgressBar2DComponent extends Sprite2D

@export
var _progress_bar: ProgressBar

## Update progress bar. Value will be clamped to remap.
func update_progress_bar(set_to_value: float, remap_start: float = 0, remap_stop: float = 100):
	var remap_min = minf(remap_start,remap_stop)
	var remap_max = maxf(remap_start,remap_stop)
	set_to_value = clampf(set_to_value, remap_min, remap_max)
	_progress_bar.value = remap(set_to_value, remap_start, remap_stop, 0, 100)

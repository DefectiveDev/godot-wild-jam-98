extends CanvasLayer

func _ready() -> void:
	_toggle_pause()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		_toggle_pause()

func _toggle_pause():
	get_tree().paused = not get_tree().paused
	visible = get_tree().paused

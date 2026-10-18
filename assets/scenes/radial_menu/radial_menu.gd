class_name RadialMenu extends Control

var _button_color_index_dict: Dictionary[Button, int]

signal selected_color_index(color_index: int)

var current_button: Button

# Called when the node enters the scene tree for the first time.
func _ready() -> void:

	var colors_and_shape := GameManagerSingleton.GAME_MANAGER.colors_and_shapes
	var children := get_children()
	for index in len(children):
		var node := children[index]
		var colors_and_shapes_index = index % len(colors_and_shape)
		if node is Button:
			var color = colors_and_shape[colors_and_shapes_index].color

			node.self_modulate = color

			_button_color_index_dict.get_or_add(node, colors_and_shapes_index)

			node.connect("mouse_entered", _set_selected.bind(node))


func _input(event):
	if event.is_action_pressed("ui_accept"):
		visible = true

	if event.is_action_released("ui_accept"):
		if current_button:
			selected_color_index.emit(_button_color_index_dict.get(current_button))
			current_button = null
		visible = false

func _set_selected(button: Button):
	current_button = button

class_name SpawnComponent extends Node

@export
var spawn_timer: Timer

@export
var scene_to_spawn: PackedScene

@export
var _off_screen_padding_px: int = 64

@onready var tree := Engine.get_main_loop() as SceneTree

enum _Side {
	TOP,
	BOTTOM,
	LEFT,
	RIGHT
}

func _ready() -> void:
	spawn_timer.timeout.connect(on_spawn_timer_timeout)

func on_spawn_timer_timeout():
	var node = scene_to_spawn.instantiate()

	if node is Node2D:
		var side := (randi() % 4) as _Side

		var camera_center_screen_pos = get_viewport().get_camera_2d().get_screen_center_position()
		var viewport_center_pos = get_viewport().get_visible_rect().get_center()

		var left_side_screen_px = camera_center_screen_pos.x - viewport_center_pos.x
		var right_side_screen_px = camera_center_screen_pos.x + viewport_center_pos.x
		var top_of_screen_px = camera_center_screen_pos.y - viewport_center_pos.y
		var bottom_of_screen_px = camera_center_screen_pos.y + viewport_center_pos.y

		match side:
			_Side.TOP, _Side.BOTTOM:
				var random_x = randi_range(right_side_screen_px, left_side_screen_px)
				if side == _Side.TOP:
					node.position = Vector2i(random_x, top_of_screen_px - _off_screen_padding_px)
				else:
					node.position = Vector2i(random_x, bottom_of_screen_px + _off_screen_padding_px)

			_Side.RIGHT, _Side.LEFT:
				var random_y = randi_range(top_of_screen_px, bottom_of_screen_px)
				if side == _Side.LEFT:
					node.position = Vector2i(left_side_screen_px - _off_screen_padding_px, random_y)
				else:
					node.position = Vector2i(right_side_screen_px + _off_screen_padding_px, random_y)

	tree.current_scene.add_child.call_deferred(node)

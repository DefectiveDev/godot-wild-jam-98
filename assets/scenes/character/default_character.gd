@abstract class_name DefaultCharacter2D extends CharacterBody2D

@export
var health_component: HealthComponent

@export
var attack_component: AttackComponent

@export
var movement_component_2d: MovementComponent2D

@export
var _progress_bar: ProgressBar2DComponent

@warning_ignore("unused_private_class_variable")
var _current_color_index: int = 0

@abstract func _set_color_index(color_index: int)

@abstract func get_color_index() -> int

func _ready():
	health_component.health_changed.connect(_progress_health_bar_update)

func _progress_health_bar_update(arg_health_component: HealthComponent):
	if _progress_bar:
		_progress_bar.update_progress_bar(arg_health_component.get_health(), 0, arg_health_component.max_health)

@export
var on_death_queue_free: Array[Node]

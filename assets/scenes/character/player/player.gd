class_name Player extends DefaultCharacter2D

@export
var _audio_streamer: AudioStreamPlayer2D

@export
var on_death_scene: PackedScene

@export
var input_component: InputComponent

@export
var _sprite_to_color: Sprite2D

@export
var _radial_menu: RadialMenu

@export
var _animated_sprite: AnimatedSprite2D

@onready var colors_and_shapes := GameManagerSingleton.GAME_MANAGER.colors_and_shapes

func _init() -> void:
	add_to_group("player")

func _ready() -> void:
	super._ready()

	health_component.died.connect(_on_death)

	health_component.health_changed.connect(_on_hurt)

	_radial_menu.selected_color_index.connect(_set_color_index)

	_sprite_to_color.self_modulate = colors_and_shapes[_current_color_index].color

func _physics_process(delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/2
	# Add player movement
	#
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.

	if health_component.get_is_dead():
		return
	
	input_component.update()
	var move_direction = input_component.get_move_direction()
	movement_component_2d.update(delta, self, move_direction)

func _on_hurt(_arg_health_component: HealthComponent):
	if health_component.get_is_dead():
		return

	_audio_streamer.play()

func _set_color_index (color_index: int):
	_current_color_index = color_index
	_sprite_to_color.self_modulate = colors_and_shapes[_current_color_index].color

func  get_color_index() -> int:
	return _current_color_index

func _attack(overlapping_bodies: Array[Node2D], attack_component_self: AttackComponent, self_color_index: int):
	for body in overlapping_bodies:
		var enemy_node: Enemy = null

		if body is Enemy:
			enemy_node = body
		elif body.owner is Enemy:
			enemy_node = body.owner

		if enemy_node is Enemy:
			if self_color_index == enemy_node.get_color_index():
				enemy_node.health_component.set_health(
					enemy_node.health_component.get_health() - ceili(attack_component_self.damage_per_second * attack_component_self.attack_cooldown_timer.wait_time)
					)

func _on_flash_beam_area_2d_has_overlapping_bodies_process(overlapping_bodies: Array[Node2D]) -> void:
	var attack = _attack.bind(overlapping_bodies, attack_component, get_color_index())
	attack_component.attack_on_cooldown(attack)

func  _on_death():
	for node in on_death_queue_free:
		if node:
			if not node.is_queued_for_deletion():
				node.queue_free()
	
	_animated_sprite.animation_finished.connect(
		func():
			get_tree().current_scene.add_child.call_deferred(on_death_scene.instantiate())
			_animated_sprite.queue_free()
			)
	_animated_sprite.play("death")
	

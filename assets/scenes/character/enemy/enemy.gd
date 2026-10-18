class_name Enemy extends DefaultCharacter2D

@export
var _audio_streamer: AudioStreamPlayer2D

@export
var _color_changing_sprite: AnimatedSprite2D = null

@onready var colors_and_shapes := GameManagerSingleton.GAME_MANAGER.colors_and_shapes

@export
var _visible_notify: VisibleOnScreenNotifier2D

@export
var _animation_player: AnimationPlayer

func _init():
	max_slides = 1

func _ready():
	super._ready()

	health_component.died.connect(_on_death)

	health_component.health_changed.connect(_on_hurt)

	_visible_notify.screen_entered.connect(
		func():
			if movement_component_2d:
				movement_component_2d.current_speed = movement_component_2d.max_speed
	)

	_visible_notify.screen_exited.connect(
		func():
			if movement_component_2d:
				movement_component_2d.current_speed *= 3
	)

	_current_color_index = randi_range(0, colors_and_shapes.size() - 1)

	_color_changing_sprite.self_modulate = colors_and_shapes[_current_color_index].color


func _on_hurt(_arg_health_component: HealthComponent):
	if health_component.get_is_dead():
		return

	_audio_streamer.play()



func _on_death():
	var score_component = GameManagerSingleton.GAME_MANAGER.get_score_component()
	score_component.set_score(
		score_component.get_score() + 1.0
		)

	for node in on_death_queue_free:
		if node:
			if not node.is_queued_for_deletion():
				node.queue_free()
	
	_animation_player.animation_finished.connect(
		func(_anim_name: String):
			if _anim_name == "on_death":
				queue_free()
			)

	_animation_player.play("on_death")

func  _set_color_index(color_index: int):
	_current_color_index = color_index
	_color_changing_sprite.self_modulate = colors_and_shapes[_current_color_index].color

func  get_color_index() -> int:
	return _current_color_index

func _physics_process(delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/4
	# Have the enemy follow the player

	if health_component.get_is_dead():
		return

	var move_direction := position.direction_to(GameManagerSingleton.GAME_MANAGER.player.position)

	movement_component_2d.update(delta, self, move_direction)

	var attack = _attack.bind(self, attack_component)

	attack_component.attack_on_cooldown(attack)




func _attack(character_body_2d: CharacterBody2D, attack_component_self: AttackComponent):
	var collision = character_body_2d.get_last_slide_collision()
	
	if collision:
		var node = (collision.get_collider() as Node)
		var owner_node = node.owner
		if owner_node is Player:
			# print("enemy attacking player, Player health: " + str(owner_node.health_component.get_health()))
			owner_node.health_component.set_health(
				owner_node.health_component.get_health() - ceili(attack_component_self.damage_per_second * attack_component_self.attack_cooldown_timer.wait_time)
				)

class_name Player extends DefaultCharacter2D


@onready var rotation_child_node = $Rotation

@export
var input_component: InputComponent


func _init() -> void:
	add_to_group("player")

func _physics_process(delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/2
	# Add player movement
	#
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	
	input_component.update()
	var move_direction = input_component.get_move_direction()
	movement_component_2d.update(delta, self, move_direction)

func _attack(overlapping_bodies: Array[Node2D], attack_component_self: AttackComponent):
	for body in overlapping_bodies:
		if body is Enemy:
			body.health_component.set_health(
				body.health_component.get_health() - ceili(attack_component_self.damage_per_second * attack_component_self.attack_cooldown_timer.wait_time)
				)

func _on_flash_beam_area_2d_has_overlapping_bodies_physics(overlapping_bodies: Array[Node2D]) -> void:
	var attack = _attack.bind(overlapping_bodies,attack_component)
	attack_component.attack_on_time(attack)

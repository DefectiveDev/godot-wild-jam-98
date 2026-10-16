class_name Enemy extends DefaultCharacter2D


@onready var player: Player = GameManagerSingleton.GAME_MANAGER.player


func _attack(character_body_2d: CharacterBody2D, attack_component_self: AttackComponent):
	var collision = character_body_2d.get_last_slide_collision()
	
	if collision:
		var node = (collision.get_collider() as Node)
		var parent_node = node.get_parent()
		if parent_node is Player:
			print("enemy attacking player, Player health: " + str(parent_node.health_component.get_health()))
			parent_node.health_component.set_health(
				parent_node.health_component.get_health() - ceili(attack_component_self.damage_per_second * attack_component_self.attack_cooldown_timer.wait_time)
				)

func _init():
	max_slides = 1

func _physics_process(delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/4
	# Have the enemy follow the player

	var move_direction := position.direction_to(player.position)

	movement_component_2d.update(delta, self, move_direction)

	attack_component.attack(_attack, [self, attack_component])

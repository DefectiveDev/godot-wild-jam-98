extends DefaultCharacter2D


var player: Player = null

var _hurt_cooldown_reached = true

@onready var _hurt_cooldown_timer: Timer = $HurtCooldownTimer

var _attack_cooldown_reached = true

@onready var _attack_cooldown_timer: Timer = $AttackCooldownTimer


func attack(target: DefaultCharacter2D):
	_attack_cooldown_reached = false
	_attack_cooldown_timer.start()
	target.current_health -= ceil(damage_per_second * _attack_cooldown_timer.wait_time)

func _init():
	max_slides = 1

func _physics_process(_delta: float) -> void:
	# https://github.com/DefectiveDev/godot-wild-jam-98/issues/4
	# Have the enemy follow the player

	if not player:
		player = GameManagerSingleton.GAME_MANAGER.player
	var direction := position.direction_to(player.position)
	velocity = direction * current_speed
	
	var collision = move_and_collide(velocity, false)

	if _attack_cooldown_reached and collision:
		var node = (collision.get_collider() as Node)
		var parent_node = node.get_parent()
		if parent_node as Player:
			parent_node.hit.emit(attack)
			attack(parent_node)

	# move_and_slide()


func _on_hurt_cooldown_timer_timeout() -> void:
	_hurt_cooldown_reached = true


func _on_attack_cooldown_timer_timeout() -> void:
	_attack_cooldown_reached = true

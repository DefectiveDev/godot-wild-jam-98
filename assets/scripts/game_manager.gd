extends Node

# TODO: https://github.com/DefectiveDev/godot-wild-jam-98/issues/5
# Create game manager to adjust settings and access player globally
class_name GameManagerSingleton

static var GAME_MANAGER: GameManagerSingleton:
	get:
		if not _GAME_MANAGER:
			var tree = Engine.get_main_loop() as SceneTree
			if tree:
				_GAME_MANAGER = tree.current_scene.add_child(
					load("res://assets/scenes/game_manager.tscn").instantiate()
					)
		return _GAME_MANAGER

static  var _GAME_MANAGER: GameManagerSingleton = null:
	set(new_game_manager):
		if not _GAME_MANAGER:
			GAME_MANAGER = new_game_manager
		else:
			new_game_manager.queue_free()
	get:
		return _GAME_MANAGER


var player: Player:
	get:
		return _player

var _player: Player = null:
	get:
		if not _player:
			_player = get_tree().get_first_node_in_group("player")
		assert(_player, "Player script is being accessed but there is no player node in group \"player\". Check if you have the Player in your current scene.")
		return _player

var _firstProcess = true

func _init():
	if not _GAME_MANAGER and _GAME_MANAGER != self:
		_GAME_MANAGER = self

func _process(_delta):
	if _firstProcess:
		_player = get_tree().get_first_node_in_group("player")
		_firstProcess = false

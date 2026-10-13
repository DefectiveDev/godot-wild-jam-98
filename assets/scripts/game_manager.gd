extends Node

# https://github.com/DefectiveDev/godot-wild-jam-98/issues/5
# Create game manager to adjust settings and access player globally
class_name GameManagerSingleton

static var GAME_MANAGER: GameManagerSingleton:
	get:
		if not _GAME_MANAGER:
			var tree = Engine.get_main_loop() as SceneTree
			if tree:
				var node = load("res://assets/scenes/game_manager.tscn").instantiate()
				tree.current_scene.add_child(node)
				_GAME_MANAGER = node
		return _GAME_MANAGER

static  var _GAME_MANAGER: GameManagerSingleton = null:
	set(new_game_manager):
		if not _GAME_MANAGER:
			_GAME_MANAGER = new_game_manager
		else:
			new_game_manager.queue_free()
	get:
		return _GAME_MANAGER


var player: Player:
	get:
		return _player

var _player: Player = null:
	set(v):
		_player = v
	get:
		if not _player:
			_player = get_tree().get_first_node_in_group("player")
		assert(_player, "Player script is being accessed but there is no player node in group \"player\". Check if you have the Player in your current scene.")
		return _player

func _init():
	if not _GAME_MANAGER and _GAME_MANAGER != self:
		_GAME_MANAGER = self

func _ready():
	_player = get_tree().get_first_node_in_group("player")

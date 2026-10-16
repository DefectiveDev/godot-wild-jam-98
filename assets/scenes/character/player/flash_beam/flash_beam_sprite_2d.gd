extends Sprite2D
@onready var color = GameManagerSingleton.GAME_MANAGER.colors_and_shapes[0].color

# Called when the node is created
func _ready() -> void:
	self_modulate = color


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass

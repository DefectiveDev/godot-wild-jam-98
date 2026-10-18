extends Control

@onready var _score_component: ScoreComponent = GameManagerSingleton.GAME_MANAGER._score_component

@export
var _score_label: Label

func _ready():
	_score_component.score_changed.connect(_on_score_change)
	_on_score_change(_score_component, 0)

func _on_score_change(arg_score_component: ScoreComponent, _previous_score: float):
	_score_label.text = "Score: " + str(
		roundi(arg_score_component.get_score())
		)

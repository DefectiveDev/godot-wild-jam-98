class_name ScoreComponent extends Node

@export
var _default_score := 0.0

@onready var _current_score := _default_score

signal score_changed(score_component: ScoreComponent, previous_score: float)

func get_score() -> float:
	return _current_score

func set_score(set_score_to: float) -> float:
	var previous_score := get_score()
	_current_score = set_score_to

	score_changed.emit(self, previous_score)

	return _current_score

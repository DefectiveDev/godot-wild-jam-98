extends Control


# Called when the node enters the scene tree for the first time.
func _resume():
	get_tree().paused = false
	
	
func _pause():
	get_tree().paused = true
	
func _testesc():
	if Input.is_action_just_pressed("esc") and get_tree().paused == false:
		_pause()
	elif Input.is_action_just_pressed("esc") and get_tree().paused == true:
		_resume()
		
		
	


func _on_play_pressed() -> void:
	pass # Replace with function body.
	_resume()
	


func _on_exit_pressed() -> void:
	pass # Replace with function body.
	get_tree().quit()
	
	
	

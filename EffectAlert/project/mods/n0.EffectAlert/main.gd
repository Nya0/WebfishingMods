extends Node

const MOD = "n0.EffectAlert"
const DEBUG = false
class_name EffectAlert

func printd(message, step):
	if not DEBUG: return

	print("[%s](%s) %s" % [MOD, step, message])

var _audio_cache = {}

func get_audio(path: String) -> AudioStream:
	if path in _audio_cache:
		return _audio_cache[path]
	
	var audio = load(path)
	if audio:
		_audio_cache[path] = audio
	return audio

func _play_sound(path: String, volume: float = 0) -> void:
	var sound_player : AudioStreamPlayer = AudioStreamPlayer.new()
	var stream : AudioStreamMP3 = get_audio(path)
	stream.loop = false
	
	sound_player.stream = stream
	sound_player.volume_db = volume
	add_child(sound_player)
	
	printd("Playing sound: %s" % path, "sound")
	sound_player.play()
	
	# clean up
	sound_player.connect("finished", sound_player, "queue_free")

func clear_cache() -> void:
	_audio_cache.clear()

func _ready() -> void:
		printd(str(get_path()), "ready")

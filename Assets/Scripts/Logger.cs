using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class Logger : MonoBehaviour {
	private static string fileName;
	private Settings settings;

	void Start(){
		settings = GameObject.FindObjectOfType<Settings> ();
	}

	public void SetFileName(string _fileName){
		fileName = _fileName;
	}

	public static string GenerateTimestamp(){
		return DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
	}

	public static void LogKeypress(string _type){
		WriteLineToLog("$"+_type);
	}

	public static void LogExpandCollapse(string _type){
		WriteLineToLog("-"+_type);
	}

	public static void LogInteraction(string _interactionType, int _numTargets){
		WriteLineToLog("#"+_interactionType, "", _numTargets);
	}

	public static void LogMenuInteraction(string _optionName){
		WriteLineToLog("@Menu", _optionName);
	}

	public static void LogSelection(string _type, string _target){
		WriteLineToLog("&"+_type, _target);
	}

	public static void LogError(){
		WriteLineToLog ("!Error");
	}

	public static void LogNewParticipant(){
		WriteLineToLog("Session Started", fileName);
	}

	public static void LogTaskStart(float startTime, string referenceImage ){
		WriteLineToLog("Task Started",referenceImage, -1, startTime);
	}

	public static void LogTaskFinish(float timeToComplete, string referenceImage){
		WriteLineToLog ("Task Finished", referenceImage, -1, timeToComplete);
	}

	public static void LogSessionComplete(){
		WriteLineToLog ("Session Finished");
	}

	public static void WriteLineToLog(string _action, string _targetType = "", int _numTargets = -1, float _time = 0){
		if (fileName == default(string)) {
			Debug.LogError ("No filename provided");
			return;
		}

		string stringToLog = _action+"\t"+_targetType+"\t"+_numTargets+"\t"+Settings.selectionMode+"\t"+GenerateTimestamp()+"\t"+_time;

		using (FileStream aFile = new FileStream(fileName, FileMode.Append, FileAccess.Write))
		using (StreamWriter sw = new StreamWriter(aFile)) {
			Debug.Log (stringToLog);
			sw.WriteLine (stringToLog);
		}
	}
}

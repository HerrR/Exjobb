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
		// $Trigger Up/Trigger Down/Trigger Hold/... 	Timestamp
		string[] msg = {
			"$" + _type + "\t" + GenerateTimestamp ()
		};
		WriteToLog (msg);
	}

	public static void LogExpandCollapse(string _type){
		// -Expand/Collapse	Timestamp
		string[] msg = {
			"-" + _type + "\t" + GenerateTimestamp ()
		};
		WriteToLog (msg);
	}

	public static void LogInteraction(string _interactionType, int _numTargets){
		// #MoveZ/MoveXY	1/2/3/4/5/6/7	Pointer/Direct	TimeStamp
		string[] msg = {
			"#"+_interactionType+"\t"+_numTargets+"\t"+Settings.selectionMode+"\t"+GenerateTimestamp()
		};

		WriteToLog (msg);
	}

	public static void LogMenuInteraction(string _optionName){
		// @Expand/Collapse/Pointer/Direct	Timestamp
		string[] msg = {
			"@Menu "+_optionName+"\t"+GenerateTimestamp()
		};
		WriteToLog (msg);
	}

	public static void LogSelection(string _type, string _target){
		// &Additive/Single		LayerImage/Frame/No Target		Pointer/Direct		Timestamp
		string[] msg = {
			"&"+_type+"\t"+_target+"\t"+Settings.selectionMode+"\t"+GenerateTimestamp()
		};
		WriteToLog (msg);
	}

	public static void LogError(){
		string[] msg = {
			"!Error\t"+Settings.selectionMode+"\t"+GenerateTimestamp()	
		};

		WriteToLog (msg);
	}

	public static void LogNewParticipant(){
		string[] msg = {
			"------------------------------------------------------",
			"New Participant",
			"Session started\t"+GenerateTimestamp(),
			"Prototype Version\t2",
			"Filename\t"+fileName,
			"------------------------------------------------------"
		};
		WriteToLog(msg);
	}

	public static void LogTaskStart(float startTime, string referenceImage ){
		// Task Started 		Reference Image 		Timestamp
		string[] msg = {
			"Task Started\t"+referenceImage+"\t"+ startTime
		};

		WriteToLog (msg);
	}

	public static void LogTaskFinish(float timeToComplete, string referenceImage){
		string[] msg = {
			"Task Finished\t"+referenceImage+"\t"+ GenerateTimestamp() + "\t" + timeToComplete
		};

		WriteToLog (msg);
	}

	public static void LogSessionComplete(){
		string[] msg = {
			"------------------------------------------------------",
			"Session Finished\t"+GenerateTimestamp(),
			"------------------------------------------------------"
		};

		WriteToLog (msg);
	}

	public static void WriteToLog(string[] _inputLines){
		if (fileName == default(string)) {
			Debug.LogError ("No filename provided");
			return;
		}
			
		using (FileStream aFile = new FileStream(fileName, FileMode.Append, FileAccess.Write))
		using (StreamWriter sw = new StreamWriter(aFile)) {
			foreach (string _line in _inputLines) {
				Debug.Log (_line);
				sw.WriteLine (_line);
			}
		}
	}
}

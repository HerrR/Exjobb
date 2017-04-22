using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class Logger : MonoBehaviour {
	private static string fileName;

	public void SetFileName(string _fileName){
		fileName = _fileName;
	}

	public static string GenerateTimestamp(){
		return DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
	}

	public static void LogTrigger(string _target){
		string[] msg = {
			"$Trigger \t"+_target+" \t" + GenerateTimestamp()	
		};

		Logger.WriteToLog (msg);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///sets difficuly level
///</summary>
public class Difficulty : MonoBehaviour
{
		/// <summary>
		/// Description:
		/// Calls the Set Difficulty fuction in Enemies with the
		/// difficuly as a parameter
		/// Input:
		/// none
		/// Return:
		/// void
		/// </summary>
		private static int difficulty;
		public static int getDiff(){
			 return difficulty;
		}

		public static void setDiff(string diff){
			switch (diff) {
		        case "Easy":
		               difficulty = 0;
		               break;
		        case "Medium":
		               difficulty = 1;
		               break;
		        case "Hard":
		                difficulty = 2;
		                break;
			 }
		}
	}

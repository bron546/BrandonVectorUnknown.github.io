﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class puzzle_info : MonoBehaviour {

	public Vector3 player_position; 		//player's starting position

	public List< Vector3> goal_positions; 	//target position. May have multiple endpoints
	public List< Vector2> choices; 			//Choices to be displayed on the UI

	public int attempt_count; 				// limiting number of attempts. ( player can only try X amount of times)
	public int trace_mode; 					// 1 is on, path player takes will be rendered
											// 0 is off, path player takes is invisible
	public int future_sight; 				// 1 is on, the next possible vector path will be rendered
											// 0 is off, the next possible vector path will remain inivisible
		
	public void Awake(){
		Reset (); 							// initializes all data structures for base game
	}

	public void Reset(){
		
		player_position = new Vector3 (0, 0, 0); //inital starting point of <0, 0, 0>
		goal_positions = new List<Vector3> ();
		choices = new List< Vector2> ();
		attempt_count = -1; 					//unlimited attempts
		trace_mode = 1;
		future_sight = 1;

	}

	public bool game_over(){
		return (attempt_count == 0); 
	}

	public int decrement_attempts(){

		attempt_count--;

		if (attempt_count == 0)
			return 1; // returns true, max attempts reached

		return 0; // game continues
	}

	public void log( string path){

		StreamWriter log = new StreamWriter ( path);
		log.Write ("");
		log.Close ();

		log = new StreamWriter ( path, true);

		log.WriteLine ("Starting Position : " + player_position.ToString ());
		log.WriteLine ("Goal points : ");
		foreach( Vector3 goal in goal_positions)
			log.WriteLine ("\t" + goal.ToString());
		log.WriteLine ("Choices :");
		foreach (Vector2 choice in choices)
			log.WriteLine ("\t"+choice.ToString ());
		log.WriteLine ("Attempts : " + attempt_count.ToString ());
		log.WriteLine ("Trace : " + (trace_mode == 1 ? "ON" : "OFF"));
		log.WriteLine ("Future Sight : " + (future_sight == 1 ? "ON" : "OFF"));

		log.Close ();
	}

	public int getFutureSight()
	{
		return future_sight;
	}

	public void setFutureSight(int choice)
	{
		if (choice == 0 || choice == 1)
		{
			future_sight = choice;
		}
	}

}

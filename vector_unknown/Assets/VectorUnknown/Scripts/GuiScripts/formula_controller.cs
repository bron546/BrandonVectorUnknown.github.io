﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Author : Nate Cortes
 * Manages Formulas from the top UI element. 
 * This class watches for changes in the UI element's data. 
 * All game objects that need the player's input will get the linear algebra representation from this class.
 * Supports operations such as formating, logging, and data passing
 * Made for CPI441 capstone. 
 */

public class formula_controller : MonoBehaviour {
	///  ( constant_1 * vector_1) + ( constant_2 * vector_2) = output /// 
	public int constant_1;
	public int constant_2;
	public Vector2 vector_1;
	public Vector2 vector_2;
	public Vector2 output;
	//////////////////////

	public GameObject player;
	public LineRenderer line_1, second_line;
	public Transform c1, c2, v1, v2, destination;
	public bool change = false;

	void Awake(){
		GameObject formula_panel = GameObject.FindGameObjectWithTag ( "Formula");
		constant_1 = 0; constant_2 = 0;
		vector_1 = Vector2.zero; vector_2 = Vector2.zero; output = Vector2.zero;
		player = GameObject.Find ("Player");

		line_1 = GameObject.Find ("Arrow1").GetComponent< LineRenderer>();
		second_line = GameObject.Find ("Arrow2").GetComponent< LineRenderer>();

		c1 = formula_panel.transform.GetChild (0);
		constant_1 = c1.GetComponent< constant_counter> ().constant;
		v1 = formula_panel.transform.GetChild (1);

		c2 = formula_panel.transform.GetChild (3);
		constant_2 = c2.GetComponent< constant_counter> ().constant;
		v2 = formula_panel.transform.GetChild (4);

		destination = formula_panel.transform.GetChild (6);

	}

	void Update(){
		int con_1 = c1.GetComponent< constant_counter> ().constant; //values of constant ui buttons
		int con_2 = c2.GetComponent< constant_counter> ().constant;
		int v1_cc = v1.childCount; //positive value if dropper element has choice attached to it
		int v2_cc = v2.childCount;

		//Check for Drag & Drop Changes
		if (constant_1 != con_1) {
			constant_1 = con_1;
			change = true;
		}
		if (constant_2 != con_2) {
			constant_2 = con_2;
			change = true;
		}
		if (v1_cc > 0) {
			Vector2 vect_1 = v1.GetChild (0).GetComponent< choice_holder> ().choice;
			if (vector_1 != vect_1) {
				vector_1 = vect_1;
				change = true;
			}
		} else {
			vector_1 = Vector2.zero;
		}
		if (v2_cc > 0){
			Vector2 vect_2 = v2.GetChild (0).GetComponent< choice_holder> ().choice;
			if (vector_2 != vect_2) {
				vector_2 = vect_2;
				change = true;
			}
		} else {
			vector_2 = Vector2.zero;
		}

		if (change) {
			//construct output
			Vector2 outp = (constant_1 * vector_1) + (constant_2 * vector_2);
			if (outp != output) { 
				output = outp;
				destination.GetChild (0).GetComponent< Text> ().text = output.x.ToString ("F0") + "\n" + output.y.ToString ("F0");
			}

			//construct "Future Sight" 
			Vector3[] points = new Vector3[2];
			Vector3 start = player.transform.position;
			points [0] = start - new Vector3 (0, 2.5f, 0);
			points [1] = constant_1 * new Vector3 (vector_1.x, 0.0f, vector_1.y) + points [0];
			line_1.SetPositions (points);
			points [0] = points [1];
			points [1] = constant_2 * new Vector3 (vector_2.x, 0.0f, vector_2.y) + points [0]; 
			second_line.SetPositions (points);
	
		}
		change = false;

	}

	public string Log_formula(){//formats a vector equation for logging, "( c1 * < x1, y1>) + ( c2 * < x2, y2>) = <dx, dy>"
		return "( "+constant_1+" * "+string_vector( vector_1)+") + ( "+constant_2+" * "+string_vector( vector_2)+") = "+string_vector( output);
	}

	public string string_vector( Vector2 vect){//formats a vector, < x, y>
		return "< " + vect.x.ToString () + ", " + vect.y.ToString () + ">";
	}

	public void move_player(){
		/************************************************/
		/* STEP 1: Send movement Coordinates to Player  */
		/************************************************/
		Vector3[] move = new Vector3[2];
		move [0] = constant_1 * new Vector3( vector_1.x, 0f, vector_1.y);
		move [1] = constant_2 * new Vector3( vector_2.x, 0f, vector_2.y);

		player.GetComponent<PlayerMovement> ().Move (move);
		/************************************************/
	}

	public void path_trace(){
		/************************************************/
		/* STEP 2: Send Coordinates to Path Renderer    */
		/************************************************/
	}

	public void reset(){
		/************************************************/
		/* STEP 3: Clear UI Elements holding formula    */
		/************************************************/
		c1.GetComponent< constant_counter> ().reset ();
		c2.GetComponent< constant_counter> ().reset ();
		v1.GetComponent< choice_holder> ().update_choice (new Vector3 (0, 0, 0));
		v2.GetComponent< choice_holder>().update_choice (new Vector3 (0, 0, 0));
	}
}

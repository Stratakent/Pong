//Author: Brian Nguyen
//Email: Ignisfaire@csu.fullerton.edu
//Course: 223N
//Assignment Number: 6
//Due Date: 11/17/2016
//The goal of this program is to emulate the game pong. This is the main function.
//File name: pongmain.cs

using System;
using System.Windows.Forms;          
public class pongmain
{
	public static void Main()
	{
		System.Console.WriteLine("The program will now simulate the game pong.");
		ponginterface pong = new ponginterface();
		Application.Run(pong);
		System.Console.WriteLine("The game had finished.");
	}//End of Main function
}//End of main class
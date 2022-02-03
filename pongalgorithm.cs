//Author: Brian Nguyen
//Email: Ignisfaire@csu.fullerton.edu
//Course: 223N
//Assignment Number: 5
//Due Date: 12/17/2016
//The goal of this program is to emulate the game pong. This is the RNG, buttons and angle deflectors.
//File name: applealgorithm.cs

using System;
using System.Windows.Forms;

namespace Buttontypes
{
	public class NoFocusButton : Button
	{
		public NoFocusButton()
		{
			SetStyle(ControlStyles.Selectable, false);
		} 
	}//End of NoFocusButton class

	public class RNG
	{
		public static double generator()
		{
			Random next = new Random();
			int number=90;
			while ((number>80&&number<100) || (number > 260&&number<280)) //try to get a number that is not close to 90 or 270 to avoid being forced to bounce up and down, or take too long.
			{
				number = next.Next(1,359);
			}
			return number;
		}
	}

	public class bouncer//reflecting the ball trajectory. //might be useful but no guarantees...
	{
		public static double xmovement(double input)
		{
			//This method returns a degrees into radians
			double newinput = (System.Math.PI / 180) * input;
			newinput = System.Math.Cos(newinput);
			return newinput;
		}//End of degrees to radians

		public static double ymovement(double input)
		{
			double newinput = (System.Math.PI / 180) * input;
			newinput = System.Math.Sin(newinput);
			return newinput;
		}

		public static double horizontalreverse(double input)
		{
			double newinput = System.Math.Acos(input);
			newinput = newinput * (180 / System.Math.PI);
			if (newinput < 0)
			{
				newinput = newinput + 360;
			}
			if (newinput >= 360)
			{
				newinput = newinput - 360;
			}
			return newinput;
		}

		public static double verticalreverse(double input)
		{
			double newinput = System.Math.Asin(input);
			newinput = newinput * (180 / System.Math.PI);
			if (newinput < 0)
			{
				newinput = newinput + 360;
			}
			if (newinput >= 360)
			{
				newinput = newinput - 360;
			}
			return newinput;
		}

		public static double tangentthird(double input)
		{
			double newinput = System.Math.Atan(input);
			newinput = newinput * (180 / System.Math.PI);
			newinput = newinput + 180;
			return newinput;
		}

		public static double tangentfourth(double input)
		{
			double newinput = System.Math.Atan(input);
			newinput = newinput * (180 / System.Math.PI);
			if (newinput < 0)
			{
				newinput = newinput + 360;
			}
			if (newinput >= 360)
			{
				newinput = newinput - 360;
			}
			return newinput;
		}
	}
}//End of Buttontypes namespace
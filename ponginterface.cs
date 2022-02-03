//Author: Brian Nguyen
//Email: Ignisfaire@csu.fullerton.edu
//Course: 223N
//Assignment Number: 5
//Due Date: 11/26/2016
//The goal of this program is to emulate the game pong. This is the frame function.
//File name: appleinterface.cs

using System;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Buttontypes;

public class ponginterface : Form //Prepare to use the frame class, to insert and use in the main.
{
	private const int formwidth = 1920;
	private const int formheight = 1080;
	private Pen ballpoint = new Pen (Color.Black, 1);
	private Pen grapher = new Pen (Color.Crimson, 1);
	//All the dimensions ready for use.
	private const int radiusa = 12;

	private double horizontaldelta; //ball movements.
	private double verticaldelta;
	private double ballspeed = 0;
	private int degrees;

	private int playermovement1;
	private int playermovement2;

	private bool animation = false;
	private bool pausegame = false;

	private int paddle1x = 50;
	private int paddle1y = 400;
	private int paddle2x = 1620;
	private int paddle2y = 400;

	private int Lpoints = 0;
	private int Rpoints = 0;

	private string Lplayer;
	private string Rplayer;
	private string initial;

	private int defaultx = formwidth/2-125;//get these numbers into the middle.
	private int defaulty = formheight/2-80;

	private int bally;//these will be the default values
	private int ballx;

	private const double ballrate = 6.0;
	private static System.Timers.Timer ballcontrol = new System.Timers.Timer();
	private bool ballactive = false;
	private bool playing = false;
	private static System.Timers.Timer textcontrol = new System.Timers.Timer();
	private bool textactive = false;

	private Label P1loc = new Label();
	private TextBox P1points = new TextBox();
	private Label P2loc = new Label();
	private TextBox P2points = new TextBox();
	private Label speed = new Label();
	private TextBox sploc = new TextBox();
	
	//The buttons
	private Button start = new Button();
	private Button newgame = new Button();
	private Button exit = new Button();
	private Button pause = new Button();
	private Point Sloc = new Point(250, 940);
	private Point Nloc = new Point(400, 955);
	private Point EXloc = new Point(1350, 955);
	private Point Ploc = new Point (250, 970);

	enum keyposition { up, down, w, s }
	private keyposition uparrow = keyposition.up;
	private keyposition downarrow = keyposition.down;
	private keyposition wkey = keyposition.w;
	private keyposition skey = keyposition.s;

	public ponginterface()   //The constructor of this class
	{  //Set the title of this form.
		ballx = defaultx;
		bally = defaulty;
		BackColor = Color.Black;
		Size = new Size(formwidth, formheight);
		System.Console.WriteLine(ballx);

		Lplayer = Lpoints.ToString();
		Rplayer = Rpoints.ToString();
		initial = ballspeed.ToString();

		//Initialize clock of ball
		ballcontrol.Enabled = false;
		ballcontrol.Elapsed += new ElapsedEventHandler(updateball);
		ballclock(ballrate);    //Set the animation rate for ball a.
		
		textcontrol.Enabled = false;
		textcontrol.Elapsed += new ElapsedEventHandler(updatetext);
		textclock();

		P1loc.Location = new Point(165,30);
		P2loc.Location = new Point(1420,30);
		P1points.Location = new Point(165,50);
		P2points.Location = new Point(1420,50);
		speed.Location = new Point(855, 940);
		sploc.Location = new Point(855, 960);

		P1loc.Text = "Player 1";
		P1loc.BackColor = Color.Red;
		P2loc.Text = "Player 2";
		P2loc.BackColor = Color.Blue;
		speed.Text = "Initial Speed";
		speed.BackColor = Color.Green;
		P1points.Text = Lplayer;
		P2points.Text = Rplayer;
		sploc.Text = initial;

		Controls.Add(P1loc);
		Controls.Add(P2loc);
		Controls.Add(speed);
		Controls.Add(P1points);
		Controls.Add(P2points);
		Controls.Add(sploc);

		start.Text = "Go!";
		start.Size = new Size(100, 30);
		start.Location = Sloc;
		start.BackColor = Color.ForestGreen;
		newgame.Text = "New Game";
		newgame.Size = new Size(100, 30);
		newgame.Location = Nloc;
		newgame.BackColor = Color.Teal;
		pause.Text = "Pause";
		pause.Size = new Size (100, 30);
		pause.Location = Ploc;
		pause.BackColor = Color.Yellow;
		exit.Text = "Exit";
		exit.Size = new Size(100, 30);
		exit.Location = EXloc;
		exit.BackColor = Color.Crimson;
		Controls.Add(start);
		Controls.Add(newgame);
		Controls.Add (pause);
		Controls.Add(exit);
		start.Click += new EventHandler(beginprogram);
		newgame.Click += new EventHandler(newround);
		pause.Click += new EventHandler (pauseprogram);
		exit.Click += new EventHandler(finishprogram);

		KeyPreview = true;
		KeyUp += new KeyEventHandler(OnKeyUp);

		DoubleBuffered = true;
	}//End of constructor

	protected override void OnPaint(PaintEventArgs eee) //values to prepare the shapes
	{
		Graphics graph = eee.Graphics;
		graph.FillRectangle(Brushes.Navy, 0, 920, formwidth, formheight);
		graph.FillRectangle(Brushes.Goldenrod, 10, 930, formwidth - 25, formheight - 30);
		graph.FillEllipse(Brushes.White, ballx, bally, 25, 25);
		graph.FillRectangle (Brushes.White, paddle1x, paddle1y, 20, 115); //player 1
		graph.FillRectangle(Brushes.White, paddle2x, paddle2y, 20, 115); //player 2
		base.OnPaint(eee);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys KeyCode)
	{
		if (KeyCode == Keys.Up) //consistency with keyboard positions.
		{
			uparrow = keyposition.down;
			if (downarrow == keyposition.up)
				playermovement2 = -25;
		}

		else if (KeyCode == Keys.Down)
		{
			downarrow = keyposition.down;
			if (uparrow == keyposition.up)
				playermovement2 = 25;
		}

		else
		{
			System.Console.WriteLine("KeyCode = {0}.", KeyCode);
		}

		if (KeyCode == Keys.W)
		{
			wkey = keyposition.s;
			if (skey == keyposition.w)
				playermovement1 = -25;
		}

		else if (KeyCode == Keys.S)
		{
			skey = keyposition.s;
			if (wkey == keyposition.w)
				playermovement1 = 25;
		}

		else
		{
			System.Console.WriteLine("KeyCode = {0}.", KeyCode);
		}

		if (playing == true)//forcing it to stay put in same locations and prevent from any "cheap" starts
		{
			paddle1y += playermovement1;
			paddle2y += playermovement2;

			if (paddle1y < 0)
			{
				paddle1y = 0;
			}
			if (paddle1y+115 > 910)
			{
				paddle1y=815;
			}

			if (paddle2y < 0)
			{
				paddle2y = 0;

			}

			if (paddle2y + 115 > 910)
			{
				paddle2y = 815;
			}
		}
		Invalidate();
		return base.ProcessCmdKey(ref msg, KeyCode);
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Up)
		{
			uparrow = keyposition.up;
			if (downarrow == keyposition.down)
				playermovement2 = -25;
			else
				playermovement2 = 0;
		}

		else if (e.KeyCode == Keys.Down)
		{
			downarrow = keyposition.up;
			if (uparrow == keyposition.down)
				playermovement2 = 25;
			else
				playermovement2 = 0;
		}

		if (e.KeyCode == Keys.W)
		{
			wkey = keyposition.w;
			if (skey == keyposition.s)
				playermovement1 = -25;
			else
				playermovement1 = 0;
		}

		else if (e.KeyCode == Keys.S)
		{
			skey = keyposition.w;
			if (wkey == keyposition.s)
				playermovement1 = 25;
			else
				playermovement1 = 0;
		}

		if (playing == true)//forcing it to stay put in same locations and prevent from any "cheap" starts
		{
			paddle1y += playermovement1;
			paddle2y += playermovement2;

			if (paddle1y < 0) //out of boundaries
			{
				paddle1y = 0;
			}
			if (paddle1y + 115 > 910)
			{
				paddle1y = 815;
			}

			if (paddle2y < 0)
			{
				paddle2y = 0;
			}

			if (paddle2y + 115 > 910)
			{
				paddle2y = 815;
			}
		}
		Invalidate();
	}

	protected void ballclock(double refresh)
	{
		double elapsed;
		if (refresh < 1.0)
			refresh = 1.0;
		elapsed = 30.0 / refresh;
		ballcontrol.Interval = (int)System.Math.Round(elapsed);
	}
	
	protected void textclock()
	{
		double elapsed = 1.0;
		textcontrol.Interval = elapsed;
		textcontrol.Enabled = false;  
		textactive = false;
	}
	
	protected void beginprogram(Object sender, EventArgs events)
	{
		if (animation == false)
		{
			if (pausegame == false) //first time with no interruptions.
			{
				ballspeed = double.Parse(sploc.Text);
				degrees = (int)RNG.generator(); //grab some random degrees and direction.
				horizontaldelta = ballspeed * bouncer.xmovement(degrees);
				verticaldelta = ballspeed * bouncer.ymovement(degrees);
			}

			if (pausegame == true)
			{
				pausegame = false;
			}

			playing = true;
			ballcontrol.Enabled = true;
			ballactive = true;
			animation = true;
			textcontrol.Enabled = true;
			textactive = true;
			if ((int)ballspeed == 0)//force the game to stop because the ball will not be moving, period.
			{
				ballspeed = 5.0;
				playing = false;
				ballcontrol.Enabled = false;
				ballactive = false;
				animation = false;
				pausegame = false;
			}

			System.Console.WriteLine("The game begins!");
			System.Console.WriteLine("Please clean up the textboxes when the game finishes!");
		}
	}
	
	protected void pauseprogram(Object sender, EventArgs events)
	{
		if (animation == true)
		{
			playing = false;
			ballcontrol.Enabled = false;
			ballactive = false;
			animation = false;
			pausegame = true;
			System.Console.WriteLine("The game has momentarily stopped playing, press start to resume.");
		}
	}

	protected void newround(Object sender, EventArgs events) //first reset everything and all of its value.
	{
		playing = false;
		ballcontrol.Enabled = false;
		ballactive = false;
		animation = false;
		pausegame = false;
		ballx = defaultx;//reset
		bally = defaulty;
		Lpoints = 0; //reset points to zero.
		Rpoints = 0; //reset points to zero.
		System.Console.WriteLine("The scores have reset; a new game has been set up.");
	}

	protected void updatetext(Object sender, ElapsedEventArgs events) //the scores go here!
	{
		P1points.Text = Lpoints.ToString();
		P2points.Text = Rpoints.ToString();
	}
	
	protected void updateball(Object sender, ElapsedEventArgs events)
	{
		ballx = ballx + (int)horizontaldelta;
		bally = bally + (int)verticaldelta;

		//figure out the boundaries for the paddle, everything else is working fine.

		if (ballx >= paddle1x+10&&ballx<=+paddle1x+20)//ball for arriving at player 1/left player; and only check during that time.
		{
			if (bally > paddle1y && bally <= paddle1y + 115) //bounce the ball back to player 2, ball collided with player 1 paddle
			{
				horizontaldelta = -horizontaldelta;
			}
		}

		if (ballx>=paddle2x-20&&ballx <= paddle2x)//ball for arriving at player 2/right player.
		{
			if (bally > paddle2y && bally <= paddle2y + 115) //bounce the ball back to player 1, ball collided with player 2 paddle
			{
				horizontaldelta = -horizontaldelta;
			}
		}

		//ball colliding with top and bottom wall, nothing to see much here.
		if (bally < 0)
		{
			verticaldelta = -verticaldelta;
		}

		if (bally + 50 > 930)
		{
			verticaldelta = -verticaldelta;
		}

		if (ballx <= -50) //its a score for player 2.
		{
			System.Console.WriteLine("Player 2 has scored.");
			ballx = defaultx;//reset
			bally = defaulty;
			Rpoints++;
			ballspeed = ballspeed * 1.1; //ball is also getting faster...
			degrees = (int)RNG.generator(); //grab some random new degrees and put it in some direction.
			horizontaldelta = ballspeed * bouncer.xmovement(degrees);
			verticaldelta = ballspeed * bouncer.ymovement(degrees);
		}

		if (ballx >= formwidth) //its a score for player 1.
		{
			System.Console.WriteLine("Player 1 has scored.");
			ballx = defaultx;//reset
			bally = defaulty;
			Lpoints++;
			ballspeed = ballspeed * 1.1; //ball is also getting faster...
			degrees = (int)RNG.generator(); //grab some random new degrees and put it in some direction.
			horizontaldelta = ballspeed * bouncer.xmovement(degrees);
			verticaldelta = ballspeed * bouncer.ymovement(degrees);
		}

		if (Lpoints==10||Rpoints==10)//A player has scored ten points, and the winner is...?
		{
			playing = false;
			ballcontrol.Enabled = false;
			ballactive = false;
			animation = false;
			System.Console.WriteLine("And the winner is...");
			if (Lpoints == 10)
			{
				System.Console.WriteLine("Player 1/Left Player!");
			}
			if (Rpoints == 10)
			{
				System.Console.WriteLine("Player 2/Right Player!");
			}
		}

		Invalidate();
	}
	
	protected void finishprogram(Object sender, EventArgs events)
	{
		System.Console.WriteLine("Pong is now ending...");
		Close();
	}
}
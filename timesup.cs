using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TimerNS;

public class TimesUp : Form
{
    private Button button; 
    private Button deleteButton; 
		//ContextMenu mnu;
    private StatusBarPanel statusbar;
    private DateTimePicker dtp = new DateTimePicker();
    //private Label timeLeftLabel;
    private TextBox textMsg;
    private	TreeView timerTree;
    List<TimerObj> timerList = new List<TimerObj>();
		private int currentIndex=-1;
    //private ContextMenu mnu;

    public TimesUp ()
    {
		    int x=90;
		    int y=50;

        Text = "Times Up! Timer Application";

        this.BackColor = System.Drawing.Color.Blue;
        this.Padding = new System.Windows.Forms.Padding(20);

		    textMsg = new TextBox();
        //textMsg.Location = new Point(20, 60);
        textMsg.Location = new Point(x, y);
		    textMsg.Text = "Enter text here";
        textMsg.Parent = this;
		    y=y+35;

        button = new Button();
        button.BackColor = System.Drawing.Color.Gray;
        button.Text = "Start Timer";
        button.Location = new Point(x-25, y);
        button.Click += new EventHandler(OnClick);
        button.Size = new Size(75,25);
        button.Parent = this;

        deleteButton = new Button();
        deleteButton.BackColor = System.Drawing.Color.Gray;
        deleteButton.Text = "Delete";
        deleteButton.Location = new Point(x+55, y);
        deleteButton.Click += new EventHandler(DeleteClicked);
        deleteButton.Size = new Size(75,25);
        deleteButton.Parent = this;

        dtp.Format = DateTimePickerFormat.Time;
        dtp.Size = new Size(500,100);
        dtp.Dock = DockStyle.Fill;
        dtp.Parent = this;
   		  y=y+30;

		    timerTree = new TreeView();
        timerTree.Location = new Point(20, y);
        timerTree.Size = new Size(250,100);
        timerTree.Click += new EventHandler(TreeClicked);
		    timerTree.Parent = this;

        StatusBar statusBar1 = new StatusBar();
        statusbar = new StatusBarPanel();
        statusBar1.BackColor = System.Drawing.Color.Gray;
        statusbar.Text = "Ready...";
        statusBar1.ShowPanels = true;
        statusBar1.Panels.Add(statusbar);
        this.Controls.Add(statusBar1);

        CenterToScreen();
    }

    void resetTimer() {
        button.Text = "Start Timer";
        //myTimer.Stop();
        return;
    }

    void TreeClicked(object sender, EventArgs e) {
				Console.WriteLine( " tree clicked");
				TreeNode node = timerTree.SelectedNode;
				currentIndex=node.Index;
  			//Console.WriteLine( currentIndex );
    }

    void DeleteTimer(int index) {
				timerList[index].Stop();
				timerList.RemoveAt(index);
				timerTree.Nodes[index].Remove();
    }

    void DeleteClicked(object sender, EventArgs e) {
				Console.WriteLine( " delete clicked");
  			Console.WriteLine( currentIndex );
				if ( -1 == currentIndex ) return;

				DeleteTimer(currentIndex);

    } // END DeleteClicked()

   void OnTimerDeleted(object sender, EventArgs e) {
				Console.WriteLine("TODO: find and delete appropriate node on tree");
				// timerTree.Nodes[currentIndex].Remove();
   }

   void OnClick(object sender, EventArgs e) {

        DateTime chosenTime=dtp.Value;
        TimeSpan setTime=chosenTime.Subtract(DateTime.Now);

        if (setTime.TotalSeconds < 1){
            statusbar.Text = "Time Must be in Future";
            return;

        }

        statusbar.Text = "Timer started.";
        button.Text = "Stop";

	      TimerObj myTimer = new TimerObj();
				myTimer.TimerDeleted += new TimerObj.ChangedEventHandler(OnTimerDeleted);

				TreeNode node = new TreeNode(textMsg.Text);
				node.Name = myTimer.ObjectId.ToString();
				timerTree.Nodes.Add(node);

        /* Adds the event and the event handler for the method that will 
	  	  *           process the timer event to the timer. */
		    myTimer.Tick += new EventHandler(TimerEventProcessor);

		    myTimer.Interval = (int) setTime.TotalSeconds * 1000;
				myTimer.setText(textMsg.Text);
				timerList.Add(myTimer);
		    myTimer.Start();
    }

    private void TimerEventProcessor(Object myObject, EventArgs myEventArgs) {
        resetTimer();
     }

    static public void Main ()
    {
        Application.Run (new TimesUp ());
    }

} // END public class TimesUp : Form
// Sat May 24 17:29:07 PDT 2014

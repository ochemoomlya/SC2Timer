using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;
using Utilities;

namespace key_preview {
	public partial class mainForm : Form {
		KeyboardSpy kSpy = new KeyboardSpy();
        System.Timers.Timer timer;
        List<string> CurrentCombination,
                LastSavedCombination;

        public mainForm()
        {
            InitializeComponent();
            Load += mainForm_Load;
            tbUserCombination.Enter += tbUserCombination_Enter;
            tbUserCombination.Leave += tbUserCombination_Leave;
            btnSave.Click += btnSave_Click;
        }

        void tbUserCombination_Leave(object sender, EventArgs e)
        {
           // kSpy.CombinationDown = null;
            kSpy.CombinationUp -= kSpy_CombinationUp;
            UpdateLog(LastSavedCombination);
        }

        void tbUserCombination_Enter(object sender, EventArgs e)
        {
            kSpy.CombinationDown += kSpy_CombinationDown;
            kSpy.CombinationUp += kSpy_CombinationUp;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            LastSavedCombination = CurrentCombination;
            kSpy.RequiredCombination = LastSavedCombination;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            CurrentCombination = new List<string>();
            kSpy.RunFullSpy();
            timer = new System.Timers.Timer();
            timer.Interval = 30;
            timer.Elapsed += timer_Elapsed;
            tbInterval.DataBindings.Add("Text", timer, "Interval");
            Binding b = new Binding("Text", CurrentCombination, "Keys");
            b.Format += b_Format;
            //tbLog.DataBindings.Add(b);
		}

        void kSpy_CombinationUp(List<string> keys)
        {
            btnSave.Enabled = true;
            //tbLog.Clear();
        }

        void kSpy_CombinationDown(List<string> keys)
        {
            //CurrentCombination.
            CurrentCombination = keys;
            UpdateLog(CurrentCombination);
        }

        void b_Format(object sender, ConvertEventArgs e)
        {
            string resString="";
            foreach(string key in (IEnumerable<string>)e.Value)
            {
                resString = key+"+";
            }
            if(resString!="")
                resString.TrimEnd('+');
            e.Value = resString;
        }

        void UpdateLog(IEnumerable<string> keys)
        {
            string resString = "";
            foreach (string key in keys)
            {
                resString += key + "+";
            }
            if (resString != "")
                resString.TrimEnd('+');
            tbUserCombination.Text = resString;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //make ding dong
        }
	}
}
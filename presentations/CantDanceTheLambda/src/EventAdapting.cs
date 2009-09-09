// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: Events - The Right Way
// Demonstrates how to get make an event handler with the method signature YOU want and how to make new events in a more reasonable style

using System;
using System.Windows.Forms;

namespace CantDanceTheLambda {
    public partial class EventAdapting : Form {
        #region Which is better?
        private void btnSayHello_Click(object sender, EventArgs e) {
            MessageBox.Show("What the heck is sender and e?");
        }
        private void ShowHelloDialog() {
            MessageBox.Show("This method name and signature shows are intent much better");
        }
        #endregion
        
        public EventAdapting() {
            InitializeComponent();
            // Adapt event handler method signature
            btnSayHello.Click += (o, e) => ShowHelloDialog();
            // Inline event handler
            this.Load += (o, e) => Console.WriteLine("This can be a good alternative to a method");
            // Inline event handler that just routes to a presenter
            this.Load += (o, e) => _presenter.Start(this);
        }
        EventAdaptingPresenter _presenter = new EventAdaptingPresenter();

        /// <summary>
        /// Now that we have lambdas this is a better way to declare events
        /// </summary>
        public event Action OnSomethingImportant = delegate { };
    }

    public class EventAdaptingPresenter {
        public void Start(EventAdapting form) { }
    }
}

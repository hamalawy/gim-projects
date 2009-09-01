using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CantDanceTheLambda {
    public partial class EventAdapting : Form {
        private void btnSayHello_Click(object sender, EventArgs e) {
            MessageBox.Show("What the heck is sender and e?");
        }
        private void ShowHelloDialog() {
            MessageBox.Show("This method name and signature shows are intent much better");
        }
        public EventAdapting() {
            InitializeComponent();
            btnSayHello.Click += (o, e) => ShowHelloDialog();

            this.Load += (o, e) => Console.WriteLine("This can be a good alternative to a method");
            
            this.Load += (o, e) => _presenter.Start(this);
        }
        EventAdaptingPresenter _presenter = new EventAdaptingPresenter();

        public event Action OnSomethingImportant = delegate { };
    }
    public class EventAdaptingPresenter {
        public void Start(EventAdapting form) { }
    }
}

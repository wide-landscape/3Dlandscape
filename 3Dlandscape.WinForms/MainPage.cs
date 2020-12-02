using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Urho.Desktop;
using Urho.Extensions.WinForms;

namespace _3Dlandscape.WinForms
{
    public partial class MainPage : Form
    {
        Application currentApplication;
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        UrhoSurface surface;
        public MainPage()
        {
            InitializeComponent();
            // var type = (Type)_3Dlandscape.WorldView;

            surface = new UrhoSurface();
            surface.Dock = DockStyle.Fill;
            urhoSurfacePlaceholder.Controls.Add(surface);

           // var app = surface.Show

           // var app = surface.Show(type, new ApplicationOptions("Data"));
        }

        private void urhoSurfacePlaceholder_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

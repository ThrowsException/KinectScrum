using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace KinectDev
{
    /// <summary>
    /// Interaction logic for TaskCards.xaml
    /// </summary>
    public partial class TaskCards : UserControl
    {
        public String Story { get; set; }

        public TaskCards()
        {
            InitializeComponent();
            DataContext = this;
            Story = "New Story";
        }

        private void TextBlock_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Debug.WriteLine(e.CumulativeManipulation);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.SqlServer.Management.Smo;

namespace ProductConfig
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    //[ContentProperty("Footer")]
    public partial class DataConnector : UserControl
    {
        public DataConnector()
        {
            InitializeComponent();
            vm = (DataConnectorViewModel)this.DataContext;
        }

        private DataConnectorViewModel vm;
    }
}

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using ProductConfig.Infrastructure;

namespace ProductConfig
{
    public class DataSourceAreaViewModel :ViewModelBase
    {
        private static ObservableCollection<Tuple<string,string,string>> _source;
        List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>();
        private ICommand mChangeDataSourceCommand;
        public string DataSourceDescription
        {
            get { return Properties.Resources.DataSourceDescription; }
        }

        public DataSourceAreaViewModel()
        {
            
	
        }
        public ICommand ChangeDataSourceCommand
        {
            get
            {
                if (this.mChangeDataSourceCommand == null)
                {
                    this.mChangeDataSourceCommand = new RelayCommand(this.ChangeDataSource);
                }
                return this.mChangeDataSourceCommand;
            }
        }
      

	// Use Sort method with Comparison delegate.
	// ... Has two parameters; return comparison of Item2 on each.
	


        private void ChangeDataSource()
        {
            ChangeDataSourceView changeDataSource = new ChangeDataSourceView();
            if (changeDataSource.ShowDialog().Value)
            {
                
            }
        }
    }
}
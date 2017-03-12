using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ProductConfig.Infrastructure;
using ProductConfig.Model;

namespace ProductConfig
{
    public class ChangeDataSourceViewModel : ViewModelBase
    {
        private IDictionary<string, DatabaseProvider> mProviders;
        private DatabaseProvider mSelectedDataProvider;
        private string _sourceName;
        private string _description;


        public Tuple<string, IList<string>, string> SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        //private  ObservableCollection<Tuple<string, string, string>> _source;
        List<Tuple<string, IList<string>, string>> list = new List<Tuple<string, IList<string>, string>>();
        private Tuple<string, IList<string>, string> _selectedItem;

        /*     public IDictionary<string, DatabaseProvider> Providers
             {
                 get
                 {
                     return this.mProviders;
                 }
             }*/
        public List<Tuple<string, IList<string>, string>> SouceInfo
        {
            get
            {

                return this.list;
            }
        }
        /*  public String SourceName
          {
              get { return _sourceName; }
          }
          public String Description
          {
              get { return _description; }
          }*/
        /*    public DatabaseProvider SelectedDataProvider
            {
                get
                {
                    return this.mSelectedDataProvider;
                }
                set
                {
                    if (this.mSelectedDataProvider != value)
                    {
                        this.mSelectedDataProvider = value;
                        base.OnPropertyChanged("SelectedDataProvider");
                    }
                }
            }*/
        public ChangeDataSourceViewModel()
        {
            // this.InitializeDataSource();

            list.Add(new Tuple<string, IList<string>, string>("Microsoft Access Database File", new List<string> { ".Net Framework Data Provider for OLE DB" }, "Use this selection to connect to a Microsoft Access database file through the .NET Framework Data Provider for OLE DB"));

            list.Add(new Tuple<string, IList<string>, string>("Microsoft ODBC Data Source", new List<string> { ".Net Framework Data Provider for ODBC" }, "Use this selection to specify an ODBC user or system data sourcce name to connect to an ODBC driver  through the .NET Framework Data Provider for ODBC"));

            list.Add(new Tuple<string, IList<string>, string>("Microsoft SQL Server", new List<string> { ".Net Framework Data Provider for OLE DB", "sql", "fuc" }, "Use this selection to connect to Microsoft SQL Server 2005 or above, or to Microsoft SQL Azure using the .NET Framework Data Provider for SQL Server"));
        }
        /*   private void InitializeDataSource()
           {
               if (this.Providers == null || this.Providers.Count == 0)
               {
                   IDatabaseProviderRegistry iDatabaseProviderRegistry = ServiceLocator.Current.Resolve<IDatabaseProviderRegistry>();
                   Guard.ArgumentNotNull(iDatabaseProviderRegistry, "iDatabaseProviderRegistry");
                   this.mProviders = iDatabaseProviderRegistry.DatabaseProviders;
                   if (this.mProviders.Count > 0)
                   {
                       this.mSelectedDataProvider = this.mProviders.Values.FirstOrDefault<DatabaseProvider>();
                   }
               }
           }*/
    }
}

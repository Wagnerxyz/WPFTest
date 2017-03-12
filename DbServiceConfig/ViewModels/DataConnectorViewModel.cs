using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using ProductConfig.Infrastructure;

namespace ProductConfig
{
    internal class DataConnectorViewModel : ViewModelBase
    {
        private readonly SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder
        {
            IntegratedSecurity = true
        };

        public Server Srv;
        private string _attachDbFilename = string.Empty;
        private ServerConnection _conn;
        private string _connectResult;
        private ObservableCollection<string> _databases = new ObservableCollection<string>();
        private bool _isRestore;
        private int _progress;
        private string _selectedDb;
        private string _selectedServer;
        private ObservableCollection<string> _servers = new ObservableCollection<string>();
        private bool _serversLoading;
        private Visibility _visible = Visibility.Hidden;

        public DataConnectorViewModel()
        {
            var dlg = new OpenFileDialog();
            // dlg.DefaultExt = ".mdf";
            // dlg.Filter = "Microsoft SQL Server Database(.mdf)|*.mdf|Backup Files (*.bak)|*.bak|All files (*.*)|*.*"; 
            dlg.Filter = "数据库备份文件|*.mdf;*.bak;*.sql;";
            LoadServersAsync();
            OpenCommand = new RelayCommand(() =>
            {
                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    // Open document
                    AttachDbFilename = dlg.FileName;
                }
            });
            ConnectCommand = new RelayCommand(Connect, () => (!string.IsNullOrEmpty(SelectedServer)) && (IntegratedSecurity || (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))));
            RefreshCommand = new RelayCommand(LoadServersAsync, () => !ServersLoading);
            BackUpCommand = new RelayCommand(BackUpDb, () => SelectedDb != null);
            RestoreCommand = new RelayCommand(RestoreDb, () => Path.GetExtension(_attachDbFilename).ToUpperInvariant() == ".BAK");
            VerifyCommand = new RelayCommand(Verify, () => Path.GetExtension(_attachDbFilename).ToUpperInvariant() == ".BAK");
            ExecuteSQLCommand = new RelayCommand(ExecuteSql, () => Path.GetExtension(_attachDbFilename).ToUpperInvariant() == ".SQL");
            AttachDbCommand = new RelayCommand(() =>
            {
                try
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_attachDbFilename);
                    Srv.AttachDatabase(fileNameWithoutExtension, new StringCollection { _attachDbFilename });
                    MessageBox.Show("Attach Complete");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            },
                () => Path.GetExtension(_attachDbFilename).ToUpperInvariant() == ".MDF");
        }

        public string ConnectionString
        {
            get { return _builder.ConnectionString; }
        }

        public RelayCommand OpenCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand BackUpCommand { get; set; }
        public RelayCommand ExecuteSQLCommand { get; set; }
        public RelayCommand RestoreCommand { get; set; }
        public RelayCommand VerifyCommand { get; set; }
        public RelayCommand AttachDbCommand { get; set; }


        public ObservableCollection<string> Databases
        {
            set
            {
                _databases = value;
                OnPropertyChanged("Databases");
            }
            get { return _databases; }
        }

        public ObservableCollection<string> Servers
        {
            set
            {
                _servers = value;
                OnPropertyChanged("Servers");
            }
            get { return _servers; }
        }

        public Visibility Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }

        /// <summary>
        ///     Restore Or Attach DB
        /// </summary>
        public bool IsRestore
        {
            get { return _isRestore; }
            set
            {
                _isRestore = value;
                OnPropertyChanged("IsRestore");
            }
        }

        public string UserName
        {
            get { return _builder.UserID; }
            set
            {
                if (_builder.UserID == value) return;
                _builder.UserID = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get { return _builder.Password; }
            set
            {
                if (_builder.Password == value) return;
                _builder.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public bool IntegratedSecurity
        {
            get { return _builder.IntegratedSecurity; }
            set
            {
                if (_builder.IntegratedSecurity == value) return;
                _builder.IntegratedSecurity = value;
                OnPropertyChanged("IntegratedSecurity");
            }
        }

        public string AttachDbFilename
        {
            get { return _attachDbFilename; }
            set
            {
                if (_attachDbFilename == value) return;
                _attachDbFilename = value;
                OnPropertyChanged("AttachDBFilename");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                _selectedServer = value;
                //if (_dbLoader.IsBusy)
                //{
                //    _dbLoader.CancelAsync();
                //}
                //_servers.Clear();
                //_dbLoader.RunWorkerAsync(ConnectionString);
                // OnPropertyChanged("DatabasesLoading");
                OnPropertyChanged("SelectedServer");
            }
        }

        public string SelectedDb
        {
            get { return _selectedDb; }
            set
            {
                _selectedDb = value;
                OnPropertyChanged("SelectedDB");
            }
        }

        public bool ServersLoading
        {
            get { return _serversLoading; }
            private set
            {
                _serversLoading = value;
                OnPropertyChanged("ServersLoading");
            }
        }

        public string ConnectResult
        {
            get { return "Connected:" + _connectResult; }
            set
            {
                _connectResult = value;
                OnPropertyChanged("ConnectResult");
            }
        }

        private void Connect()
        {
            if (SelectedServer == null)
            {
                MessageBox.Show("Please select server");
                return;
            }
            try
            {
                string sqlSErverInstance = SelectedServer;

                if (IntegratedSecurity)
                {
                    _conn = new ServerConnection();
                    _conn.ServerInstance = sqlSErverInstance;
                    Srv = new Server(_conn);
                }
                else
                {
                    _conn = new ServerConnection(sqlSErverInstance, UserName, Password);
                    Srv = new Server(_conn);
                }

                _databases.Clear();
                foreach (Database db in Srv.Databases)
                {
                    _databases.Add(db.Name);
                }
                ConnectResult = SelectedServer;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BackUpDb()
        {
            Progress = 0;
            Visible = Visibility.Visible;
            var bkp = new Backup();
            try
            {
                string fileName = SelectedDb + ".bak";
                bkp.Action = BackupActionType.Database;
                bkp.Database = SelectedDb;
                bkp.Devices.AddDevice(fileName, DeviceType.File);
                bkp.Incremental = false;
                bkp.PercentCompleteNotification = 5;
                bkp.PercentComplete += (s, e) => Progress = e.Percent;
                bkp.Complete += (s, e) =>
                {
                    Visible = Visibility.Hidden;
                    MessageBox.Show("Database Backed Up To: " + fileName, "BackUpDb");
                };

                bkp.SqlBackupAsync(Srv);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal void RestoreDb()
        {
            Progress = 0;
            Visible = Visibility.Visible;
            var res = new Restore();
            try
            {
                res.Database = SelectedDb;
                res.Action = RestoreActionType.Database;
                res.Devices.AddDevice(AttachDbFilename, DeviceType.File);
                // res.ReadFileList(srv);

                res.PercentCompleteNotification = 5;
                res.ReplaceDatabase = true;
                res.PercentComplete += (s, e) => Progress = e.Percent;
                res.SqlRestoreAsync(Srv);
                res.Complete += (s, e) =>
                {
                    Visible = Visibility.Hidden;
                    MessageBox.Show("Restore of " + AttachDbFilename + " Complete!", "Restore", MessageBoxButton.OK, MessageBoxImage.Information);
                };
                //Task.Factory.StartNew(() => res.SqlRestore(Srv)).ContinueWith(
                //    t =>
                //    {
                //        MessageBox.Show("Restore of " + AttachDbFilename + " Complete!", "Restore", MessageBoxButton.OK, MessageBoxImage.Information);
                //        Visible = Visibility.Hidden;
                //    });
            }
            catch (SmoException exSMO)
            {
                MessageBox.Show(exSMO.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExecuteSql()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var fileInfo = new FileInfo(AttachDbFilename);
                string script = fileInfo.OpenText().ReadToEnd();
                Srv.ConnectionContext.ExecuteNonQuery(script);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void LoadServersAsync()
        {
            ServersLoading = true;
            Servers.Clear();
            //  DataTable dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
            Task<DataTable>.Factory.StartNew(SmoApplication.EnumAvailableSqlServers).ContinueWith(t1 =>
            {
                if (t1.Result.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Result.Rows)
                    {
                        Servers.Add(dr["Name"].ToString());
                    }
                }
                ServersLoading = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Verify()
        {
            if (string.IsNullOrEmpty(AttachDbFilename))
            {
                MessageBox.Show("Please select file");
                return;
            }

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var rest = new Restore();
                rest.Devices.AddDevice(AttachDbFilename, DeviceType.File);
                bool verifySuccessful = rest.SqlVerify(Srv);

                if (verifySuccessful)
                {
                    MessageBox.Show("Backup Verified!", "SMO Demos");
                     DataTable dt = rest.ReadFileList(Srv);
                    //  this.dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Backup NOT Verified!", "SMO Demos");
                }
            }
            catch (SmoException exSMO)
            {
                MessageBox.Show(exSMO.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfTest
{
    public class Customer :IDataErrorInfo {
        private string _firstName;

       

        public ICommand Save;
        private string _lastName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == string.Empty)
                {
                    throw new FileNotFoundException("Age must not be less than 0 or greater than 150.");
                }
                _firstName = value;
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value == string.Empty)
                {
                    throw new FileNotFoundException("Age must noasdasdt be less than 0 or greater than 150.");
                }
                _lastName = value;
            }
        }

        public int Age { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        result = "Please enter a First Name";
                }
                if (columnName == "LastName")
                {
                    if (LastName.Length<3)
                        result = "长度不够";
                }
                if (columnName == "Age")
                {
                    if (Age <= 0 || Age >= 99)
                        result = "Please enter a valid age";
                }
                return result;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Samdoss.CommonLayer
{
    public class SamdossConnection
    {
        #region Constructor(s)

        public SamdossConnection()
        {
            DatabaseName = "samdbconnection";
        }

        #endregion

        #region Private Variables

        private string _connectionString;
        private string _databaseName;
        
        #endregion

        #region Public Properties

        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; } 
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        
        #endregion
    }
}

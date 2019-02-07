using Microsoft.Practices.EnterpriseLibrary.Data;
using Samdoss.CommonLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samdoss.DataLayer
{
    public class DataStore
    {
		#region Private Variables

		private SamdossConnection _myConnection = new SamdossConnection();
		private ScreenMode _screenMode;

		DateTime _currentDateTime;
		string _dataValue;

		#endregion

		public DateTime CurrentDateTime
		{
			get { return _currentDateTime; }
			set { _currentDateTime = value; }
		}
		public string DataValue
		{
			get { return _dataValue; }
			set { _dataValue = value; }
		}

		public ScreenMode ScreenMode
		{
			get { return _screenMode; }
			set { _screenMode = value; }
		}

		#region Constructor(s)

		public DataStore() { }

		public DataStore(bool getAllProperties)
		{			
			if (getAllProperties)
			{
				
			}
		}

		#endregion

		#region Commit Add/Update/Delete Transactions

		public TransactionResult Commit()
		{
			TransactionResult result = null;
			Database db = DatabaseFactory.CreateDatabase(_myConnection.DatabaseName);
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				DbTransaction transaction = connection.BeginTransaction();
				try
				{
					switch (_screenMode)
					{
						case ScreenMode.Add:
							result = AddEdit(db, transaction);
							if (result.Status == TransactionStatus.Failure)
							{
								transaction.Rollback();
								return result;
							}
							break;
						case ScreenMode.View:
							result = new TransactionResult(TransactionStatus.Success);
							break;
						case ScreenMode.Delete:
							//result = Delete(db, transaction);
							if (result.Status == TransactionStatus.Failure)
							{
								transaction.Rollback();
							}
							break;
					}
					transaction.Commit();
					return result;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					if (_screenMode == ScreenMode.Add)
					{
						ErrorLog.LogErrorMessageToDB("", "DataStore.cs", "Commit For Add", ex.Message, _myConnection);
						return new TransactionResult(TransactionStatus.Failure, "Failure Adding DataStore Description");
					}
					if (_screenMode == ScreenMode.Edit)
					{
						ErrorLog.LogErrorMessageToDB("", "DataStore.cs", "Commit For Edit", ex.Message, _myConnection);
						return new TransactionResult(TransactionStatus.Failure, "Failure Updating DataStore Description");
					}
					if (_screenMode == ScreenMode.Delete)
					{
						ErrorLog.LogErrorMessageToDB("", "DataStore.cs", "Commit For Delete", ex.Message, _myConnection);
						return new TransactionResult(TransactionStatus.Failure, "Failure Deleting DataStore Description");
					}
				}
				return result;
			}
		}

		#endregion

		private TransactionResult AddEdit(Database db, DbTransaction transaction)
		{
			int returnValue = 0;
			string sqlCommand = "spAddEdit";
			DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
			db.AddInParameter(dbCommand, "DataValue", DbType.String, _dataValue);
			db.AddInParameter(dbCommand, "CurrentDateTime", DbType.DateTime, DateTime.Now);

			//db.AddInParameter(dbCommand, "UserID", DbType.Int32, _userID);
			//db.AddInParameter(dbCommand, "UserName", DbType.String, _username);
			//db.AddInParameter(dbCommand, "Password", DbType.String, _password);
			//db.AddInParameter(dbCommand, "EmailId", DbType.String, _emailID);
			//db.AddInParameter(dbCommand, "Phonenumber", DbType.String, _phonenumber);
			//db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, _isDelete);
			//db.AddInParameter(dbCommand, "IsEmailVerified", DbType.Boolean, _isEmailVerified);
			//db.AddInParameter(dbCommand, "AddEditOption", DbType.Int16, _addEditOption);

			db.AddParameter(dbCommand, "Return Value", DbType.Int32, ParameterDirection.ReturnValue, "Return Value",
							DataRowVersion.Default, returnValue);

			db.ExecuteNonQuery(dbCommand, transaction);
			returnValue = (Int32)db.GetParameterValue(dbCommand, "Return Value");

			return new TransactionResult(TransactionStatus.Success, "Successfully Added");
		}

		public void GetDataStored()
		{
			try
			{
				Database db = DatabaseFactory.CreateDatabase(_myConnection.DatabaseName);
				string sqlCommand = "spGetDataStored";
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				
				using (SqlDataReader dataReader = (SqlDataReader)db.ExecuteReader(dbCommand))
				{
					if (dataReader.HasRows)
					{
						while (dataReader.Read())
						{
							_dataValue = dataReader.GetString(dataReader.GetOrdinal("DataValue"));
							
						}
					}
				}
			}
			catch
			{
				
			}
		}
	}
}

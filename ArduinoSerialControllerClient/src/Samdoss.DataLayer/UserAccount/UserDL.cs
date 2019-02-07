using Microsoft.Practices.EnterpriseLibrary.Data;
using Samdoss.CommonLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BloomsandBlossomsDL
{
    public class UserDL
    {
        #region Private Variables

        private SamdossConnection _myConnection = new SamdossConnection();
        private ScreenMode _screenMode;

        int _userID;
        string _username;
        string _password;
        string _emailID;
        string _phonenumber;
        bool _isDelete;
        bool _isEmailVerified;
        DateTime _auditDate;
        int _addEditOption = 0;

        #endregion

        #region Public Variables

        public ScreenMode ScreenMode
        {
            get { return _screenMode; }
            set { _screenMode = value; }
        }

        public int UserID
        {
            get
            {
                return _userID;
            }

            set
            {
                _userID = value;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }

        public bool IsDelete
        {
            get
            {
                return _isDelete;
            }

            set
            {
                _isDelete = value;
            }
        }


        public int AddEditOption
        {
            get { return _addEditOption; }
            set { _addEditOption = value; }
        }

        public string EmailID
        {
            get
            {
                return _emailID;
            }

            set
            {
                _emailID = value;
            }
        }

        public string Phonenumber
        {
            get
            {
                return _phonenumber;
            }

            set
            {
                _phonenumber = value;
            }
        }

        public bool IsEmailVerified
        {
            get
            {
                return _isEmailVerified;
            }

            set
            {
                _isEmailVerified = value;
            }
        }
        #endregion

        #region Constructor(s)

        public UserDL() { }

        public UserDL(int userID, bool getAllProperties)
        {
            _userID = userID;
            if (getAllProperties)
            {
               // GetUser();
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
                            result = AddEditUser(db, transaction);
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
                            //result = DeleteUser(db, transaction);
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
                        ErrorLog.LogErrorMessageToDB("", "UserDL.cs", "Commit For Add", ex.Message, _myConnection);
                        return new TransactionResult(TransactionStatus.Failure, "Failure Adding User Description");
                    }
                    if (_screenMode == ScreenMode.Edit)
                    {
                        ErrorLog.LogErrorMessageToDB("", "UserDL.cs", "Commit For Edit", ex.Message, _myConnection);
                        return new TransactionResult(TransactionStatus.Failure, "Failure Updating User Description");
                    }
                    if (_screenMode == ScreenMode.Delete)
                    {
                        ErrorLog.LogErrorMessageToDB("", "UserDL.cs", "Commit For Delete", ex.Message, _myConnection);
                        return new TransactionResult(TransactionStatus.Failure, "Failure Deleting User Description");
                    }
                }
                return result;
            }
        }

        #endregion

        private TransactionResult AddEditUser(Database db, DbTransaction transaction)
        {
            int returnValue = 0;
            string sqlCommand = "spAddEditUser";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, _userID);
            db.AddInParameter(dbCommand, "UserName", DbType.String, _username);
            db.AddInParameter(dbCommand, "Password", DbType.String, _password);
            db.AddInParameter(dbCommand, "EmailId", DbType.String, _emailID);
            db.AddInParameter(dbCommand, "Phonenumber", DbType.String ,_phonenumber);
            db.AddInParameter(dbCommand, "IsDelete", DbType.Boolean, _isDelete);
            db.AddInParameter(dbCommand, "IsEmailVerified", DbType.Boolean, _isEmailVerified);
            db.AddInParameter(dbCommand, "AddEditOption", DbType.Int16, _addEditOption);

            db.AddParameter(dbCommand, "Return Value", DbType.Int32, ParameterDirection.ReturnValue, "Return Value",
                            DataRowVersion.Default, returnValue);

            db.ExecuteNonQuery(dbCommand, transaction);
            returnValue = (Int32)db.GetParameterValue(dbCommand, "Return Value");

            _userID = returnValue;

            if (returnValue == 0)
            {
                return new TransactionResult(TransactionStatus.Failure, "User Already Exists");
            }
            else if (returnValue == -1)
            {
                if (_addEditOption == 1)
                    return new TransactionResult(TransactionStatus.Failure, "Failure Updating");
                else
                    return new TransactionResult(TransactionStatus.Failure, "Failure Adding");
            }
            else
            {
                if (_addEditOption == 1)
                    return new TransactionResult(TransactionStatus.Success, "Successfully Updated");
                else
                    return new TransactionResult(TransactionStatus.Success, "Successfully Added");
            }
        }

        private TransactionResult DeleteUser(Database db, DbTransaction transaction)
        {
            int returnValue = 0;
            string sqlCommand = "spDeleteUser";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, _userID);
            db.AddParameter(dbCommand, "Return Value", DbType.Int32, ParameterDirection.ReturnValue, "Return Value",
                            DataRowVersion.Default, returnValue);

            db.ExecuteNonQuery(dbCommand, transaction);
            returnValue = (Int32)db.GetParameterValue(dbCommand, "Return Value");

            if (returnValue == -1)
                return new TransactionResult(TransactionStatus.Failure, "Failure Deleting State");
            else
                return new TransactionResult(TransactionStatus.Success, "State Successfully Deleted");
        }

        public bool IsUserExists()
        {
            bool isvalid = false;

            try
            {                
                Database db = DatabaseFactory.CreateDatabase(_myConnection.DatabaseName);
                string sqlCommand = "spGetLoginUserDetail";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "UserName", DbType.String, _username);
                db.AddInParameter(dbCommand, "Password", DbType.String, _password);

                using (SqlDataReader dataReader = (SqlDataReader)db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        isvalid = true;
                        _userID = dataReader.GetInt32(dataReader.GetOrdinal("UserID"));
                        _username = dataReader.GetString(dataReader.GetOrdinal("Username"));
                        _emailID = dataReader.GetString(dataReader.GetOrdinal("EmailID"));
                        _phonenumber = dataReader.GetString(dataReader.GetOrdinal("Phonenumber"));
                        _isDelete = dataReader.GetBoolean(dataReader.GetOrdinal("IsDelete"));
                        _isEmailVerified = dataReader.GetBoolean(dataReader.GetOrdinal("IsEmailVerified"));

                    }
                }
                return isvalid;
            }
            catch (Exception ex)
            {
                ErrorLog.LogErrorMessageToDB("", "Login.cs", "GetLoginUserDetail", ex.Message.ToString(), _myConnection);
                throw;
            }
            return isvalid;
        }

        public void GetUserByUserID(int userID)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_myConnection.DatabaseName);
                string sqlCommand = "spGetUserByUserID";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "UserID", DbType.Int32, userID);

                using (SqlDataReader dataReader = (SqlDataReader)db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        _userID = dataReader.GetInt32(dataReader.GetOrdinal("UserID"));
                        _username = dataReader.GetString(dataReader.GetOrdinal("Username"));
                        _emailID = dataReader.GetString(dataReader.GetOrdinal("EmailID"));
                        _phonenumber = dataReader.GetString(dataReader.GetOrdinal("Phonenumber"));
                        _isDelete = dataReader.GetBoolean(dataReader.GetOrdinal("IsDelete"));
                        _isEmailVerified = dataReader.GetBoolean(dataReader.GetOrdinal("IsEmailVerified"));
                    }
                }                
            }
            catch (Exception ex)
            {
                ErrorLog.LogErrorMessageToDB("", "UserDL.cs", "GetUserByUserID", ex.Message.ToString(), _myConnection);
                throw;
            }
        }
    }
}

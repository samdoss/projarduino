using System;
using System.Collections.Generic;
using System.Text;

namespace Samdoss.CommonLayer
{
    #region Enum Screen Mode

    //Enums for User's Screen Mode
    public enum ScreenMode
    {
        Add = 1,
        Edit = 2,
        View = 3,
        Delete = 4,
        Exists = 5
    }

    #endregion

    #region Enum for Transactions

    // TransactionStatus defines whether the transaction was successful or failed.
    public enum TransactionStatus
    {
        Success,
        Failure
    }

    #endregion

    public enum SortDirection { Ascending = 0, Descending = 1 }

}

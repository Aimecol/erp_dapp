namespace INES.ERP.Core.Enums;

/// <summary>
/// Chart of accounts types following IFRS standards
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Assets - Resources owned by the institution
    /// </summary>
    Asset = 1,

    /// <summary>
    /// Liabilities - Obligations owed by the institution
    /// </summary>
    Liability = 2,

    /// <summary>
    /// Equity - Owner's interest in the institution
    /// </summary>
    Equity = 3,

    /// <summary>
    /// Revenue - Income earned by the institution
    /// </summary>
    Revenue = 4,

    /// <summary>
    /// Expenses - Costs incurred by the institution
    /// </summary>
    Expense = 5
}

/// <summary>
/// Detailed account categories
/// </summary>
public enum AccountCategory
{
    // Assets
    CurrentAssets = 101,
    FixedAssets = 102,
    IntangibleAssets = 103,
    
    // Liabilities
    CurrentLiabilities = 201,
    LongTermLiabilities = 202,
    
    // Equity
    Capital = 301,
    RetainedEarnings = 302,
    
    // Revenue
    StudentFees = 401,
    Grants = 402,
    OtherIncome = 403,
    
    // Expenses
    OperatingExpenses = 501,
    AdministrativeExpenses = 502,
    AcademicExpenses = 503
}

/// <summary>
/// Transaction types for double-entry bookkeeping
/// </summary>
public enum TransactionType
{
    Debit = 1,
    Credit = 2
}

/// <summary>
/// Payment methods supported by the system
/// </summary>
public enum PaymentMethod
{
    Cash = 1,
    BankTransfer = 2,
    MobileMoney = 3,
    Cheque = 4,
    CreditCard = 5,
    DebitCard = 6,
    OnlinePayment = 7
}

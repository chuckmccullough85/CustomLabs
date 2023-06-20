namespace AcmeBank.Models;

public record TransactionModel (string AcctNumber, decimal BegBalance, decimal EndBalance,
    IEnumerable<TransactionDetail> Transactions);


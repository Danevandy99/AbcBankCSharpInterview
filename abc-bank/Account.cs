﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
  public class Account
  {

    public const int CHECKING = 0;
    public const int SAVINGS = 1;
    public const int MAXI_SAVINGS = 2;

    private readonly int accountType;
    public List<Transaction> transactions;

    public Account(int accountType)
    {
      this.accountType = accountType;
      this.transactions = new List<Transaction>();
    }

    public void Deposit(double amount)
    {
      if (amount <= 0)
      {
        throw new ArgumentException("amount must be greater than zero");
      }
      else
      {
        transactions.Add(new Transaction(amount));
      }
    }

    public void Withdraw(double amount)
    {
      if (amount <= 0)
      {
        throw new ArgumentException("amount must be greater than zero");
      }
      else
      {
        transactions.Add(new Transaction(-amount));
      }
    }

    public double InterestEarned()
    {
      double amount = sumTransactions();
      switch (accountType)
      {
        case SAVINGS:
          if (amount <= 1000)
            return amount * 0.001;
          else
            return 1 + (amount - 1000) * 0.002;
        case MAXI_SAVINGS:
          DateTime mostRecentWithdrawalDate = MostRecentWidthdrawalDate();
          if (mostRecentWithdrawalDate != null && mostRecentWithdrawalDate.AddDays(10) < DateProvider.GetInstance().Now())
            return amount * 0.05;
          else
            return amount * 0.001;
        default:
          return amount * 0.001;
      }
    }

    public DateTime MostRecentWidthdrawalDate()
    {
      return this.transactions
                .OrderBy(t => t.GetTransactioDate())
                .Where(t => t.amount < 0)
                .Select(t => t.GetTransactioDate())
                .FirstOrDefault();
    }

    public double sumTransactions()
    {
      double amount = 0.0;
      foreach (Transaction t in transactions)
        amount += t.amount;
      return amount;
    }

    public int GetAccountType()
    {
      return accountType;
    }

    public void TransferToAccount(Account toAccount, double amount)
    {
      this.Withdraw(amount);
      toAccount.Deposit(amount);
    }
  }
}

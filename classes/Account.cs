/*
BSc in Science in Computing & Multimedia
OOP - BSC20921
Stage 2, Semester 1
November 2021
Continuous Assessment number 1 (Individual)

Module Title:             Object-Oriented Programming
Module Code:           	  BSC20921
Weighting:			          40%
Maximal Possible Mark:    100 marks
Submission Date:			    25/11/21 (via private repository on GitHub)

Student Name:             Mateus Fonseca Campos
Student Number:		        24088
Student Email:		        24088@student.dorset-college.ie

A full description of the project, achievements and challenges
can be found in the "readme.txt" file at the root of its directory tree.
*/

// Account:
// This class defines individual accounts for the customers in the application. It keeps record of
// the account's dynamic details (i.e., Balance and Transactions).

using System; // namespace that provides the Convert class.
using System.IO; // namespace that provides the File, StreamReader and FileNotFoundException classes.
using System.Collections.Generic; // namespace that provides the List<T> class.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  class Account
  {

    // Account's properties.
    public decimal Balance { get; set; } // stores the current balance of the account.
    public List<Transaction> Transactions { get; set; } // defines a list that keeps record of all transactions for the account.

    // Account's constructor with one parameter. It is invoked when a brand new
    // customer account is created and there is no data to be retrieved from the database.
    public Account(string fileName) {
      this.Balance = 0;
      this.Transactions = new();
      using (File.Create(fileName)) {}
    }

    // Account's constructor with two parameters. It is invoked when an existing
    // customer account is loaded from the database.
    public Account(decimal balance, string fileName) {
      this.Balance = balance;
      this.Transactions = new();
      try {
        using (StreamReader sr = new StreamReader(fileName)) {
          string line;
          while ((line = sr.ReadLine()) != null) {
            string[] transactionField = line.Split('|');
            Transaction transaction = new (Convert.ToDateTime(transactionField[0]+" "+transactionField[1]),transactionField[2],Convert.ToDecimal(transactionField[3]),Convert.ToDecimal(transactionField[4]));
            this.Transactions.Add(transaction);
          }
        }
      } catch (FileNotFoundException) {
        // do nothing. Trasanctions list will remain empty.
      }
    }

  }
}

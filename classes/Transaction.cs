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
Submission Date:			    23/11/21 (via private repository on GitHub)

Student Name:             Mateus Fonseca Campos
Student Number:		        24088
Student Email:		        24088@student.dorset-college.ie

A full description of the project, achievements and challenges
can be found in the "readme.txt" file at the root of its directory tree.
*/

// Transaction:
// This class defines individual transactions for the accounts in the application. Every time a new transaction
// is created (e.g., a deposit, a withdrawal), an object of this class is instantiated to represent
// that transaction and keep record of its details.

using System; // namespace that provides the DateTime struct.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  class Transaction
  {

    // Transaction's properties.
    public DateTime Date { get; set; } // stores both date and time when the transaction happens.
    public string Type { get; set; } // stores the type of the transaction (either deposit or withdrawal).
    public decimal Amount { get; set; } // stores the amount of money of the transaction.
    public decimal Balance { get; set; } // stores the final balance of the account after the transaction happens.

    // Transaction's constructor. It initializes all four properties of the class.
    // No overloaded constructor is provided.
    public Transaction(DateTime date, string type, decimal amount, decimal balance) {
      this.Date = date;
      this.Type = type;
      this.Amount = amount;
      this.Balance = balance;
    }

  }
}

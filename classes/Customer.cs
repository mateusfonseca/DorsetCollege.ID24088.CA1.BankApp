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

// Customer:
// This class defines the application actor of type Customer which represents a bank customer.
// As a specialized type of User, it has access to all of the base class' members, it is
// enforced the implementation of the latter's abstract members and offers methods of its own.
// It also implements the IMenu interface, which allows an Customer instance to handle an
// customer-oriented menu.

using System; // namespace that provides the Console and Environment classes.
using System.Linq; // namespace that provides the Enumerable class.
using System.Collections.Generic; // namespace that provides the List<T> class.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  class Customer : User, IMenu // this class inherits from the User class and implements the IMenu interface.
  {

    // Customer's properties.
    public string AccountNumber { get; set; } // stores the auto-generated account number of a customer account.
    public string FirstName { get; set; } // stores the first name of a customer.
    public string LastName { get; set; } // stores the last name of a customer.
    public string Email { get; set; } // stores the email address of a customer.
    public Account Savings { get; set; } // stores savings account details of a customer account.
    public Account Current { get; set; } // stores current account details of a cusomter account.

    // Customer's properties enforced by IMenu (refer to IMenu's code to understand what they mean).
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string[] Message { get; set; }
    public string[] Options { get; set; }
    public int Width { get; set; }
    public char Corner { get; set; }
    public char Col { get; set; }
    public char Row { get; set; }

    // Customer's constructor with no parameters. It is invoked when creating
    // a generic instance of Customer that works as a placeholder that will
    // eventually be assigned the content of a real identified customer.
    public Customer() {}

    // Customer's constructor with eight parameters. It initializes the
    // properties enforced by IMenu.
    public Customer(int width, string title, string subTitle, string[] message, string[] options, char corner, char col, char row) {
      this.Width = width;
      this.Title = title;
      this.SubTitle = subTitle;
      this.Message = message;
      this.Options = options;
      this.Corner = corner;
      this.Col = col;
      this.Row = row;
    }

    // Customer's constructor with three parameters. It is invoked when a brand new
    // customer account is created and there is no data to be retrieved from the database.
    public Customer(string accountNumber, string firstName, string lastName) {
      this.AccountNumber = accountNumber;
      this.FirstName = char.ToUpper(firstName[0]) + firstName.Substring(1);
      this.LastName = char.ToUpper(lastName[0]) + lastName.Substring(1);
      this.Pin = $"{this.FirstName[0]-64:D2}{this.LastName[0]-64:D2}";
    }

    // Customer's constructor with eight parameters (different signature). It is invoked when an existing
    // customer account is loaded from the database.
    public Customer(string accountNumber, string firstName, string lastName, string email, string pin, decimal savings, decimal current, TestDriver testDriver) : this(accountNumber, firstName, lastName) {
      this.Email = email;
      this.Pin = pin;
      this.Savings = new(savings, $"{testDriver.FilePathIn}{this.AccountNumber}-savings.txt");
      this.Current = new(current, $"{testDriver.FilePathIn}{this.AccountNumber}-current.txt");
    }

    // Customer's method enforced by User (refer to User's code to understand its purpose).
    public override bool logIn(TestDriver testDriver) {
      List<Customer> customers = new();
      customers = importCustomers(testDriver);
      Customer customer = new();
      customer = customerIdentification(customers, testDriver);
      if (customer is null) {
        return true;
      } else if (customer.AccountNumber == "") {
        Console.WriteLine("\nThe customer account that you are looking for could not be found in the system!");
        Console.Write("\n===> Please, press any key to return: ");
        if (!testDriver.IsTest) Console.ReadKey();
        return true;
      } else {
        if (pinAuthentication(customer, testDriver)) {
          customer.Width = this.Width;
          customer.Title = this.Title;
          customer.SubTitle = this.SubTitle;
          customer.Message = this.Message;
          customer.Options = this.Options;
          customer.Corner = this.Corner;
          customer.Col = this.Col;
          customer.Row = this.Row;
          return customer.start(customers, testDriver);
        } else {
          return false;
        }
      }
    }

    // Customer's method enforced by User (refer to User's code to understand its purpose).
    public override bool start(List<Customer> customers, TestDriver testDriver) {
      this.menuHeader();
      this.menuBody(0,0);
      do {
        this.menuHeader();
        this.menuBody(0,0);
        string operationNumber = Console.ReadLine();
        if (testDriver.IsTest) { if (operationNumber == testDriver.EndOfFile) Environment.Exit(1); }
        if (operationNumber == "1") {
          transactionHistory(testDriver);
          this.menuHeader();
          this.menuBody(1,1);
        } else if (operationNumber == "2") {
          this.menuHeader();
          this.menuBody(2,1);
          customers = deposit(customers, this, testDriver);
          this.menuHeader();
          Console.Write("\nSelect an operation:\n\n1. View transaction history\n2. Make a deposit\n3. Make a withdrawal\n4. Return to login screen\n\n0. Exit\n\n===> Enter your option (1/2/3/4/0): ");
        } else if (operationNumber == "3") {
          this.menuHeader();
          this.menuBody(3,1);
          customers = withdraw(customers, this, testDriver);
          this.menuHeader();
          Console.Write("\nSelect an operation:\n\n1. View transaction history\n2. Make a deposit\n3. Make a withdrawal\n4. Return to login screen\n\n0. Exit\n\n===> Enter your option (1/2/3/4/0): ");
        } else if (operationNumber == "4") {
          return true;
        } else if (operationNumber == "0") {
          return false;
        } else {
          Console.WriteLine("\nInvalid option! Please, try again.");
          Console.Write("\n===> Enter your option (1/2/3/4/0): ");
        }
      } while (true);
    }

    // Customer's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public string getLeftPadding(string pad, string text, int width) {
      return string.Concat(Enumerable.Repeat(pad, width/2 - text.Length/2));
    }

    // Customer's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public string getRightPadding(string pad, string text, int width, string leftPadding) {
      return string.Concat(Enumerable.Repeat(pad, width - leftPadding.Length - text.Length));
    }

    // Customer's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public void menuHeader() {
      Console.WriteLine();
      Console.Clear();
      string border = string.Concat(Enumerable.Repeat(this.Row, this.Width));
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",this.Title,this.Width)}{this.Title}{getRightPadding(" ",this.Title,this.Width,getLeftPadding(" ",this.Title,this.Width))}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",this.SubTitle,this.Width)}{this.SubTitle}{getRightPadding(" ",this.SubTitle,this.Width,getLeftPadding(" ",this.SubTitle,this.Width))}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
    }

    // Customer's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public void menuBody(int messageNo, int optionNo) {
      string border = string.Concat(Enumerable.Repeat(this.Row, this.Width));
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",this.Message[messageNo],this.Width)}{this.Message[messageNo]}{getRightPadding(" ",this.Message[messageNo],this.Width,getLeftPadding(" ",this.Message[messageNo],this.Width))}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      Console.Write(this.Options[optionNo]);
    }

    // Customer's local method that prints a formatted list with all the transactions for a specified account type.
    public void transactionHistory(TestDriver testDriver) {
      this.menuHeader();
      this.menuBody(1,1);
      string accountType = whichAccount(testDriver);
      if (accountType == "1") {
        if (this.Savings.Transactions.Count == 0) {
          Console.WriteLine("\nThere are no transactions in this account yet!");
          Console.Write("\n===> Please, press any key to return: ");
          if (!testDriver.IsTest) Console.ReadKey();
          return;
        } else {
          this.menuHeader();
          this.menuBody(1,1);
          Console.WriteLine("\nSavings Account:");
          Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,20:C}{4,20:C}", "Date", "Time", "Transaction", "Amount", "Balance");
          Console.WriteLine();
          foreach (Transaction transaction in this.Savings.Transactions) {
            Console.WriteLine("{0,-20:d}{1,-20:t}{2,-20}{3,20:C}{4,20:C}", transaction.Date, transaction.Date, transaction.Type, transaction.Amount, transaction.Balance);
          }
          Console.Write("\n===> Please, press any key to return: ");
          if (!testDriver.IsTest) Console.ReadKey();
          return;
        }
      } else if (accountType == "2") {
        if (this.Current.Transactions.Count == 0) {
          Console.WriteLine("\nThere are no transactions in this account yet!");
          Console.Write("\n===> Please, press any key to return: ");
          if (!testDriver.IsTest) Console.ReadKey();
          return;
        } else {
          this.menuHeader();
          this.menuBody(1,1);
          Console.WriteLine("\nCurrent Account:");
          Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,20:C}{4,20:C}", "Date", "Time", "Transaction", "Amount", "Balance");
          foreach (Transaction transaction in this.Current.Transactions) {
            Console.WriteLine("{0,-20:d}{1,-20:t}{2,-20}{3,20:C}{4,20:C}", transaction.Date, transaction.Date, transaction.Type, transaction.Amount, transaction.Balance);
          }
          Console.Write("\n===> Please, press any key to return: ");
          if (!testDriver.IsTest) Console.ReadKey();
          return;
        }
      } else {
        return;
      }
    }

  }
}

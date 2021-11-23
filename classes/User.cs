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

// User:
// This class defines the application actor of type User which represents any user of the application.
// It is an abstract class, which means that it cannot be directly instantiated. The purpose of this
// class is to define general properties and methods that are common to any derived class that may
// inherit from it (e.g., Employee, Customer).

using System; // namespace that provides the Console, Environment, Convert, FormatException classes, the StringComparison enum and the DateTime struct.
using System.IO; // namespace that provides the StreamReader, StreamWriter and FileNotFoundException classes.
using System.Linq; // namespace that provides the Contains method.
using System.Threading; // namespace that provides the Thread class.
using System.Collections.Generic; // namespace that provides the List<T> class.
using System.Text.RegularExpressions; // namespace that provides the Regex class.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  abstract class User // this class cannot be directly instantiated. It serves only as a blueprint for creating more specialized types of user (e.g., Employee, Customer).
  {

    // User's property.
    public string Pin { get; set; } // stores the PIN code of a user.

    // User's abstract methods. Classes derived from User are enforced to implement these methods.
    abstract public bool logIn(TestDriver testDriver); // returns true or false depending on whether the user was able to log in.
    abstract public bool start(List<Customer> customers, TestDriver testDriver); // returns true or false depending on whether the application should go back to the start.

    // User's local method. It verifies that a given PIN code is authentic by checking it
    // against what is stored for the user in the database.
    public bool pinAuthentication(User user, TestDriver testDriver) {
      for (int attempt = 3; attempt > 0; attempt--) {
        if (user is Employee) {
          Console.Write($"\n===> Enter your Employee PIN ({attempt} attempt(s) left; Enter (0) to exit): ");
        } else {
          Console.Write($"\n===> Enter your Customer PIN ({attempt} attempt(s) left; Enter (0) to exit): ");
        }
        string pin = Console.ReadLine();
        if (testDriver.IsTest) { if (pin == testDriver.EndOfFile) Environment.Exit(1); }
        if (pin == "0") {
          return false;
        } else if (pin == user.Pin) {
          Console.Write("\nCorrect PIN! Access granted! Please, wait");
          if (!testDriver.IsTest) {
            for (int i=0; i<3; i++) {
                Thread.Sleep(500);
                Console.Write(".");
                Thread.Sleep(500);
            }
          }
          return true;
        } else if (attempt > 1) {
          Console.WriteLine("\nIncorrect PIN! Please, try again.");
        }
      }
      Console.Write("\nIncorrect PIN! Access denied! Application will now be terminated");
      if (!testDriver.IsTest) {
        for (int i=0; i<3; i++) {
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
        }
      }
      return false;
    }

    // User's local method. It scans the database in search for a user whose credentials (i.e., first name,
    // last name and account number) match those given. It returns an object the represents the found customer
    // if it exists in the database and a blank customer object if not. It also returns null if the user aborts
    // the process.
    public Customer customerIdentification(List<Customer> customers, TestDriver testDriver) {
      string firstName;
      do {
        Console.Write("\n===> Enter customer's first name (Enter (0) to return to the main menu): ");
        firstName = Console.ReadLine();
        if (testDriver.IsTest) { if (firstName == testDriver.EndOfFile) Environment.Exit(1); }
        if (firstName == "0") {
          return null;
        } else if (!Regex.IsMatch(firstName, @"^[a-zA-Z]+$")) {
          Console.WriteLine("\nFirst name cannot be empty and can only contain letters! Please, try again.");
        }
      } while (!Regex.IsMatch(firstName, @"^[a-zA-Z]+$"));
      string lastName;
      do {
        Console.Write("\n===> Enter customer's last name (Enter (0) to return to the main menu): ");
        lastName = Console.ReadLine();
        if (testDriver.IsTest) { if (lastName == testDriver.EndOfFile) Environment.Exit(1); }
        if (lastName == "0") {
          return null;
        } else if (!Regex.IsMatch(lastName, @"^[a-zA-Z]+$")) {
          Console.WriteLine("\nLast name cannot be empty and can only contain letters! Please, try again.");
        }
      } while (!Regex.IsMatch(lastName, @"^[a-zA-Z]+$"));
      foreach (Customer customer in customers) {
        if (customer.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) && customer.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) {
          Console.WriteLine("\nAn account under this name has been found in the system!");
          string accountNumber;
          do {
            Console.Write("\n===> Enter customer's account number (Enter (0) to return to the main menu): ");
            accountNumber = Console.ReadLine();
            if (testDriver.IsTest) { if (accountNumber == testDriver.EndOfFile) Environment.Exit(1); }
            if (accountNumber == "0") {
              return null;
            } else if (!Regex.IsMatch(accountNumber, @"^[a-zA-Z]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}$")) {
              Console.WriteLine("\nThe account number cannot be empty and has to follow the pattern \"CC-DD-DD-DD\"! Please, try again.");
            } else if (!customer.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase)) {
              Console.WriteLine("\nThe account number entered does not match the one in the system! Please, try again.");
            }
          } while (!Regex.IsMatch(accountNumber, @"^[a-zA-Z]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}$") || !customer.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));
          return customer;
        }
      }
      Customer newCustomer = new("", firstName, lastName);
      return newCustomer;
    }

    // User's local method. It returns a string that identifies whether the user is trying to access
    // an account of type savings or current.
    public string whichAccount(TestDriver testDriver) {
      string[] options = {"1","2","0"};
      Console.Write("\n1. Savings\n2. Current\n\n0. Return to main menu\n\n==> Enter your option (1/2/0): ");
      do {
        string accountType = Console.ReadLine();
        if (testDriver.IsTest) { if (accountType == testDriver.EndOfFile) Environment.Exit(1); }
        if (options.Contains(accountType)) {
          return accountType;
        } else {
          Console.WriteLine("\nInvalid option! Please, try again.");
          Console.Write("\n==> Enter your option (1/2/0): ");
        }
      } while (true);
    }

    // User's local method. It increases the value of the Balance property for either the savings or current
    // account of a given customer. It returns an updated version of the customers list if the operation was
    // successful or an unaltered version of it in case of failure.
    public List<Customer> deposit(List<Customer> customers, Customer customer, TestDriver testDriver) {
      string accountType = whichAccount(testDriver);
      string amountString;
      decimal amount = -0.5m;
      if (accountType == "1") {
        do {
          Console.WriteLine($"\nSavings Account Balance: {customer.Savings.Balance:C}");
          Console.Write("\n===> Enter the amount you would like to deposit (Enter (0) to return to main menu): ");
          amountString = Console.ReadLine();
          if (testDriver.IsTest) { if (amountString == testDriver.EndOfFile) Environment.Exit(1); }
          try {
            amount = Convert.ToDecimal(amountString);
          } catch (FormatException) {
            Console.WriteLine("\nThis field only accepts numbers with(out) decimal places! Please, try again.");
            continue;
          }
          if (amount < 0) {
            Console.WriteLine("\nCannot deposit a negative amount! Please, try again.");
          } else if (amount != 0) {
            customer.Savings.Balance += amount;
            Transaction transaction = new(DateTime.Now, "Deposit", amount, customer.Savings.Balance);
            customer.Savings.Transactions.Add(transaction);
            saveTransactions(customer.Savings.Transactions, accountType, customer.AccountNumber, testDriver);
            Console.WriteLine("\nDeposit successful!");
            Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,20:C}{4,20:C}", "Date", "Time", "Transaction", "Amount", "Balance");
            Console.WriteLine("{0,-20:d}{1,-20:t}{2,-20}{3,20:C}{4,20:C}", transaction.Date, transaction.Date, transaction.Type, transaction.Amount, transaction.Balance);
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
            return saveCustomers(customers, testDriver);
          }
        } while (amount != 0);
        return customers;
      } else if (accountType == "2") {
        do {
          Console.WriteLine($"\nCurrent Account Balance: {customer.Current.Balance:C}");
          Console.Write("\n===> Enter the amount you would like to deposit (Enter (0) to return to main menu): ");
          amountString = Console.ReadLine();
          if (testDriver.IsTest) { if (amountString == testDriver.EndOfFile) Environment.Exit(1); }
          try {
            amount = Convert.ToDecimal(amountString);
          } catch (FormatException) {
            Console.WriteLine("\nThis field only accepts numbers with(out) decimal places! Please, try again.");
            continue;
          }
          if (amount < 0) {
            Console.WriteLine("\nCannot deposit a negative amount! Please, try again.");
          } else if (amount != 0) {
            customer.Current.Balance += amount;
            Transaction transaction = new(DateTime.Now, "Deposit", amount, customer.Current.Balance);
            customer.Current.Transactions.Add(transaction);
            saveTransactions(customer.Current.Transactions, accountType, customer.AccountNumber, testDriver);
            Console.WriteLine("\nDeposit successful!");
            Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,20:C}{4,20:C}", "Date", "Time", "Transaction", "Amount", "Balance");
            Console.WriteLine("{0,-20:d}{1,-20:t}{2,-20}{3,20:C}{4,20:C}", transaction.Date, transaction.Date, transaction.Type, transaction.Amount, transaction.Balance);
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
            return saveCustomers(customers, testDriver);
          }
        } while (amount != 0);
        return customers;
      } else {
        return customers;
      }
    }

    // User's local method. It decreases the value of the Balance property for either the savings or current
    // account of a given customer. It returns an updated version of the customers list if the operation was
    // successful or an unaltered version of it in case of failure.
    public List<Customer> withdraw(List<Customer> customers, Customer customer, TestDriver testDriver) {
      string accountType = whichAccount(testDriver);
      string amountString;
      decimal amount = -0.5m;
      if (accountType == "1") {
        if (customer.Savings.Balance == 0) {
          Console.WriteLine("\nThe selected account is empty. No funds can be withdrawn.");
          return customers;
        } else {
          do {
            Console.WriteLine($"\nSavings Account Balance: {customer.Savings.Balance:C}");
            Console.Write("\n===> Enter the amount you would like to withdraw (Enter (0) to return to main menu): ");
            amountString = Console.ReadLine();
            if (testDriver.IsTest) { if (amountString == testDriver.EndOfFile) Environment.Exit(1); }
            try {
              amount = Convert.ToDecimal(amountString);
            } catch (FormatException) {
              Console.WriteLine("\nThis field only accepts numbers with(out) decimal places! Please, try again.");
              continue;
            }
            if (amount < 0) {
              Console.WriteLine("\nCannot withdraw a negative amount! Please, try again.");
            } else if (amount > customer.Savings.Balance) {
              Console.WriteLine("\nInsufficient funds! Please, try again.");
            } else if (amount != 0) {
              customer.Savings.Balance -= amount;
              Transaction transaction = new(DateTime.Now, "Withdrawal", amount, customer.Savings.Balance);
              customer.Savings.Transactions.Add(transaction);
              saveTransactions(customer.Savings.Transactions, accountType, customer.AccountNumber, testDriver);
              Console.WriteLine("\nWithdrawal successful!");
              Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,20:C}{4,20:C}", "Date", "Time", "Transaction", "Amount", "Balance");
              Console.WriteLine("{0,-20:d}{1,-20:t}{2,-20}{3,20:C}{4,20:C}", transaction.Date, transaction.Date, transaction.Type, transaction.Amount, transaction.Balance);
              Console.Write("\n===> Please, press any key to return: ");
              if (!testDriver.IsTest) Console.ReadKey();
              return saveCustomers(customers, testDriver);
            }
          } while (amount != 0);
          return customers;
        }
      } else if (accountType == "2") {
        if (customer.Current.Balance == 0) {
          Console.WriteLine("\nThe selected account is empty. No funds can be withdrawn.");
          return customers;
        } else {
          do {
            Console.WriteLine($"\nCurrent Account Balance: {customer.Current.Balance:C}");
            Console.Write("\n===> Enter the amount you would like to withdraw (Enter (0) to return to main menu): ");
            amountString = Console.ReadLine();
            if (testDriver.IsTest) { if (amountString == testDriver.EndOfFile) Environment.Exit(1); }
            try {
              amount = Convert.ToDecimal(amountString);
            } catch (FormatException) {
              Console.WriteLine("\nThis field only accepts numbers with(out) decimal places! Please, try again.");
              continue;
            }
            if (amount < 0) {
              Console.WriteLine("\nCannot withdraw a negative amount! Please, try again.");
            } else if (amount > customer.Current.Balance) {
              Console.WriteLine("\nInsufficient funds! Please, try again.");
            } else if (amount != 0) {
              customer.Current.Balance -= amount;
              Transaction transaction = new(DateTime.Now, "Withdrawal", amount, customer.Current.Balance);
              customer.Current.Transactions.Add(transaction);
              saveTransactions(customer.Current.Transactions, accountType, customer.AccountNumber, testDriver);
              Console.WriteLine("\nWithdrawal successful!");
              Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,20:C}{4,20:C}", "Date", "Time", "Transaction", "Amount", "Balance");
              Console.WriteLine("{0,-20:d}{1,-20:t}{2,-20}{3,20:C}{4,20:C}", transaction.Date, transaction.Date, transaction.Type, transaction.Amount, transaction.Balance);
              Console.Write("\n===> Please, press any key to return: ");
              if (!testDriver.IsTest) Console.ReadKey();
              return saveCustomers(customers, testDriver);
            }
          } while (amount != 0);
          return customers;
        }
      } else {
        return customers;
      }
    }

    // User's local method. It loads all customers' data from the database and stores them
    // in a list which is returned. If the database file cannot be found, the method creates one
    // and returns an empty list.
    public List<Customer> importCustomers(TestDriver testDriver) {
      List<Customer> customers = new();
      try {
        using (StreamReader sr = new StreamReader($"{testDriver.FilePathIn}customers.txt")) {
          string line;
          while ((line = sr.ReadLine()) != null) {
            string[] customerField = line.Split('|');
            Customer customer = new (customerField[0],customerField[1],customerField[2],customerField[3],customerField[4],Convert.ToDecimal(customerField[5]),Convert.ToDecimal(customerField[6]),testDriver);
            customers.Add(customer);
          }
        }
        return customers;
      } catch (FileNotFoundException) {
        return customers;
      }
    }

    // User's local method. It writes the current state of the customers list to the database file.
    // If the file cannot be found, the method creates one. It returns the list of customers.
    public List<Customer> saveCustomers(List<Customer> customers, TestDriver testDriver) {
      for (int i=0; i<2; i++) {
        try {
          using (StreamWriter sw = new StreamWriter($"{testDriver.FilePathOut}customers.txt")) {
            foreach (Customer customer in customers) {
              sw.WriteLine($"{customer.AccountNumber}|{customer.FirstName}|{customer.LastName}|{customer.Email}|{customer.Pin}|{customer.Savings.Balance}|{customer.Current.Balance}");
            }
          }
          return customers;
        } catch (FileNotFoundException) {
          using (File.Create($"{testDriver.FilePathOut}customers.txt")) {}
        }
      }
      return customers;
    }

    // User's local method. It writes the last transaction performed in the system to the database according to
    // the account type.
    public void saveTransactions(List<Transaction> transactions, string type, string accountNumber, TestDriver testDriver) {
      type = type == "1" ? "savings" : "current";
      using (StreamWriter sw = new StreamWriter($"{testDriver.FilePathOut}{accountNumber}-{type}.txt")) {
        foreach (Transaction transaction in transactions) {
          sw.WriteLine($"{transaction.Date:d}|{transaction.Date:t}|{transaction.Type}|{transaction.Amount}|{transaction.Balance}");
        }
      }
    }

  }
}

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

// Employee:
// This class defines the application actor of type Employee which represents a bank clerk.
// As a specialized type of User, it has access to all of the base class' members, it is
// enforced the implementation of the latter's abstract members and offers methods of its own.
// It also implements the IMenu interface, which allows an Employee instance to handle an
// employee-oriented menu.

using System; // namespace that provides the Console and Environment classes.
using System.Linq; // namespace that provides the Enumerable class.
using System.Net.Mail; // namespace that provides the MailAddress class.
using System.Collections.Generic; // namespace that provides the List<T> class.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  class Employee : User, IMenu // this class inherits from the User class and implements the IMenu interface.
  {

    // Employee's properties enforced by IMenu (refer to IMenu's code to understand what they mean).
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string[] Message { get; set; }
    public string[] Options { get; set; }
    public int Width { get; set; }
    public char Corner { get; set; }
    public char Col { get; set; }
    public char Row { get; set; }

    // Employee's constructor. It initializes the Pin property (inherited from User) and
    // all the properties enforced by IMenu.
    public Employee(string pin, int width, string title, string subTitle, string[] message, string[] options, char corner, char col, char row) {
      this.Pin = pin;
      this.Width = width;
      this.Title = title;
      this.SubTitle = subTitle;
      this.Message = message;
      this.Options = options;
      this.Corner = corner;
      this.Col = col;
      this.Row = row;
    }

    // Employee's method enforced by User (refer to User's code to understand its purpose).
    public override bool logIn(TestDriver testDriver) {
      if (pinAuthentication(this, testDriver)) {
        return start(importCustomers(testDriver), testDriver);
      } else {
        return false;
      }
    }

    // Employee's method enforced by User (refer to User's code to understand its purpose).
    public override bool start(List<Customer> customers, TestDriver testDriver) {
      Customer customer = new();
      this.menuHeader();
      this.menuBody(0,0);
      do {
        string operationNumber = Console.ReadLine();
        if (testDriver.IsTest) { if (operationNumber == testDriver.EndOfFile) Environment.Exit(1); }
        if (operationNumber == "1") {
          this.menuHeader();
          this.menuBody(1,1);
          customers = createCustomer(customers, testDriver);
          this.menuHeader();
          this.menuBody(0,0);
        } else if (operationNumber == "2") {
          this.menuHeader();
          this.menuBody(2,1);
          if (customers.Count == 0) {
            Console.WriteLine("\nThere are no customers in the system at the moment!");
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
          } else {
            customers = deleteCustomer(customers, testDriver);
          }
          this.menuHeader();
          this.menuBody(0,0);
        } else if (operationNumber == "3") {
          this.menuHeader();
          this.menuBody(3,1);
          if (customers.Count == 0) {
            Console.WriteLine("\nThere are no customers in the system at the moment!");
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
          } else {
            customer = customerIdentification(customers, testDriver);
            if (customer is null) {
              // do nothing
            } else if (customer.AccountNumber == "") {
              Console.WriteLine("\nThe customer account that you are looking for could not be found in the system!");
              Console.Write("\n===> Please, press any key to return: ");
              if (!testDriver.IsTest) Console.ReadKey();
            } else {
              customers = deposit(customers, customer, testDriver);
            }
          }
          this.menuHeader();
          this.menuBody(0,0);
        } else if (operationNumber == "4") {
          this.menuHeader();
          this.menuBody(4,1);
          if (customers.Count == 0) {
            Console.WriteLine("\nThere are no customers in the system at the moment!");
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
          } else {
            customer = customerIdentification(customers, testDriver);
            if (customer is null) {
              // do nothing
            } else if (customer.AccountNumber == "") {
              Console.WriteLine("\nThe customer account that you are looking for could not be found in the system!");
              Console.Write("\n===> Please, press any key to return: ");
              if (!testDriver.IsTest) Console.ReadKey();
            } else {
              if (pinAuthentication(this, testDriver)) {
                customers = withdraw(customers, customer, testDriver);
              }
            }
          }
          this.menuHeader();
          this.menuBody(0,0);
        } else if (operationNumber == "5") {
          this.menuHeader();
          this.menuBody(5,1);
          listCustomers(customers, testDriver);
          this.menuHeader();
          this.menuBody(0,0);
        } else if (operationNumber == "6") {
          return true;
        } else if (operationNumber == "0") {
          return false;
        } else {
          Console.WriteLine("\nInvalid option. Please, try again!");
          Console.Write(this.Options[0].Substring(this.Options[0].IndexOf("\n===>")));
        }
      } while (true);
    }

    // Employee's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public string getLeftPadding(string pad, string text, int width) {
      return string.Concat(Enumerable.Repeat(pad, width/2 - text.Length/2));
    }

    // Employee's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public string getRightPadding(string pad, string text, int width, string leftPadding) {
      return string.Concat(Enumerable.Repeat(pad, width - leftPadding.Length - text.Length));
    }

    // Employee's method enforced by IMenu (refer to IMenu's code to understand its purpose).
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

    // Employee's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    public void menuBody(int messageNo, int optionNo) {
      string border = string.Concat(Enumerable.Repeat(this.Row, this.Width));
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",this.Message[messageNo],this.Width)}{this.Message[messageNo]}{getRightPadding(" ",this.Message[messageNo],this.Width,getLeftPadding(" ",this.Message[messageNo],this.Width))}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      Console.Write(this.Options[optionNo]);
    }

    // Employee's local method that triggers the instantiation of a new object of type Customer.
    // It calls the User's customerIdentification method to validate naming format and verify that
    // those credentials do not belong to a customer already present in the system.
    // It also validates email format by testing the Address property of the MailAddress class, which
    // is set to true if the address is valid and false if not.
    public List<Customer> createCustomer(List<Customer> customers, TestDriver testDriver) {
      Customer customer = new();
      customer = customerIdentification(customers, testDriver);
      if (customer == null) {
        return customers;
      } else if (customer.AccountNumber == "") {
        customer.AccountNumber = $"{customer.FirstName[0]}{customer.LastName[0]}-{customer.FirstName.Length+customer.LastName.Length:D2}-{customer.FirstName[0]-64:D2}-{customer.LastName[0]-64:D2}";
        bool test;
        do {
          Console.Write("\n===> Enter customer's email address (Enter (0) to return to the main menu): ");
          customer.Email = Console.ReadLine();
          if (testDriver.IsTest) { if (customer.Email == testDriver.EndOfFile) Environment.Exit(1); }
          try {
            MailAddress addr = new(customer.Email);
            test = addr.Address == customer.Email;
          } catch {
            test = false;
          }
          if (customer.Email == "0") {
            return customers;
          } else if (!test) {
            Console.WriteLine("\nEmail cannot be empty and has to follow the pattern \"user@server.domain\"! Please, try again.");
          }
        } while (!test);
        customer.Savings = new($"{testDriver.FilePathOut}{customer.AccountNumber}-savings.txt");
        customer.Current = new($"{testDriver.FilePathOut}{customer.AccountNumber}-current.txt");
        customers.Add(customer);
        Console.WriteLine("\nCustomer account has been created successfully!");
        Console.Write("\n===> Please, press any key to return: ");
        if (!testDriver.IsTest) Console.ReadKey();
        return saveCustomers(customers, testDriver);
      } else {
        Console.WriteLine("\nThe customer account you are trying to create already exists in the system!");
        Console.Write("\n===> Please, press any key to return: ");
        if (!testDriver.IsTest) Console.ReadKey();
        return customers;
      }
    }

    // Employee's local method that removes an identified instance of the Customer class from the
    // database. It calls the User's customerIdentification method to verify that the informed credentials
    // in fact belong to a customer currently present in the system. It also checks that both balances of
    // the customer's account are zeroed, otherwise the account cannot be deleted.
    public List<Customer> deleteCustomer(List<Customer> customers, TestDriver testDriver) {
      Customer customer = new();
      customer = customerIdentification(customers, testDriver);
      if (customer == null) {
        return customers;
      } else if (customer.AccountNumber == "") {
        Console.WriteLine("\nThe customer account that you are looking for could not be found in the system!");
        Console.Write("\n===> Please, press any key to return: ");
        if (!testDriver.IsTest) Console.ReadKey();
        return customers;
      } else {
        if (pinAuthentication(this, testDriver)) {
          if (customer.Savings.Balance == 0 && customer.Current.Balance == 0) {
            customers.Remove(customer);
            Console.WriteLine("\n\nCustomer account has been deleted successfully!");
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
            return saveCustomers(customers, testDriver);
          } else {
            Console.WriteLine("\n\nThis account's balance is not zero! Cannot delete an account with available funds!");
            Console.Write("\n===> Please, press any key to return: ");
            if (!testDriver.IsTest) Console.ReadKey();
            return customers;
          }
        } else {
          return customers;
        }
      }
    }

    // Employee's local method that prints a formatted list with all the customers currently present in the system.
    public void listCustomers(List<Customer> customers, TestDriver testDriver) {
      if (customers.Count == 0) {
        Console.WriteLine("\nThere are no customers in the system at the moment!");
        Console.Write("\n===> Please, press any key to return: ");
        if (!testDriver.IsTest) Console.ReadKey();
      } else {
        Console.WriteLine("\n{0,-20}{1,-20}{2,-20}{3,-30}{4,20:C}{5,20:C}", "Account Number", "First Name", "Last Name", "Email Address", "Savings Account", "Current Account");
        Console.WriteLine();
        customers.Sort((x, y) => x.AccountNumber.CompareTo(y.AccountNumber));
        foreach (Customer customer in customers) {
          Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-30}{4,20:C}{5,20:C}", customer.AccountNumber, customer.FirstName, customer.LastName, customer.Email, customer.Savings.Balance, customer.Current.Balance);
        }
        Console.Write("\n===> Please, press any key to return: ");
        if (!testDriver.IsTest) Console.ReadKey();
      }
    }

  }
}

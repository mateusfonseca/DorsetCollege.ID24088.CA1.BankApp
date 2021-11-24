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

// Program:
// This class defines the "driver" of the application. It houses the Main method,
// which is the entry point of the program. It also boots up basic variables and objects
// that are passed on as arguments to other classes during runtime.

using System; // namespace that provides the Console, Environment and ArgumentOutOfRangeException classes.
using System.IO; // namespace that provides the Directory class.
using System.Linq; // namespace that provides the Enumerable class and the ContainsKey method.
using System.Collections.Generic; // namespace that provides the List<T> and Dictionary<TKey,TValue> classes.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  class Program : IMenu // "driver" class that implements the IMenu interface.
  {

    // Program's properties enforced by IMenu (refer to IMenu's code to understand what they mean).
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string[] Message { get; set; }
    public string[] Options { get; set; }
    public int Width { get; set; }
    public char Corner { get; set; }
    public char Col { get; set; }
    public char Row { get; set; }

    // Program's constructor (its sole purpose is to instantiate an object that can handle a non-user oriented menu).
    public Program(int width, string title, string subTitle, string[] message, string[] options, char corner, char col, char row) {
      this.Width = width;
      this.Title = title;
      this.SubTitle = subTitle;
      this.Message = message;
      this.Options = options;
      this.Corner = corner;
      this.Col = col;
      this.Row = row;
    }

    // Program's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    // Returns a padding string generated by Enumerable.Repeat, based on a predefined width and the length of a piece of text.
    public string getLeftPadding(string pad, string text, int width) {
      return string.Concat(Enumerable.Repeat(pad, width/2 - text.Length/2));
    }

    // Program's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    // Returns a padding string generated by Enumerable.Repeat, based on a predefined width, the length of a piece of text and the padding string on the opposite side.
    public string getRightPadding(string pad, string text, int width, string leftPadding) {
      return string.Concat(Enumerable.Repeat(pad, width - leftPadding.Length - text.Length));
    }

    // Program's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    // Prints an enclosed menu header.
    public void menuHeader() {
      Console.WriteLine();
      Console.Clear();
      string border = string.Concat(Enumerable.Repeat(this.Row, this.Width));
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",this.Title,this.Width)}{this.Title}{getRightPadding(" ",this.Title,this.Width,getLeftPadding(" ",this.Title,this.Width))}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
    }

    // Program's method enforced by IMenu (refer to IMenu's code to understand its purpose).
    // Prints an enclosed menu message and a list of options.
    public void menuBody(int messageNo, int optionNo) {
      string border = string.Concat(Enumerable.Repeat(this.Row, this.Width));
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",this.Message[messageNo],this.Width)}{this.Message[messageNo]}{getRightPadding(" ",this.Message[messageNo],this.Width,getLeftPadding(" ",this.Message[messageNo],this.Width))}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      Console.Write(this.Options[optionNo]);
    }

    // Program's local method that prints an enclosed banner used both for greeting and farewell.
    // It has three sections: head sentence, description and footer.
    // Head sentence and footer are static in size and the code only centralizes them.
    // The middle section, description, may have multiple lines of text. Therefore, the code
    // provides dynamic linebreakers to prevent overflow.
    public void Banner(string headSentence, string description, string[] footer) {
      List<string> subDescription = new();
      int maxLength = this.Width - 10;
      string border = string.Concat(Enumerable.Repeat(this.Row, this.Width));
      string emptyLine = string.Concat(Enumerable.Repeat(" ", this.Width));
      Console.WriteLine($"{this.Col}{emptyLine}{this.Col}");
      Console.WriteLine($"{this.Col}{getLeftPadding(" ",headSentence,this.Width)}{headSentence}{getRightPadding(" ",headSentence,this.Width,getLeftPadding(" ",headSentence,this.Width))}{this.Col}");
      Console.WriteLine($"{this.Col}{emptyLine}{this.Col}");
      if (description.Length > maxLength) {
        for (int i=0; i<description.Length; i = i + maxLength) {
          try {
            if (description[i + maxLength] == ' ') {
              subDescription.Add(description.Substring(i, maxLength));
            } else {
              int lastWord = description.Substring(i, maxLength).LastIndexOf(" ");
              subDescription.Add(description.Substring(i, lastWord));
              i -= maxLength - lastWord;
            }
          } catch (Exception e) {
            if (e is ArgumentOutOfRangeException || e is IndexOutOfRangeException) {
              subDescription.Add(description.Substring(i));
            }
          }
        }
        foreach (string line in subDescription) {
          Console.WriteLine($"{this.Col}{getLeftPadding(" ",line,this.Width)}{line}{getRightPadding(" ",line,this.Width,getLeftPadding(" ",line,this.Width))}{this.Col}");
        }
      } else {
        Console.WriteLine($"{this.Col}{getLeftPadding(" ",description,this.Width)}{description}{getRightPadding(" ",description,this.Width,getLeftPadding(" ",description,this.Width))}{this.Col}");
      }
      Console.WriteLine($"{this.Col}{emptyLine}{this.Col}");
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
      foreach (string line in footer) {
        Console.WriteLine($"{this.Col}{getLeftPadding(" ",line,this.Width)}{line}{getRightPadding(" ",line,this.Width,getLeftPadding(" ",line,this.Width))}{this.Col}");
      }
      Console.WriteLine($"{this.Corner}{border}{this.Corner}");
    }

    // Program's Main method. This is the entry point of the whole application.
    public static void Main(string[] args)
    {
      // APPLICATION STARTS HERE

      // TEST MODE SECTION STARTS HERE
      // This application features a self-driven test mode whose resources follow:

      // Declares an object of type TestDriver (refer to TestDriver's code to understand how it works).
      TestDriver testDriver;

      // Instantiates an object of type Dictionary where, for each key-value pair, the key represents
      // a testable feature and the value represents different applicable scenarios.
      Dictionary<string, string[]> testOptions = new Dictionary<string, string[]> {
        { "deploy", new string[] {"1","2"} },
        { "create", new string[] {"1","2"} },
        { "delete", new string[] {"1","2","3"} },
        { "list", new string[] {"1","2"} },
        { "history", new string[] {"1","2"} },
        { "deposit", new string[] {"1","2","3","4"} },
        { "withdraw", new string[] {"1","2","3","4","5","6"} },
        { "custom", new string[] {"1","2","3","4"} }
      };

      // This conditional block verifies what, if anything, has been passed as argument(s) to the application.
      // If nothing, the program will not enter test mode and will execute its default routines.
      if (args.Length == 0) {
        testDriver = new("./database/");
      } else if (args.Length == 3 && args[0] == "test" && testOptions.ContainsKey(args[1]) && testOptions[args[1]].Contains(args[2])) {
        if (args[1] == "deploy") {
          if (args[2] == "2") {
            Console.WriteLine("You have chosen automated deployment with overwriting.");
            Console.WriteLine("All content in the database directory may be lost.");
            Console.WriteLine("Please, ensure that you have backed up any sensitive information before proceeding.");
            Console.WriteLine("\nAre you certain that you want to continue?");
            Console.Write("Enter \"yes\" or \"no\" (default is \"no\"): ");
            if (Console.ReadLine() != "yes") {
              Console.WriteLine("\nAutomated deployment canceled!");
              return;
            }
          }
          testDriver = new($"./test/{args[1]}/{args[2]}/input-and-output/", $"./test/{args[1]}/{args[2]}/input-and-output/", "!EOF!");
          foreach (string file in Directory.GetFiles(testDriver.FilePathIn)) {
            string fileName = file.Substring(file.LastIndexOf("/")+1);
            if (fileName != "output.txt" && fileName != "input.txt") {
              File.Delete(file);
            }
          }
        } else {
          testDriver = new($"./test/{args[1]}/{args[2]}/input/", $"./test/{args[1]}/{args[2]}/output/", "!EOF!");
        }
        Console.SetIn(testDriver.Input);
        testDriver.Output.AutoFlush = true;
        Console.SetOut(testDriver.Output);
      } else {
        Console.WriteLine("It looks like you are trying to enter test mode. Please, follow the instructions below:");
        Console.WriteLine("\nThe CLI syntax is:\n");
        Console.WriteLine("  dotnet run test {FEATURE} {SCENARIO}");
        Console.WriteLine("\nAlternatively, you can pass FEATURE and SCENARIO as the first and second arguments of the main method, respectively, through the GUI of your favorite IDE.");
        Console.WriteLine("\nWhere:\n");
        Console.WriteLine("  FEATURE     SCENARIO");
        Console.WriteLine("\n  deploy                  Automatically populates the database.");
        Console.WriteLine("                 1        - Output files are kept in the test directory only and the current database files are NOT overwritten.");
        Console.WriteLine("                 2        - Output files are copied to the database directory. Any equally-named file in the database is overwritten.");
        Console.WriteLine("\n  create                  Tests the system's functionality of opening customer accounts.");
        Console.WriteLine("                 1        - The customer does not yet exist in the database and account creation is successful.");
        Console.WriteLine("                 2        - The customer already exists in the database and account creation fails.");
        Console.WriteLine("\n  delete                  Tests the system's functionality of closing customer accounts.");
        Console.WriteLine("                 1        - The customer exists in the database and their accounts' balances are zeroed. Deletion is successful.");
        Console.WriteLine("                 2        - The customer exists in the database but their accounts' balances are NOT zeroed. Deletion fails.");
        Console.WriteLine("                 3        - The customer is not in the database and account deletion fails.");
        Console.WriteLine("\n  list                    Tests the system's functionality of listing all customer accounts' details.");
        Console.WriteLine("                 1        - There is, at least, one customer in the database. Listing is successful.");
        Console.WriteLine("                 2        - There are no customers in the database. Listing fails.");
        Console.WriteLine("\n  history                 Tests the system's functionality of displaying transaction history.");
        Console.WriteLine("                 1        - There is, at least, one transaction in the database. History view is successful.");
        Console.WriteLine("                 2        - There are no transactions in the database. History view fails.");
        Console.WriteLine("\n  deposit                 Tests the system's functionality of making deposits.");
        Console.WriteLine("                 1        - The customer exists in the database. Deposit is successful (Employee's perspective).");
        Console.WriteLine("                 2        - The customer does not exist in the database. Deposit fails (Employee's perspective).");
        Console.WriteLine("                 3        - The customer exists in the database. Deposit is successful (Customer's perspective).");
        Console.WriteLine("                 4        - The customer does not exist in the database. Deposit fails (Customer's perspective).");
        Console.WriteLine("\n  withdraw                Tests the system's functionality of making withdrawals.");
        Console.WriteLine("                 1        - The customer exists in the database and has funds. Withdrawal is successful (Employee's perspective).");
        Console.WriteLine("                 2        - The customer exists in the database but does not have funds. Withdrawal fails (Employee's perspective).");
        Console.WriteLine("                 3        - The customer does not exist in the database. Withdrawal fails (Employee's perspective).");
        Console.WriteLine("                 4        - The customer exists in the database and has funds. Withdrawal is successful (Customer's perspective).");
        Console.WriteLine("                 5        - The customer exists in the database but does not have funds. Withdrawal fails (Customer's perspective).");
        Console.WriteLine("                 6        - The customer does not exist in the database. Withdrawal fails (Customer's perspective).");
        Console.WriteLine("\n  custom                  Runs customized test scripts.");
        Console.WriteLine("                 1        - Runs customized test script #1");
        Console.WriteLine("                 2        - Runs customized test script #2");
        Console.WriteLine("                 3        - Runs customized test script #3");
        Console.WriteLine("                 4        - Runs customized test script #4");
        Console.WriteLine("\nPlease, refer to the documentation to better understand how each test mode works and how you can create your own test scripts.");
        return;
      }
      // TEST MODE SECTION ENDS HERE

      // Instantiates an object of type Program and initializes all its properties.
      Program primaryObject = new Program (
        128,
        "DORSET COLLEGE BANK APPLICATION",
        "PLACEHOLDER",
        new string[] {
          "Log In As"
        },
        new string[] {
          "\n1. Employee\n2. Customer\n\n0. Exit\n\n===> Enter your option (1/2/0): "
        },
        '¬',
        '$',
        '¬'
      );

      // Instantiates an object of type Employee and initializes all its properties (refer to Employee's code to understand how it works).
      Employee employee = new Employee (
        "A1234",
        primaryObject.Width,
        primaryObject.Title,
        "LOGGED AS EMPLOYEE",
        new string[] {
          "Choose An Option",
          "Open A New Customer Account",
          "Close A Customer Account",
          "Make A Deposit",
          "Make A Withdrawal",
          "List All Customer Accounts"
        },
        new string[] {
          "\n1. Open a new customer account\n2. Close a customer account\n3. Make a deposit\n4. Make a withdrawal\n5. List all customer accounts\n6. Return to login screen\n\n0. Exit\n\n===> Enter your option (1/2/3/4/5/6/0): ",
          ""
        },
        primaryObject.Corner,
        primaryObject.Col,
        primaryObject.Row
      );

      // Instantiates an object of type Customer and initializes only the properties that are needed
      // to handle a customer oriented menu (refer to Customer's code to understand how it works).
      Customer customer = new Customer (
        primaryObject.Width,
        primaryObject.Title,
        "LOGGED AS CUSTOMER",
        new string[] {
          "Choose An Option",
          "View Transaction History",
          "Make A Deposit",
          "Make A Withdrawal"
        },
        new string[] {
          "\n1. View transaction history\n2. Make a deposit\n3. Make a withdrawal\n4. Return to login screen\n\n0. Exit\n\n===> Enter your option (1/2/3/4/0): ",
          ""
        },
        primaryObject.Corner,
        primaryObject.Col,
        primaryObject.Row
      );

      // Declares and initializes local variables.
      string userType; // type of user to log in (either Employee or Customer).
      // The three variables below compose the banner.
      string headSentence = "Dorset College Bank Application!";
      string description = "This .NET console application, written in C#, was developed as part of the Object-Oriented Programming module's Continuous Assessment number 1 of the BSc of Science in Computing & Multimedia course at Dorset College and aims to implement some basic banking functionalities.";
      string[] footer = {"By Mateus F Campos", "Dorset College Student ID #24088", "Dublin, November of 2021"};

      // Prints greeting banner.
      primaryObject.menuHeader();
      primaryObject.Banner("Welcome to "+headSentence, description, footer);
      Console.Write("\n===> Please, press any key to start: ");
      if (!testDriver.IsTest) Console.ReadKey();

      // Prints menu.
      primaryObject.menuHeader();
      primaryObject.menuBody(0,0);

      //
      do {
        userType = Console.ReadLine();
        if (testDriver.IsTest) { if (userType == testDriver.EndOfFile) Environment.Exit(1); }
        if (userType == "1") {
          if (employee.logIn(testDriver)) {
            primaryObject.menuHeader();
            primaryObject.menuBody(0,0);
          } else {
            break;
          }
        } else if (userType == "2") {
          if (customer.logIn(testDriver)) {
            primaryObject.menuHeader();
            primaryObject.menuBody(0,0);
          } else {
            break;
          }
        } else if (userType == "0") {
          break;
        } else {
          Console.WriteLine("\nInvalid option! Please, try again.");
          Console.Write(primaryObject.Options[0].Substring(primaryObject.Options[0].IndexOf("\n===>")));
        }
      } while (true);

      // Prints farewell banner.
      primaryObject.menuHeader();
      primaryObject.Banner("Thanks for using "+headSentence, description, footer);
      Console.Write("\n===> Please, press any key to end: ");
      if (!testDriver.IsTest) {
        Console.ReadKey();
      } else {
        if (args[1] == "deploy" && args[2] == "2") {
          foreach (string file in Directory.GetFiles(testDriver.FilePathOut)) {
            string fileName = file.Substring(file.LastIndexOf("/")+1);
            if (fileName != "output.txt" && fileName != "input.txt") {
              File.Copy(file, $"./database/{fileName}", true);
            }
          }
        }
      }
      Console.Clear();

      // APPLICATION ENDS HERE
    }
  }
}

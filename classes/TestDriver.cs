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

// TestDriver:
// This class defines an "pseudo-actor" that drives the application when it is set to run in test mode. It signals all
// classes and methods in the program that they should read input data from a file instead of the standard input
// stream (usually the keyboard) and write output data to another file instead of the standard output stream
// (usually the monitor). It also provides safety by validating the input file used when in test mode, preventing
// the program from being stuck in a endless execution cycle in the occasion of a badly designed input script,
// that is, when the file does not indicate a way out.

using System.IO; // namespace that provides the StreamReader and StreamWriter classes.
using System.Collections.Generic; // namespace that provides the List<T> class.

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  public class TestDriver
  {

    // TestDriver's properties.
    public bool IsTest { get; set; } // a "flag" that signals other classes whether the application is running in test mode.
    public string FilePathIn { get; set; } // stores the file path to the directory where input files are kept.
    public string FilePathOut { get; set; } // stores the file path to the directory where output files are kept.
    public string EndOfFile { get; set; } // a "code" that marks the end of the input file used while in test mode. It prevents the program from being trapped inside of infinite loops.
    public StreamReader Input { get; set; } // defines the reading source when in test mode.
    public StreamWriter Output { get; set; } // defines the writing destination when in test mode.

    // TestDriver's constructor with one parameter. It is invoked when the program is set not to run in test mode.
    public TestDriver(string filePath) {
      this.IsTest = false;
      this.FilePathIn = filePath;
      this.FilePathOut = filePath;
    }

    // TestDriver's constructor with three parameters. It is invoked when the program is set to run in test mode.
    public TestDriver(string filePathIn, string filePathOut, string endOfFile) {
      this.IsTest = true;
      this.FilePathIn = filePathIn;
      this.FilePathOut = filePathOut;
      this.EndOfFile = endOfFile;
      this.Input = new StreamReader(validateInput($"{this.FilePathIn}input.txt", this.EndOfFile));
      this.Output = new StreamWriter($"{this.FilePathOut}output.txt");
    }

    // TestDriver's local method that validates the input file when running in test mode by making sure that
    // the file contains the EndOfFile code. If the file does not exist, the method creates one to ensure that
    // the application will not be trapped.
    private string validateInput(string input, string endOfFile) {
      List<string> lines = new();
      try {
        using (StreamReader sr = new StreamReader(input)) {
          string line;
          while ((line = sr.ReadLine()) != null) {
            lines.Add(line);
          }
        }
      } catch (FileNotFoundException e) {
        using (StreamWriter sw = new StreamWriter(input)) {
          sw.WriteLine($"{endOfFile}\n\n{e}");
        }
        return input;
      }
      if (lines[lines.Count-1] != endOfFile) {
        using (StreamWriter sw = new StreamWriter(input)) {
          lines.Add(endOfFile);
          foreach (string line in lines) {
            sw.WriteLine(line);
          }
        }
        return input;
      } else {
        return input;
      }
    }

  }
}

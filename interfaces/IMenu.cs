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

// IMenu:
// This interface defines properties and methods to be implemented by classes that wish to print a customizable
// and responsive menu, that is, a menu block that adjusts itself to the length of the content it displays.
// Please, notice that, as an interface, IMenu only declares and enforces the implementation of its members, but
// does not really define their use and behavior. It is up to the implementing classes to do so.
// The comments below should be seen as general guidelines aiming to explain the intent behind this interface
// as of its creation and not as a limitation to its use. Do get funky with it if you will!

namespace DorsetCollege.ID24088.CA1.BankApp // namespace defined by this project.
{
  interface IMenu
  {

    // IMenu's properties.
    string Title { get; set; } // defines the title to displayed on the menu header.
    string SubTitle { get; set; } // defines the subtitle to be displayed on the menu header.
    string[] Message { get; set; } // defines an array of different messages to be displayed on the menu body as needed.
    string[] Options { get; set; } // defines an array of different menu options to be displayed on the menu body as needed.
    int Width { get; set; } // defines the internal width of the menu box.
    char Corner { get; set; } // defines the character to be used as corner delimeter of the menu box.
    char Col { get; set; } // defines the character to be used as column delimeter of the menu box.
    char Row { get; set; } // defines the character to be used as row delimeter of the menu box.

    // IMenu's methods.
    void menuHeader(); // prints the header of the menu and houses the title and the subtitle.
    void menuBody(int messageNo, int optionNo); // prints the body of the menu and houses the message and the options.
    string getLeftPadding(string pad, string text, int width); // returns a padding string based on a predefined width and the length of a piece of text.
    string getRightPadding(string pad, string text, int width, string leftPadding); // returns a padding string based on a predefined width, the length of a piece of text and the padding string on the opposite side.

  }
}

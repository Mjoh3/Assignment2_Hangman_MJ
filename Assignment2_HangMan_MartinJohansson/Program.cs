using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2_HangMan_MartinJohansson
{
    class Program
    {
        //members
        static readonly string [] wordsToChooseFrom=new string[10] {
            "guld","silver","kristall","rubin","smaragd","platina","safir","diamant","brons","tenn"
        };
        static char[] mDisplayedWord;
        static string mCorrectWord;
        static StringBuilder mTriedLetters;
        static int mTriesLeft;

        //functions

        //pick a random word from wordlist by generating a random index from 0 to 9 
        static string GetRandomWord()
        {
            Random rand = new Random();
            int randomIndex = rand.Next(wordsToChooseFrom.Length);
            return wordsToChooseFrom[randomIndex];
        }

        //generate a new char array and at every line insert _ to ´the displayed letter
        static void GenerateLinesofLetters()
        {
            mDisplayedWord = new char[mCorrectWord.Length];
            for(int i=0; i < mCorrectWord.Length; i++)
            {
                mDisplayedWord[i] = '_';
            }
        }

        //check through every char in displayed word to check if there are still any _ left
        static bool DisplayWordHasNoLinesLeft()
        {
            for(int i=0; i<mDisplayedWord.Length; i++)
            {
                if (mDisplayedWord[i] == '_')
                {
                    return false;
                }
            }
            return true;
        }

        //check if letter is in the correct word by going through every letter to find matches
        static bool LetterCorrectlyGuessed(char correctLetter)
        {
            foreach (char letter in mCorrectWord) {
                if (correctLetter == letter)
                {
                    return true;
                }
      
            }
            return false;
        }
        //save input that was not correct into a stringbuilder
        static void SaveIncorrectLetter(char incorrectInputLetter)
        {
            mTriedLetters.Append(incorrectInputLetter+" ");
        }
        //resetting the tried letters to a new empty stringbuilder
        static void ResetTriedLetters()
        {
            mTriedLetters = new StringBuilder("");
        }
        //insert the correct letter to the display
        static void InsertCorrectLetter(char correctLetter)
        {
            for(int i=0; i<mCorrectWord.Length; i++)
            {
                if (correctLetter == mCorrectWord[i])
                {
                    mDisplayedWord[i] = correctLetter;
                }
            }
        }

        //input the entire guessed word into the display (logic in other functions will check if the guess was right)
        static void InsertEntireWord(string inputWord)
        {          
            mDisplayedWord = inputWord.ToCharArray();            
        }

        //input function will let the user input until they only has letters, and will not allow empty input as it would crash the program
        static string InputToLowerCase()
        {
            string inputMessage;
            bool hasNonLetters;
            do
            {
                hasNonLetters = false;
                inputMessage = Console.ReadLine();
                foreach (char c in inputMessage)
                {
                    if (!Char.IsLetter(c))
                    {
                        hasNonLetters = true;
                        Console.WriteLine("contains a non letter, try again");
                        break;
                    }                      
                }
                if (inputMessage.Length == 0)
                    Console.WriteLine("you didn´t enter anything");
                
            } while (hasNonLetters || inputMessage.Length==0);
            return inputMessage.ToLower();
        }
        static bool InputIsEntireWord(string word)
        {
            if (word.Length > 1) { return true; }
            else { return false; }
        }

        //checks to see if it finds a match in the previous inputted characters in both the successful and unsuccessful guesslists
        static bool LetterNotTriedYet(char currentLetter)
        {
            foreach(char letter in mDisplayedWord)
            {
                if (currentLetter == letter)
                {
                    return false;
                }
            }
            foreach (char letter in mTriedLetters.ToString().ToCharArray())
            {
                if (currentLetter == letter)
                {
                    return false;
                }
            }
            return true;

        }
        //main function
        static void Main(string[] args)
        {
            //a new state of the game that will set everything to starting level
            mTriesLeft = 10;
            mCorrectWord = GetRandomWord();
            GenerateLinesofLetters();
            ResetTriedLetters();
            //game continues as long as there are more than 0 tries left or the displayword has no hidden lines left
            while (DisplayWordHasNoLinesLeft() == false && mTriesLeft>0)
            {
                //show the user information about the word, tries left and correct and wrong guesses
                Console.Write("Hidden word: ");
                Console.WriteLine(mDisplayedWord[0..mDisplayedWord.Length]);
                Console.WriteLine("Tries left:" + mTriesLeft);
                Console.WriteLine("Letters not in word: " + mTriedLetters);
                Console.WriteLine("Enter a letter to guess, or try to guess the entire word, then press ENTER:");
                
                //after input from user clear screen for readability                
                string inputFromUser = InputToLowerCase();
                Console.Clear();

                //if userinput was an entire word, insert it into the display and reduce the number of tries
                if (InputIsEntireWord(inputFromUser))
                {
                    if (inputFromUser == mCorrectWord)
                    {
                        Console.WriteLine("***Correct word***");
                        InsertEntireWord(inputFromUser);
                        mTriesLeft--;
                    }
                    else
                    {
                        Console.WriteLine("***Incorrect word***");
                        mTriesLeft--;
                    }
                }

                //if user inputted only one letter it will check the letter, save the input in the correct place depending on if the input was correct or not, 
                else
                {
                    if (LetterCorrectlyGuessed(inputFromUser[0]) && LetterNotTriedYet(inputFromUser[0]))
                    {
                        Console.WriteLine("***Correct letter***");
                        InsertCorrectLetter(inputFromUser[0]);
                        mTriesLeft--;
                    }
                    else if (!LetterCorrectlyGuessed(inputFromUser[0]) && LetterNotTriedYet(inputFromUser[0]))
                    {
                        Console.WriteLine("***Incorrect letter***");
                        SaveIncorrectLetter(inputFromUser[0]);
                        mTriesLeft--;
                    }
                    //if the letter was already tried, no point reduction
                    else
                    {
                        Console.WriteLine("you already tried this letter");
                    }
                }

                
            }
            //clean the window before showing the result
            Console.Clear();
            Console.WriteLine("Correct word was: "+ mCorrectWord);
            if (mTriesLeft == 0 && DisplayWordHasNoLinesLeft() == false)
            {
                Console.WriteLine("Sorry, you didn't get the right word!");
            }
            else if(mTriesLeft>0 && DisplayWordHasNoLinesLeft() == true)
            {
                Console.WriteLine("Congratulations, you got the correct word!");
            }
            else
            {
                Console.WriteLine("Something wrong happened, not sure if you won or lost, sorry");
            }
            
            Console.ReadKey();
        }
    }
}

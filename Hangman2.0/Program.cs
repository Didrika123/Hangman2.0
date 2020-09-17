using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hangman2._0
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] words = { "sinner", "heretic", "anarchist", "devil", "hell", "demon", "cucumber", "asparagus", "soy", "tofu", "orange", "banana" }; 
            bool keepPlaying = true;
            while (keepPlaying)
            {
                bool playerWon = PlayOneRound(GetRandomWord(words), 10);

                if (playerWon)
                    Console.WriteLine("You Win!");
                else
                    Console.WriteLine("You Lose!");

                Console.WriteLine("Press Enter to play another round or write exit.");

                if (Console.ReadLine().ToLower() == "exit")
                    keepPlaying = false;
            }
        }
        static bool PlayOneRound(string secretWord, int numGuesses)
        {
            bool playerWon = false;
            StringBuilder incorrectCharGuesses = new StringBuilder();
            var secretWordProgress = InitSecretWordProgress(secretWord);

            while (numGuesses > 0 && !playerWon)
            {
                ShowGame(incorrectCharGuesses, secretWordProgress, numGuesses);
                string guess = AskUserForGuess(secretWord);
                bool alreadyGuessedThatChar = false;

                if (guess.Length == 1)
                {
                    alreadyGuessedThatChar = AlreadyGuessedThat(guess[0], incorrectCharGuesses, secretWordProgress);
                    if (!alreadyGuessedThatChar && !GuessAChar(guess[0], secretWord, secretWordProgress))
                    {
                        incorrectCharGuesses.Append(guess[0]);
                    }
                }
                else if (guess == secretWord)
                {
                    secretWordProgress = guess.ToCharArray();
                }

                if (!alreadyGuessedThatChar)
                    numGuesses--;

                playerWon = new string(secretWordProgress) == secretWord;
            }
            ShowGame(incorrectCharGuesses, secretWordProgress, numGuesses);
            return playerWon;
        }
        static char[] InitSecretWordProgress(string secretWord)
        {
            var chars = new char[secretWord.Length];
            for (int i = 0; i < secretWord.Length; i++)
                chars[i] = '_';
            return chars;
        }
        static bool AlreadyGuessedThat(char guess, StringBuilder incorrectCharGuesses, char[] secretWordProgress)
        {
            if (incorrectCharGuesses.ToString().Contains(guess))
            {
                return true;
            }
            foreach (var ch in secretWordProgress)
            {
                if(ch == guess)
                    return true;
            }
            return false;
        }
        static bool GuessAChar(char guess, string secretWord, char[] secretWordProgress)
        {
            bool correct = false;
            for (int i = 0; i < secretWordProgress.Length; i++)
            {
                if(guess == secretWord[i])
                {
                    secretWordProgress[i] = guess;
                    correct = true;
                }
            }
            return correct;
        }
        static void ShowGame(StringBuilder incorrectCharGuesses, char[] secretWordProgress, int numGuesses)
        {
            Console.Clear();
            Console.WriteLine("Hangman!\n");
            Console.WriteLine("Remaining guesses: " + numGuesses);
            Console.WriteLine("Incorrect guesses: " + incorrectCharGuesses);
            Console.WriteLine("Secret Word: " + new string(secretWordProgress));
            Console.WriteLine();
        }

        static string AskUserForGuess(string secretWord)
        {
            string guess = "";

            while (guess == "")
            {
                Console.Write("Your guess: ");
                guess = Console.ReadLine().ToLower();

                Console.ForegroundColor = ConsoleColor.Red;
                if (String.IsNullOrEmpty(guess))
                {
                    Console.WriteLine("You must enter a guess.");
                    guess = "";
                }
                else if (!ContainsOnlyLowerCaseAlphabeticalLetters(guess))
                {
                    Console.WriteLine("You must enter a guess that only contains alphabetical letters.");
                    guess = "";
                }
                else if (guess.Length != secretWord.Length && guess.Length != 1)
                {
                    Console.WriteLine("You must enter a guess of 1 letter or the same length as the secret word.");
                    guess = "";
                }
                Console.ResetColor();
            }
            return guess;
        }

        static bool ContainsOnlyLowerCaseAlphabeticalLetters(string word)
        {
            foreach (char ch in word)
            {
                if (ch < 'a' || ch > 'z')
                    return false;
            }
            return true;
        }
        static string GetRandomWord(string[] words)
        {
            Random rand = new Random();
            int index = rand.Next(0, words.Length);
            return words[index];
        }
    }
 }
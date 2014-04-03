using System;
using System.Threading;
using System.Text;
using System.Collections;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace MorseBuzzer
{
  public class Program
  {
    private static Random _rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
    private static string randCall()
    {
      // Generate a random "call sign" to play
      // US call sign rules http://wireless.fcc.gov/services/index.htm?job=call_signs_1&id=amateur
      string prefix = "aknw";
      string letter = "abcdefghijklmnopqrstuvwxyz";
      string number = "0123456789";
      StringBuilder call = new StringBuilder();
      // Decide what length of call sign to generate
      switch ((int)_rnd.Next(6))
      {
        case 0: // 1x1 call
          call.Append(prefix[_rnd.Next(prefix.Length + 1)]);
          call.Append(number[_rnd.Next(number.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          break;
        case 1: // 1x2 call
          call.Append(prefix[_rnd.Next(prefix.Length + 1)]);
          call.Append(number[_rnd.Next(number.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          break;
        case 2: // 1x3 call
          call.Append(prefix[_rnd.Next(prefix.Length + 1)]);
          call.Append(number[_rnd.Next(number.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          break;
        case 3: // 2x1 call
          call.Append(prefix[_rnd.Next(prefix.Length + 1)]);
          if (call[0].Equals("a")) // only A[A-L] are valid 2xn calls
          {
            call.Append(letter[_rnd.Next(13)]);
          }
          else
          {
            call.Append(letter[_rnd.Next(letter.Length + 1)]);
          }
          call.Append(number[_rnd.Next(number.Length + 1)]);
          call.Append(letter[_rnd.Next(letter.Length + 1)]);
          break;
        case 4: // 2x2 call
          call.Append(prefix[_rnd.Next(prefix.Length + 1)]);
          if (call[0].Equals("a"))
          {
            call.Append(letter[_rnd.Next(13)]);
          }
          else
          {
            call.Append(letter[_rnd.Next(letter.Length + 1)]);
          }
          call.Append(number[_rnd.Next(number.Length)]);
          call.Append(letter[_rnd.Next(letter.Length)]);
          call.Append(letter[_rnd.Next(letter.Length)]);
          break;
        case 5: // 2x3 call
        default:
          call.Append(prefix[_rnd.Next(prefix.Length)]);
          if (call[0].Equals("a"))
          {
            call.Append(letter[_rnd.Next(13)]);
          }
          else
          {
            call.Append(letter[_rnd.Next(letter.Length + 1)]);
          }
          call.Append(number[_rnd.Next(number.Length)]);
          call.Append(letter[_rnd.Next(letter.Length)]);
          call.Append(letter[_rnd.Next(letter.Length)]);
          call.Append(letter[_rnd.Next(letter.Length)]);
          break;
      }

      return call.ToString();
    }
    private static void buzz(PWM spkr, string a, string n)
    {
      // spkr: PWM object representing the speaker to buzz
      // a: string consisting of dots (.) and dashes (-) representing a character in Morse code
      // n: Note to play (contained in the scale hashtable
      // Most of this code is stolen^H^H^H^H^H^Htaken from Getting Started with Netduino
      // Hashtable to store the notes
      Hashtable scale = new Hashtable();

      scale.Add("c", 1915u);
      scale.Add("d", 1700u);
      scale.Add("e", 1519u);
      scale.Add("f", 1432u);
      scale.Add("g", 1275u);
      scale.Add("a", 1136u);
      scale.Add("b", 1014u);
      scale.Add("C", 956u);
      scale.Add("D", 851u);
      scale.Add("E", 758u);
      scale.Add("h", 0u);

      // Tweak the value of beatsPerMinute to change the Morse code speed
      int beatsPerMinute = 500;
      // beatTimeInMilliseconds == one dit length
      int beatTimeInMilliseconds = 60000 / beatsPerMinute;
      int pauseTimeInMilliseconds = (int)(beatTimeInMilliseconds * 0.1);
      uint noteDuration = (uint)scale[n];

      foreach (char sym in a)
      {
        switch (sym)
        {
          case '.':
            // Play a dit
            spkr.SetPulse(noteDuration * 2, noteDuration);
            Thread.Sleep(beatTimeInMilliseconds - pauseTimeInMilliseconds);

            // pause for 1/10th of a beat
            spkr.SetDutyCycle(0);
            Thread.Sleep(pauseTimeInMilliseconds);
            break;
          case '-':
            // Play a dah
            spkr.SetPulse(noteDuration * 2, noteDuration);
            Thread.Sleep(beatTimeInMilliseconds * 3 - pauseTimeInMilliseconds);

            // pause for 1/10th of a beat
            spkr.SetDutyCycle(0);
            Thread.Sleep(pauseTimeInMilliseconds);
            break;
          case ' ':
            // pause for a length of five dits
            spkr.SetDutyCycle(0);
            Thread.Sleep(beatTimeInMilliseconds * 5);
            break;
          default:
            break;
        }
      }
      // Pause for 3 dits after the character
      spkr.SetDutyCycle(0);
      Thread.Sleep(beatTimeInMilliseconds*3);
    }

    public static void Main()
    {
      // Text to Morse code conversion table
      Hashtable morse = new Hashtable();

      morse.Add('a', ".-");
      morse.Add('b', "-...");
      morse.Add('c', "-.-.");
      morse.Add('d', "-..");
      morse.Add('e', ".");
      morse.Add('f', "..-.");
      morse.Add('g', "--.");
      morse.Add('h', "....");
      morse.Add('i', "..");
      morse.Add('j', ".---");
      morse.Add('k', "-.-");
      morse.Add('l', ".-..");
      morse.Add('m', "--");
      morse.Add('n', "-.");
      morse.Add('o', "---");
      morse.Add('p', ".--.");
      morse.Add('q', "--.-");
      morse.Add('r', ".-.");
      morse.Add('s', "...");
      morse.Add('t', "-");
      morse.Add('u', "..-");
      morse.Add('v', "...-");
      morse.Add('w', ".--");
      morse.Add('x', "-..-");
      morse.Add('y', "-.--");
      morse.Add('z', "--..");
      morse.Add('0', "-----");
      morse.Add('1', ".----");
      morse.Add('2', "..---");
      morse.Add('3', "...--");
      morse.Add('4', "....-");
      morse.Add('5', ".....");
      morse.Add('6', "-....");
      morse.Add('7', "--...");
      morse.Add('8', "---..");
      morse.Add('9', "----.");
      morse.Add(' ', " ");
      morse.Add('.', "·–·–·–");
      morse.Add(',', "--..--");
      morse.Add('?', "..--..");
      morse.Add('!', "-.-.--");
      morse.Add('/', "-..-.");

      // Text to play in Morse code. Change this to whatever you want
      // string morseText = "ab4ug";
      // Play the alphabet
      // string morseText = "abcdefghijklmnopqrstuvwxyz0123456789";
      // Note to play Morse code in. See the buzz() routine for a list of available notes
      string note = "a";

      // PWM speaker = new PWM(Pins.GPIO_PIN_D5);

      while (true)
      {
        // Get a random call sign to play
        string morseText = randCall();
        foreach (char c in morseText)
        {
          // Get the Morse "song" corresponding to the current letter and send it to buzz()
          // buzz(speaker, (string)morse[c], note);
        }
        // Delay 5 seconds before repeating
        // speaker.SetDutyCycle(0);
        Thread.Sleep(5000);
      }
    }
  }
}

using System;
using System.Threading;
using System.Collections;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace MorseBuzzer
{
  public class Program
  {
    private static void buzz(PWM spkr, string a, string n)
    {
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
      int beatsPerMinute = 450;
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
            // pause for a length of one dit
            spkr.SetDutyCycle(0);
            Thread.Sleep(beatTimeInMilliseconds);
            break;
          default:
            break;
        }
      }
      // Pause for 1 dit after the character
      spkr.SetDutyCycle(0);
      Thread.Sleep(beatTimeInMilliseconds);
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
      string morseText = "ab4ug";
      // Note to play Morse code in. See the buzz() routine for a list of available notes
      string note = "a";

      PWM speaker = new PWM(Pins.GPIO_PIN_D5);

      while (true)
      {
        foreach (char c in morseText)
        {
          // Get the Morse "song" corresponding to the current letter and send it to buzz()
          buzz(speaker, (string)morse[c], note);
        }
        // Delay 5 seconds before repeating
        speaker.SetDutyCycle(0);
        Thread.Sleep(5000);
      }
    }
  }
}

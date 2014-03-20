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
    public static void Main()
    {
      // Text to Morse code conversion table
      Hashtable morse = new Hashtable();

      morse.Add('a', "a1a3");
      morse.Add('b', "a3a1a1a1");
      morse.Add('c', "a3a1a3a1");
      morse.Add('d', "a3a1a1");
      morse.Add('e', "a1");
      morse.Add('f', "a1a1a3a1");
      morse.Add('g', "a3a3a1");
      morse.Add('h', "a1a1a1a1");
      morse.Add('i', "a1a1");
      morse.Add('j', "a1a3a3a3");
      morse.Add('k', "a3a1a3");
      morse.Add('l', "a1a3a1a1");
      morse.Add('m', "a3a3");
      morse.Add('n', "a3a1");
      morse.Add('o', "a3a3a3");
      morse.Add('p', "a1a3a3a1");
      morse.Add('q', "a3a3a1a3");
      morse.Add('r', "a1a3a1");
      morse.Add('s', "a1a1a1");
      morse.Add('t', "a3");
      morse.Add('u', "a1a1a3");
      morse.Add('v', "a1a1a1a3");
      morse.Add('w', "a1a3a3");
      morse.Add('x', "a3a1a1a3");
      morse.Add('y', "a3a1a3a3");
      morse.Add('z', "a3a3a1a1");
      morse.Add('0', "a3a3a3a3a3");
      morse.Add('1', "a1a3a3a3a3");
      morse.Add('2', "a1a1a3a3a3");
      morse.Add('3', "a1a1a1a3a3");
      morse.Add('4', "a1a1a1a1a3");
      morse.Add('5', "a1a1a1a1a1");
      morse.Add('6', "a3a1a1a1a1");
      morse.Add('7', "a3a3a1a1a1");
      morse.Add('8', "a3a3a3a1a1");
      morse.Add('9', "a3a3a3a3a1");
      morse.Add(' ', " ");
      morse.Add('.', "a1a3a1a3a1a3");
      morse.Add(',', "a3a3a1a1a3a3");
      morse.Add('?', "a1a1a3a3a1a1");
      morse.Add('!', "a3a1a3a1a3a3");
      morse.Add('/', "a3a1a1a3a1");

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

      // Text to play in Morse code. Change this to whatever you want
      string morseText = "ab4ug";

      int beatsPerMinute = 500;
      int beatTimeInMilliseconds = 60000 / beatsPerMinute;
      int pauseTimeInMilliseconds = (int)(beatTimeInMilliseconds * 0.1);

      PWM speaker = new PWM(Pins.GPIO_PIN_D5);

      while (true)
      {
        foreach (char c in morseText)
        {
          // Get the Morse "song" corresponding to the current letter
          string song = (string)morse[c];
          for (int i = 0; i < song.Length; i += 2)
          {
            // Extract the note from the string
            string note = song.Substring(i, 1);
            int beatCount = int.Parse(song.Substring(i + 1, 1));

            uint noteDuration = (uint)scale[note];

            // Play the note
            speaker.SetPulse(noteDuration * 2, noteDuration);
            Thread.Sleep(beatTimeInMilliseconds * beatCount - pauseTimeInMilliseconds);

            // pause for 1/10th of a beat
            speaker.SetDutyCycle(0);
            Thread.Sleep(pauseTimeInMilliseconds);
          }
          // Pause for one beat between each character
          speaker.SetDutyCycle(0);
          Thread.Sleep(beatTimeInMilliseconds);
        }
        // Pause for five beats before repeating
        speaker.SetDutyCycle(0);
        Thread.Sleep(beatTimeInMilliseconds * 5);
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;

namespace SWP1
{
    internal class Program
    {
        static SpeechSynthesizer ss = new SpeechSynthesizer();
        static SpeechRecognitionEngine sre;
        static bool done = false;
        static void Main(string[] args)
        {
            ss.SetOutputToDefaultAudioDevice();
            ss.Speak("Witaj w moim programie");
            CultureInfo ci = new CultureInfo("pl-PL");
            sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += Sre_SpeechRecognized;
            Choices stopChoices = new Choices();
            stopChoices.Add("Stop");
            stopChoices.Add("Koniec");
            stopChoices.Add("Zatrzymaj");
            stopChoices.Add("Pomoc");
            GrammarBuilder gb_stop = new GrammarBuilder();
            gb_stop.Append(stopChoices);
            Grammar stop_gramar = new Grammar(gb_stop);
            string[] numbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Choices ch_numbers = new Choices();
            ch_numbers.Add(numbers);

            GrammarBuilder gb_calc_p = new GrammarBuilder();
            gb_calc_p.Append("Ile jest");
            gb_calc_p.Append(ch_numbers);
            gb_calc_p.Append("plus");
            gb_calc_p.Append(ch_numbers);
            Grammar g_calc_p = new Grammar(gb_calc_p);

            GrammarBuilder gb_calc_o = new GrammarBuilder();
            gb_calc_o.Append("Ile jest");
            gb_calc_o.Append(ch_numbers);
            gb_calc_o.Append("minus");
            gb_calc_o.Append(ch_numbers);
            Grammar g_calc_o = new Grammar(gb_calc_o);

            GrammarBuilder gb_calc_r = new GrammarBuilder();
            gb_calc_r.Append("Ile jest");
            gb_calc_r.Append(ch_numbers);
            gb_calc_r.Append("razy");
            gb_calc_r.Append(ch_numbers);
            Grammar g_calc_r = new Grammar(gb_calc_r);

            GrammarBuilder gb_calc_d = new GrammarBuilder();
            gb_calc_d.Append("Ile jest");
            gb_calc_d.Append(ch_numbers);
            gb_calc_d.Append("dzielone na");
            gb_calc_d.Append(ch_numbers);
            Grammar g_calc_d = new Grammar(gb_calc_d);

            sre.LoadGrammarAsync(g_calc_p);
            sre.LoadGrammarAsync(g_calc_o);
            sre.LoadGrammarAsync(g_calc_r);
            sre.LoadGrammarAsync(g_calc_d);
            sre.LoadGrammarAsync(stop_gramar);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            while(done == false) {; }
            Console.WriteLine("Kończenie programu");
            Console.ReadLine();

        }

        private static void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float conficence = e.Result.Confidence;
            Console.WriteLine("Rozpoznano: " + txt + " z pewnością: " + conficence);
            if (conficence < 0.6)
            {
                ss.Speak("Proszę powtóżyć");
            }
            else if (txt.IndexOf("Stop") >= 0)
            {
                done = true;
                ss.Speak("Do widzenia");
            }
            else if (txt.IndexOf("Ile") >= 0 && txt.IndexOf("plus") >= 0)
            {
                string[] words = txt.Split(' ');
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[4]);
                int sum = num1 + num2;
                if (sum == 0)
                    ss.Speak("Wynik to zero");
                else
                    ss.Speak("Wynik Twojego dodawania to " + sum);

            }
            else if (txt.IndexOf("Ile") >= 0 && txt.IndexOf("minus") >= 0)
            {
                string[] words = txt.Split(' ');
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[4]);
                int sum = num1 - num2;
                if (sum > 0)
                    ss.Speak("Wynik to " + sum);
                else if (sum == 0)
                    ss.Speak("Wynik to zero");
                else
                    ss.Speak("Wynik to minus " + sum);

            }
            else if (txt.IndexOf("Ile") >= 0 && txt.IndexOf("razy") >= 0)
            {
                string[] words = txt.Split(' ');
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[4]);
                int sum = num1 * num2;
                if (sum == 0)
                    ss.Speak("Wynik to zero");
                else
                    ss.Speak("Wynik  to " + sum);

            }
            else if (txt.IndexOf("Ile") >= 0 && txt.IndexOf("dzielone na") >= 0)
            {
                string[] words = txt.Split(' ');
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[5]);
                if (num2 == 0)
                {
                    ss.Speak("Nie można dzielić przez zero");
                }
                else
                {
                    int sum = num1 / num2;
                    if (sum == 0)
                        ss.Speak("Wynik to zero");
                    else
                        ss.Speak("Wynik to " + sum);
                }
            }
        }
    }
}

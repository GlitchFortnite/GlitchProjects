using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace GlitchDownloader
{
	public class Program
	{

		private static readonly string fortnite1 =
			"https://drive.google.com/file/d/1A3Y2werWd5mlbp2MlUeP8NL5sEXy8lOn/view";

		static void Main(string[] args)
		{
			Console.Title = "Glitch Downloader";

			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Glitch Fortnite Downloader");
			Console.WriteLine("Available Downloads From Chapter 1 - Chapter 2");
			Console.WriteLine("Note: Some Downloads Dont Work Due To Some Links Not Being Able To Work");
			Console.WriteLine("Glitch Downloader Created By Twisted0097\n\n");

			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine("[1] Press A To Download Fortnite 1.7.2");
			Console.WriteLine("Press ESC To Close\n");
			while (true)
			{
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo keyboardKey = Console.ReadKey(true);

					if (keyboardKey.Key == ConsoleKey.A)
					{
						Console.WriteLine("Downloading 1.7.2");

						try
						{
							Process.Start(new ProcessStartInfo
							{
								FileName = fortnite1,
								UseShellExecute = true
							});
						}
						catch (Exception ex)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Cannot Find URL: " + ex.Message);
							Console.ForegroundColor = ConsoleColor.White;
						}
					}
					else if (keyboardKey.Key == ConsoleKey.Escape)
					{
						Console.WriteLine("\nClosing Glitch Downloader\n");

						Console.Title = "Glitch Downloader [Closed]";

						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Thank You For Using The Glitch Downloader");
						break;
					}
				}

				Thread.Sleep(50);
			}
		}
	}
}

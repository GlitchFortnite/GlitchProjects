using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
namespace GlitchLauncher
{
	internal class Program
	{
		// Current launcher version
		private const string CurrentVersion = "1.0";

		// Update server URLs
		private const string VersionURL =
			"http://localhost:4444/version.txt";

		private const string DownloadURL =
			"http://localhost:4444/glitch_launcher.rar";

		// Downloaded archive filename
		private const string ArchiveName = "glitch_launcher.rar";

		// Extraction folder
		private static readonly string ExtractFolder =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NewLauncher");

		static async Task Main(string[] args)
		{
			Console.Title = "Glitch Launcher Updater";

			using HttpClient client = new HttpClient();

			try
			{
				Console.WriteLine("Checking for Glitch Launcher updates...");
				Console.WriteLine();

				// Get the latest version
				string latestVersionText =
					await client.GetStringAsync(VersionURL);

				latestVersionText = latestVersionText.Trim();

				// Validate version numbers
				if (!Version.TryParse(CurrentVersion, out Version currentVersion) ||
					!Version.TryParse(latestVersionText, out Version latestVersion))
				{
					Console.WriteLine("Error: Invalid version information.");
					return;
				}

				// Check if an update is available
				if (latestVersion > currentVersion)
				{
					Console.ForegroundColor = ConsoleColor.Green;

					Console.WriteLine(
						"New Version Available For The Glitch Launcher");

					Console.ResetColor();

					Console.WriteLine();
					Console.WriteLine("Current Version: " + CurrentVersion);
					Console.WriteLine("Latest Version: " + latestVersionText);
					Console.WriteLine();

					// Download the update
					string archivePath = Path.Combine(
						AppDomain.CurrentDomain.BaseDirectory,
						ArchiveName
					);

					Console.WriteLine("Downloading Glitch Launcher...");

					using HttpResponseMessage response =
						await client.GetAsync(DownloadURL);

					response.EnsureSuccessStatusCode();

					await using (FileStream file = new FileStream(
						archivePath,
						FileMode.Create,
						FileAccess.Write,
						FileShare.None))
					{
						await response.Content.CopyToAsync(file);
					}

					Console.WriteLine("Download completed!");
					Console.WriteLine();

					// Create NewLauncher folder
					Console.WriteLine("Creating NewLauncher folder...");

					Directory.CreateDirectory(ExtractFolder);

					Console.WriteLine("Folder created successfully!");
					Console.WriteLine();

					// Extract RAR archive
					Console.WriteLine("Extracting Glitch Launcher...");

					using (var archive = RarArchive.OpenArchive(archivePath))
					{
						archive.WriteToDirectory(
							ExtractFolder,
							new ExtractionOptions
							{
								ExtractFullPath = true,
								Overwrite = true,
								CheckCrc = true
							}
						);
					}


					Console.WriteLine("Extraction completed!");
					Console.WriteLine();

					// Display extracted files
					Console.ForegroundColor = ConsoleColor.Cyan;

					Console.WriteLine("Files Inside NewLauncher:");
					Console.WriteLine("------------------------------");

					Console.ResetColor();

					string[] files = Directory.GetFiles(
						ExtractFolder,
						"*",
						SearchOption.AllDirectories
					);

					if (files.Length == 0)
					{
						Console.WriteLine("No files were extracted.");
					}
					else
					{
						foreach (string file in files)
						{
							Console.WriteLine(
								Path.GetRelativePath(ExtractFolder, file)
							);
						}
					}

					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Glitch Launcher update is ready!");
					Console.ResetColor();

					Console.WriteLine();
					Console.WriteLine("Extracted Location:");
					Console.WriteLine(ExtractFolder);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Green;

					Console.WriteLine(
						"No Updates Available, Current Version: " +
						CurrentVersion
					);

					Console.ResetColor();
				}
			}
			catch (HttpRequestException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Connection Error: " + ex.Message);
				Console.ResetColor();
			}
			catch (InvalidOperationException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Extraction Error: " + ex.Message);
				Console.ResetColor();
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("An Error Occurred: " + ex.Message);
				Console.ResetColor();
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}

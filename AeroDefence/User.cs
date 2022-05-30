using System;

namespace Stealer
{
	// Token: 0x02000014 RID: 20
	internal class User
	{
		// Token: 0x04000054 RID: 84
		public static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		// Token: 0x04000055 RID: 85
		public static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		// Token: 0x04000056 RID: 86
		public static string tempFolder = Environment.GetEnvironmentVariable("TEMP");
	}
}

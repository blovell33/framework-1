using System.Net.Configuration;

namespace Core {
	public class MailSettings {
		public MailSettings() {
			Port = 25;
		}

		public MailSettings(SmtpSection smtp) {
			From = smtp.From;
			Host = smtp.Network.Host;
			Port = smtp.Network.Port;
			Username = smtp.Network.UserName;
			Password = smtp.Network.Password;
		}

		public virtual string From { get; set; }

		public virtual string Host { get; set; }

		public virtual int Port { get; set; }

		public virtual string Username { get; set; }

		public virtual string Password { get; set; }
	}
}
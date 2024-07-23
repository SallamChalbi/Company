using System.Threading.Tasks;

namespace Company.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		void SendAsync(string from, string recipients, string subject, string body);
		//Task SendAsync(string from, string recipients, string subject, string body);
	}
}

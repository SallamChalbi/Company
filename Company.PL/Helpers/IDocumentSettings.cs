using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Company.PL.Helpers
{
	public interface IDocumentSettings
	{
		Task<string> UploadFile(IFormFile file, string folderName);
		void DeleteFile(string fileName, string folderName);
	}
}

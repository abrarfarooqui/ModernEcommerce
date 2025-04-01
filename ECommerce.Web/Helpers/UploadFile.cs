namespace ECommerce.Web.Helpers
{
    public class UploadFile
    {
        private readonly IWebHostEnvironment _environment;
        public UploadFile(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string fileUpload(IFormFile formFile, string imageUrl)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            string imagePath = Path.Combine(_environment.WebRootPath, @"images\VillaImage");

            if (!string.IsNullOrEmpty(imageUrl))
                deleteFile(imageUrl);

            using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            formFile.CopyTo(fileStream);
            string ImageUrl = @"\images\VillaImage\" + fileName;
            return ImageUrl;
        }
        public void deleteFile(string imageUrl)
        {
            var oldImagePath = Path.Combine(_environment.WebRootPath, imageUrl.Trim('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }
    }
}

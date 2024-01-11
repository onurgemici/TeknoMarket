
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;
using Size = SixLabors.ImageSharp.Size;

namespace TeknoMarketServices
{
    public interface IFilesService
    {
        Task<string> ResizeImageAsync(Stream imageStrem, Stream watermarkImage, Size size);
        Task<string> ResizeImageAsync(Stream imageStrem, Stream watermarkImage, int width, int height);
    }


    public class FileService : IFilesService
    {
        public async Task<string> ResizeImageAsync(Stream imageStrem, Stream watermarkImage, Size size)
        {
            var image = await Image.LoadAsync(imageStrem);

            image.Mutate(p =>
            {
                p.Resize(new ResizeOptions
                {
                    Size = size,
                    Mode = ResizeMode.Pad
                });
            });
            image.Mutate(p =>
            {
                var watermark = Image.Load(watermarkImage);
                p.DrawImage(watermark, 0.3f);
            });
            return image.ToBase64String(JpegFormat.Instance);
        }

        public async Task<string> ResizeImageAsync(Stream imageStrem, Stream watermarkImage, int width, int height)
        {
            return await ResizeImageAsync(imageStrem, watermarkImage, new Size(width, height));
        }
    }
}
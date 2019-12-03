using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeathByCaptcha;

namespace CourtRooms.Helpers
{
    public static class CaptchaHelper
    {
        private static Client GetClient()
        {
            return new SocketClient(Properties.Settings.Default.DeathByCaptchaLogin, Properties.Settings.Default.DeathByCaptchaPassword);
        }

        public static Captcha Decode(Bitmap captchaImage)
        {
            var client = GetClient();

            using (var captchaStream = new MemoryStream())
            {
                captchaImage.Save(captchaStream, ImageFormat.Png);

                Captcha captcha = client.Decode(captchaStream);
                if (captcha.Solved && captcha.Correct)
                {
                    return captcha;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void Report(Captcha captcha)
        {
            var client = GetClient();
            client.Report(captcha);
        }

        public static async Task ReportAsync(Captcha captcha, CancellationToken cancellationToken)
        {
            await Task.Run(() => { Report(captcha); }, cancellationToken);
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using WebApp.Model;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace WebApp.Service
{
    /// <summary>
    /// Dienst zum Erzeugen von QR-Codes für Fahrzeuginformationen.
    /// </summary>
    public class QRCodeService
    {
        /// <summary>
        /// Generiert einen QR-Code basierend auf den Fahrzeugdaten.
        /// </summary>
        /// <param name="fahrzeugModel">Das FahrzeugModel, das die Fahrzeuginformationen enthält.</param>
        /// <returns>Der Pfad des gespeicherten QR-Code-Bildes.</returns>
        public string GenerateQrCode(FahrzeugModel fahrzeugModel)
        {
            string qrCodeText;
            if (!fahrzeugModel.Belegt)
            {
                qrCodeText = 
                    $"Kennzeichen: {fahrzeugModel.Kennzeichen}\n" +
                    $"Kilometer: {fahrzeugModel.Kilometerstand}\n" +
                    $"Leistung: {fahrzeugModel.Leistung}€\n" +
                    $"Belegt: Nein\n" +
                    $"Link zum Mieten: https://localhost:7788/Fahrzeuge/Details/{fahrzeugModel.FId}";
            }
            else
            {
                qrCodeText =
                    $"Kennzeichen: {fahrzeugModel.Kennzeichen}\n" +
                    $"Kilometer: {fahrzeugModel.Kilometerstand}\n" +
                    $"Leistung: {fahrzeugModel.Leistung}€\n" +
                    $"Belegt: Ja";
            }

            var qrCodeImage = GenerateQrCodeImage(qrCodeText);
            var imagePath = SaveQrCodeImage(qrCodeImage);
            return imagePath;
        }

        /// <summary>
        /// Erzeugt ein Bitmap-Bild eines QR-Codes basierend auf dem gegebenen Text.
        /// </summary>
        /// <param name="qrCodeText">Der Text, der im QR-Code kodiert werden soll.</param>
        /// <returns>Ein Bitmap-Bild des QR-Codes.</returns>
        private Bitmap GenerateQrCodeImage(string qrCodeText)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300
                }
            };

            var pixelData = writer.Write(qrCodeText);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                        pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return new Bitmap(ms);
                }
            }
        }

        /// <summary>
        /// Speichert das QR-Code-Bild in einem Verzeichnis und gibt den Pfad des gespeicherten Bildes zurück.
        /// </summary>
        /// <param name="qrCodeImage">Das Bitmap-Bild des QR-Codes.</param>
        /// <returns>Der Pfad des gespeicherten QR-Code-Bildes.</returns>
        private string SaveQrCodeImage(Bitmap qrCodeImage)
        {
            var fileName = $"qrcode_{DateTime.Now.Ticks}.png";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/qrcodes", fileName);
            
            // Erstellen Sie den Ordner, falls er nicht existiert
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            qrCodeImage.Save(filePath, ImageFormat.Png);
            return $"/qrcodes/{fileName}";
        }
    }
}

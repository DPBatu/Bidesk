using Bidesk.Application.Abstraction.Services.Log;

namespace Bidesk.Persistence.Services.Log
{
    public class ErrorLogService : IErrorLogService
    {
        public async Task ErrorLogger(string _errorMessage)
        {

            try
            {
                string logDirectory = AppContext.BaseDirectory + @"Logs";
                string logFilePath = Path.Combine(logDirectory, $"error_log_{DateTime.Today.ToShortDateString()}.txt");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                if (!File.Exists(logFilePath))
                {
                    using (File.Create(logFilePath)) ;
                    //File.Create(logFilePath);
                }

                await File.AppendAllTextAsync(logFilePath, _errorMessage);

            }
            catch (IOException ioEx)
            {
                // Dosya kilitliyse veya disk doluysa oluşan hataları yakalarız
                Console.WriteLine("An IO exception occurred while logging the error.");
                Console.WriteLine($"IOException: {ioEx.Message}");

                // Alternatif loglama mekanizması kullanılabilir
                await HandleLogError(ioEx);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                // Yetki problemi varsa oluşan hataları yakalarız
                Console.WriteLine("Unauthorized access exception occurred while logging the error.");
                Console.WriteLine($"UnauthorizedAccessException: {uaEx.Message}");

                // Alternatif loglama mekanizması kullanılabilir
                await HandleLogError(uaEx);
            }
            catch (Exception Ex)
            {

                await HandleLogError(Ex);
            }


        }

        public async Task HandleLogError(Exception ex)
        {
            try
            {
                string logDirectory = AppContext.BaseDirectory + @"Logs";
                string fallbackLogPath = Path.Combine(logDirectory, $"fallback_error_log_{DateTime.Today.ToShortDateString()}.txt");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                if (!File.Exists(fallbackLogPath))
                {
                    using (File.Create(fallbackLogPath)) { }
                }

                var fallbackMessage = $"[{DateTime.Now}] Fallback log: {ex.Message}{Environment.NewLine}";

                File.AppendAllText(fallbackLogPath, fallbackMessage);
            }
            catch (Exception fallbackEx)
            {
                throw fallbackEx;
            }
        }
    }
}

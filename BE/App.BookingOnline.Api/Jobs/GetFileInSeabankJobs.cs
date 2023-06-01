using App.BookingOnline.Service;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace App.BookingOnline.WebApi.Jobs
{
    public class GetFileInSeabankJobs
    {
        private readonly ITransactionService _service;
        private readonly ILogger _log;
        public GetFileInSeabankJobs(ITransactionService service, ILogger<GetFileInSeabankJobs> logger)
        {
            _service = service;
            _log = logger;
        }

        public async Task GetFileInFromSbAsync()
        {
            try
            {
                using (LogContext.PushProperty("MethodName", "GetFileInFromSbAsync"))
                {
                    _log.LogInformation("GetFileInFromSbAsync");
                }
                _service.DownloadFileInViaFtp();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "GetFileInFromSbAsync"))
                {
                    _log.LogError(e.Message);
                }
            }
        }
    }
}

using App.BookingOnline.Service;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace App.BookingOnline.WebApi.Jobs
{
    public class SendFileOutJobs
    {
        private readonly ITransactionService _service;
        private readonly ILogger _log;
        public SendFileOutJobs(ITransactionService service, ILogger<SendFileOutJobs> logger)
        {
            _service = service;
            _log = logger;
        }

        public async Task SendFileOutToSbAsync()
        {
            await _service.SendFileOutToSeabank();
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogError("HangfireSendEmailJobs");
            }
        }
    }
}

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace SpiderWin
{
    public class WinFormLogSink : ILogEventSink, IDisposable
    {
        // 用于将日志消息发送到 WinForms 应用程序的事件
        public event EventHandler<string>? LogReceived;

        private MessageTemplateTextFormatter? formatter;

        public WinFormLogSink()
        {
            this.formatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
        }

        public void Emit(LogEvent logEvent)
        {
            using var sw = new StringWriter();
            formatter!.Format(logEvent, sw);
            LogReceived?.Invoke(this, sw.ToString());
        }

        public void Dispose()
        {
            if (formatter != null)
            {
                formatter = null;
                LogReceived = null;
            }
        }
    }
}

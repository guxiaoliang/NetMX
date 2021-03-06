﻿using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public abstract class HtmlFormatterBase : MediaTypeFormatter
    {
        protected HtmlFormatterBase()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xhtml+xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override bool CanReadType(System.Type type)
        {
            return false;
        }

        public override Task WriteToStreamAsync(System.Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var streamWriter = new StreamWriter(writeStream, Encoding.UTF8);
                        streamWriter.WriteLine(
                            string.Format(
                                @"<?xml version=""1.0"" encoding=""utf-8""?>
<?xml-stylesheet type=""text/css"" href=""style.css""?>
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"">
<head>
    <title>{0}</title>
    <link type=""text/css"" href=""UI/style.css"" rel=""Stylesheet"" />
    <link type=""text/css"" href=""UI/jquery.treeview.css"" rel=""Stylesheet"" />
</head>
<body>",
                                Title));

                        WriteBody(value, streamWriter);                        
                        streamWriter.WriteLine(@"</body></html>");
                        streamWriter.Flush();
                    });
        }

        protected abstract void WriteBody(object value, TextWriter writer);
        protected abstract string Title { get; }

    }
}
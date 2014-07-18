using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Documentation
{
    internal static class DocumentationHtmlHelpers
    {
        public const string AttributeName = "helpid";

        private const string header =
            @"<script src=""http://cdn.jsdelivr.net/qtip2/2.2.0/jquery.qtip.min.js"" type=""text/javascript""></script>
   <link rel=""stylesheet"" type=""text/css"" href=""http://cdn.jsdelivr.net/qtip2/2.2.0/jquery.qtip.min.css"" />

<style>
.tooltiptext{
    display: none;
}
</style>";

        private const string footer =
            @"
  <script>
// Create the tooltips only when document ready
$(document).ready(function()
{

     // MAKE SURE YOUR SELECTOR MATCHES SOMETHING IN YOUR HTML!!!
     $('[helpId]').each(function() {
        var el = document.getElementById( $( this ).attr('helpid') );

        if( el === null )
            return true;
        
         $(this).qtip({
             content: {
                 text: el.innerHTML
             }
         });
     });
});
  </script>
";

        public static string GetAllTypesHintElements()
        {
            var types = DocumentationStore.GetAllTypesRawHelp();

            var result = new StringBuilder();

            foreach (var nameToText in types)
            {
                var rawText = nameToText.Value;

                result.AppendFormat("<div class='tooltiptext' id='{0}'>{1}</div>", GetTypeId(nameToText.Key), rawText);
            }

            return result.ToString();
        }

        public static string GetHintAttributeValue(TypeReference type)
        {
            return GetTypeId(type.FullName);
        }

        private static string GetTypeId(string typeFullName)
        {
            return string.Format("type_{0}", typeFullName.Replace('.', '_'));
        }

        public static string HtmlHeader
        {
            get
            {
                return header;
            }
        }
        public static string HtmlFooter
        {
            get
            {
                return footer;
            }
        }
    }
}

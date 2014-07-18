using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
         $(this).qtip({
             content: {
                 text: el.innerHTML
             }
         });
     });
});
  </script>
";


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

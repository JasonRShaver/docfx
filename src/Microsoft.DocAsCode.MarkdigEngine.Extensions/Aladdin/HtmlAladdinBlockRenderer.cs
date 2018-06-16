// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdigEngine.Extensions
{
    using Markdig;
    using Markdig.Renderers;
    using Markdig.Renderers.Html;

    public class HtmlAladdinBlockRenderer : HtmlObjectRenderer<AladdinBlock>
    {
        private readonly MarkdownContext _context;
        private MarkdownPipeline _pipeline;

        public HtmlAladdinBlockRenderer(MarkdownContext context, MarkdownPipeline pipeline)
        {
            _context = context;
            _pipeline = pipeline;
        }

        protected override void Write(HtmlRenderer renderer, AladdinBlock obj)
        {
            var hookId = $"{obj.Page}-aladdin-hook";
            var hookDivId = $"#{hookId}";
            var title = "Related Questions";

            var aladdinPrompt = @"

<script src='../articles/aladdin/jquery-2.2.4.min.js' type='text/javascript'></script>
<script src='../articles/aladdin/showdown.min.js' type='text/javascript'></script>
<script src='../articles/aladdin/swiper.min.js' type='text/javascript'></script>
<script src='../articles/aladdin/aladdin.js' type='text/javascript'></script>

<script>$('head').append(""<link rel='stylesheet' type='text/css' href='../articles/aladdin/aladdin.css'>"");</script>
<script>$('head').append(""<link rel='stylesheet' type='text/css' href='../articles/aladdin/swiper.min.css'>"");</script>'

<div id={hookId} class='aladdin-hook' title='{title}'>
   <div class='question-border'></div>
   <div style='display:inline-block'>
      <a class='question-button question-icon' id='{hookId}-questions-icon'>
         {title} <div><i class='ms-Icon ms-Icon--ChevronDown'></i></div>
      </a>
   </div>
   <div class='question-border'></div>
</div>

<script type='text/javascript'>
     $('#{hookId}-questions-icon').click(function() {
        var page = '{page}';
        var section = '{section}';
        var hookId = '#{hookId}';
        var chevronDown = '<div><i class=""ms-Icon ms-Icon--ChevronDown""></i></div>';
        var chevronUp = '<div><i class=""ms-Icon ms-Icon--ChevronUp""></i></div>';
        handleInteractionLinesClick(page, section, hookId, chevronUp, chevronDown);
    });
</script>
";
            //var content = "Here!";
            aladdinPrompt = aladdinPrompt.Replace("{hookId}", hookId);
            aladdinPrompt = aladdinPrompt.Replace("{title}", title);
            aladdinPrompt = aladdinPrompt.Replace("{page}", obj.Page);
            aladdinPrompt = aladdinPrompt.Replace("{section}", obj.Section);

            renderer.Write(aladdinPrompt);
        }
    }
}


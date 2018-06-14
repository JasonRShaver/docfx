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
            var hookId = $"{obj.Page}.aladdin-hook";
            var hookDivId = $"#{hookId}";
            var title = "Related Questions";
            var chevronUp = @"<div><i class='ms-Icon ms-Icon--ChevronUp'></i></div>";

            var aladdinPrompt = @"
<div id={hookId} class='aladdin-hook' title='{title}'>
   <div class='question-border'></div>
   <div style='display:inline-block'>
      <a class='question-button question-icon' id='{hookId}-questions-icon'>
         {title} <div><i class='ms-Icon ms-Icon--ChevronDown'></i></div>
      </a>
   </div>
   <div class='question-border'></div>
</div>";
            //var content = "Here!";
            aladdinPrompt = aladdinPrompt.Replace("{hookId}", hookId);
            aladdinPrompt = aladdinPrompt.Replace("{title}", title);

            renderer.Write(aladdinPrompt);
        }
    }
}

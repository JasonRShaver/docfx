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
            var hookId = $"{obj.name}-aladdin-hook";
            var aladdinPrompt = @"
<div id={hookId} class='aladdin-hook'>
    <script src='../articles/aladdin/jquery-2.2.4.min.js' type='text/javascript'></script>
    <script src='../articles/aladdin/showdown.min.js' type='text/javascript'></script>
    <script src='../articles/aladdin/aladdin.js' type='text/javascript'></script>
    <script>$('head').append(""<link rel='stylesheet' type='text/css' href='../articles/aladdin/aladdin.css'>"");</script>

    <div class='question-border'></div>    
    <div style='display:inline-block'>
        <a class='question-button question-icon' id='{hookId}-question-icon'>
            Related Questions <div><i class='ms-Icon ms-Icon--ChevronDown'></i></div></a></div>
    <div class='question-border'></div>
</div>

<script type='text/javascript'>

    // Add the click event to enable Aladdin to live
    $('#{hookId}-question-icon').click(function() {
        var readFrom = '{readFrom}';
        var name = '{name}';
        var hookId = '#{hookId}';
        handleInteractionLinesClick(name, readFrom, hookId, false);
    });
    
    // Tell Aladdin to die when you hit 'esc', you mean, mean, person, but only for search
    $('body').keyup(function (e) { if (e.keyCode == 27) { closeSearchCard(); }});

    // All telemetry event for tracking engagement rate by page (left without opening Aladdin)
    window.addEventListener('beforeunload', function (e) {loggingNavigationAwayFromPage()});
</script>";

            aladdinPrompt = aladdinPrompt.Replace("{hookId}", hookId);
            aladdinPrompt = aladdinPrompt.Replace("{readFrom}", obj.readFrom);
            aladdinPrompt = aladdinPrompt.Replace("{name}", obj.name);

            renderer.Write(aladdinPrompt);
        }
    }
}


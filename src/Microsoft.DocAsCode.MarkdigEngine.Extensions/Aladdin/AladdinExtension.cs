// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdigEngine.Extensions
{
    using Markdig;
    using Markdig.Parsers;
    using Markdig.Parsers.Inlines;
    using Markdig.Renderers;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;

    public class AladdinExtension : IMarkdownExtension
    {
        private readonly MarkdownContext _context;

        public AladdinExtension(MarkdownContext context)
        {
            _context = context;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.BlockParsers.AddIfNotAlready<AladdinBlockParser>();
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {

            var htmlRenderer = renderer as HtmlRenderer;
            if (htmlRenderer != null && !htmlRenderer.ObjectRenderers.Contains<HtmlAladdinBlockRenderer>())
            {
                htmlRenderer.ObjectRenderers.Insert(0, new HtmlAladdinBlockRenderer(_context, pipeline));
            }
        }

    }
}

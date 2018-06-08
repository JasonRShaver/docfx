// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Markdig;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;
using Newtonsoft.Json.Linq;

namespace Microsoft.Docs.Build
{
    internal delegate (string content, Document file) ResolveContent(Document relativeTo, string href);

    /// <summary>
    /// Converts markdown to html
    /// </summary>
    internal static class MarkdownUtility
    {
        // In docfx 2, a localized text is prepended to quotes beginning with
        // [!NOTE], [!TIP], [!WARNING], [!IMPORTANT], [!CAUTION].
        //
        // Docfx 2 reads localized tokens from template repo. In docfx3, build (excluding static page generation)
        // does not depend on template, thus these tokens are managed by us.
        //
        // TODO: add localized tokens
        private static readonly IReadOnlyDictionary<string, string> s_markdownTokens = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Note", "<p>Note</p>" },
            { "Tip", "<p>Tip</p>" },
            { "Warning", "<p>Warning</p>" },
            { "Important", "<p>Important</p>" },
            { "Caution", "<p>Caution</p>" },
        };

        public static (string html, MarkupResult result) Markup(string markdown, Document file, Context context, ResolveHref resolveHref, ResolveContent resolveContent)
        {
            using (InclusionContext.PushFile(file))
            {
                var metadata = new StrongBox<JObject>();
                var title = new StrongBox<string>();
                var hasHtml = new StrongBox<bool>();

                var markdownContext = new MarkdownContext(
                    s_markdownTokens,
                    context.ReportWarning,
                    context.ReportError,
                    ReadFile,
                    GetLink);

                var pipeline = new MarkdownPipelineBuilder()
                    .UseYamlFrontMatter()
                    .UseDocfxExtensions(markdownContext)
                    .UseExtractYamlHeader(context, file, metadata)
                    .UseExtractTitle(title)
                    .UseResolveHtmlLinks(markdownContext, hasHtml)
                    .Build();

                var html = Markdown.ToHtml(markdown, pipeline);

                var result = new MarkupResult
                {
                    Title = title.Value,
                    HasHtml = hasHtml.Value,
                    Metadata = metadata.Value,
                };

                return (html, result);
            }

            (string content, object file) ReadFile(string path, object relativeTo)
            {
                Debug.Assert(relativeTo is Document);

                return resolveContent((Document)relativeTo, path);
            }

            string GetLink(string path, object relativeTo)
            {
                Debug.Assert(relativeTo is Document);

                return resolveHref((Document)relativeTo, path, file);
            }
        }
    }
}
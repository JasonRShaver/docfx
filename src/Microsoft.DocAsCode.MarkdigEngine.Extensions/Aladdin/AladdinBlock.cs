// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdigEngine.Extensions
{
    using Markdig.Parsers;
    using Markdig.Syntax;

    public class AladdinBlock : LeafBlock
    {
        public string Page { get; set; }

        public string Section { get; set; }

        public string GetRawToken => $"[!aladdin[{Page}]({Section})]";

        public AladdinBlock(BlockParser parser): base(parser)
        {
        }
    }
}

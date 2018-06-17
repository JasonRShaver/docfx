// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdigEngine.Extensions
{
    using Markdig.Parsers;
    using Markdig.Syntax;

    public class AladdinBlock : LeafBlock
    {
        public string name { get; set; }

        public string readFrom { get; set; }


        public string GetRawToken => $"[!aladdin[{name}]({readFrom})]";

        public AladdinBlock(BlockParser parser): base(parser)
        {
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdigEngine.Extensions
{
    using System;
    using System.Collections.Generic;

    using Markdig.Helpers;
    using Markdig.Parsers;

    public class AladdinBlockParser : BlockParser
    {
        private const string StartString = "[!aladdin";

        public AladdinBlockParser()
        {
            OpeningCharacters = new[] { '[' };
        }

        public override BlockState TryOpen(BlockProcessor processor)
        {
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // [!aladdin[<page>](<section>)]
            var column = processor.Column;
            var line = processor.Line;
            var command = line.ToString();
            var aladdinBlock = new AladdinBlock(this);

            if (!ExtensionsHelper.MatchStart(ref line, StartString, false))
            {
                return BlockState.None;
            }
            else
            {
                if (line.CurrentChar == '+')
                {
                    line.NextChar();
                }
            }

            string page = null, section = null;

            if (!ExtensionsHelper.MatchLink(ref line, ref page, ref section))
            {
                return BlockState.None;
            }

            while (line.CurrentChar.IsSpaceOrTab()) line.NextChar();
            if (line.CurrentChar != '\0')
            {
                return BlockState.None;
            }

            aladdinBlock.Page = page;
            aladdinBlock.Section = section;
            processor.NewBlocks.Push(aladdinBlock);

            return BlockState.BreakDiscard;
        }
    }
}

﻿using System;
using System.Text.RegularExpressions;
using Markdig;
using NUglify;
using NUglify.Html;

namespace Blog.Extensions
{
   public static class ContentProcessor
    {
        public static string AddLazyLoadToImgTag(this string rawHtmlContent)
        {
            // Replace ONLY IMG tag's src to data-src
            // Otherwise embedded videos will blow up

            if (string.IsNullOrWhiteSpace(rawHtmlContent)) return rawHtmlContent;
            var imgSrcRegex = new Regex("<img.+?(src)=[\"'](.+?)[\"'].+?>");
            var newStr = imgSrcRegex.Replace(rawHtmlContent, match =>
            {
                if (!match.Value.Contains("loading"))
                {
                    return match.Value.Replace("src",
                        @"loading=""lazy"" src");
                }

                return match.Value;
            });
            return newStr;
        }

        public static string GetPostAbstract(this string rawContent, int wordCount, bool useMarkdown = true)
        {
            var plainText = useMarkdown ?
                MarkdownToContent(rawContent, MarkdownConvertType.Text) :
                RemoveTags(rawContent);

            var result = plainText.Ellipsize(wordCount);
            return result;
        }

        public static string RemoveTags(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }
            var result = Uglify.HtmlToText(html,HtmlToTextOptions.KeepStructure);

            return !result.HasErrors && !string.IsNullOrWhiteSpace(result.Code)
                ? result.Code.Trim()
                : RemoveTagsBackup(html);
        }

        public static string RemoveTagsBackup(string html)
        {
            var result = new char[html.Length];

            var cursor = 0;
            var inside = false;
            foreach (var current in result)
            {
                switch (current)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }

                if (!inside)
                {
                    result[cursor++] = current;
                }
            }

            var stringResult = new string(result, 0, cursor);
            return stringResult.Replace("&nbsp", " ");
        }

        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "\u00A0\u2026");
        }

        public static string Ellipsize(this string text, int characterCount, string ellipsis, bool wordBoundary = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            if (characterCount < 0 || text.Length <= characterCount)
                return text;

            // search beginning of word
            var backup = characterCount;
            while (characterCount > 0 && text[characterCount - 1].IsLetter())
            {
                characterCount--;
            }

            // search previous word
            while (characterCount > 0 && text[characterCount - 1].IsSpace())
            {
                characterCount--;
            }

            // if it was the last word, recover it, unless boundary is requested
            if (characterCount == 0 && !wordBoundary)
            {
                characterCount = backup;
            }

            var trimmed = text[..characterCount];
            return trimmed + ellipsis;
        }

        public static bool IsLetter(this char c)
        {
            return 'A' <= c && c <= 'Z' || 'a' <= c && c <= 'z';
        }

        public static bool IsSpace(this char c)
        {
            return c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == ' ';
        }

        public static string MarkdownToContent(this string markdown, MarkdownConvertType type, bool disableHtml = true)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UsePipeTables()
                .UseBootstrap();

            if (disableHtml)
            {
                pipeline.DisableHtml();
            }

            var result = type switch
            {
                MarkdownConvertType.None => markdown,
                MarkdownConvertType.Html => Markdown.ToHtml(markdown, pipeline.Build()),
                MarkdownConvertType.Text => Markdown.ToPlainText(markdown, pipeline.Build()),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return result;
        }

        public enum MarkdownConvertType
        {
            None = 0,
            Html = 1,
            Text = 2
        }
    }
}

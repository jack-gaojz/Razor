// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions
{
    public class RazorComponentDocumentClassifierPass : DocumentClassifierPassBase
    {
        private static readonly string RazorComponentDocumentKind = "mvc.3.0.razor-component";
        private static readonly string RazorComponentExtension = ".razor";

        protected override string DocumentKind => RazorComponentDocumentKind;

        protected override bool IsMatch(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
        {
            var source = codeDocument.Source;
            return source.FilePath.EndsWith(RazorComponentExtension, StringComparison.OrdinalIgnoreCase);
        }

        protected override void OnDocumentStructureCreated(
            RazorCodeDocument codeDocument,
            NamespaceDeclarationIntermediateNode @namespace,
            ClassDeclarationIntermediateNode @class,
            MethodDeclarationIntermediateNode method)
        {
            base.OnDocumentStructureCreated(codeDocument, @namespace, @class, method);

            @namespace.Content = "AspNetCore";

            @class.BaseType = "global::Microsoft.AspNetCore.Mvc.Razor.RazorComponent";

            var filePath = codeDocument.Source.RelativePath ?? codeDocument.Source.FilePath;
            if (string.IsNullOrEmpty(filePath))
            {
                // It's possible for a Razor document to not have a file path.
                // Eg. When we try to generate code for an in memory document like default imports.
                var checksum = BytesToString(codeDocument.Source.GetChecksum());
                @class.ClassName = $"AspNetCore_{checksum}";
            }
            else
            {
                @class.ClassName = CSharpIdentifier.SanitizeClassName(Path.GetFileNameWithoutExtension(filePath));
            }

            @class.Modifiers.Clear();
            @class.Modifiers.Add("public");
            @class.Modifiers.Add("sealed");

            method.MethodName = "BuildRenderTree";
            method.ReturnType = "void";
            method.Modifiers.Clear();
            method.Modifiers.Add("public");
            method.Modifiers.Add("override");
        }

        private static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var result = new StringBuilder(bytes.Length);
            for (var i = 0; i < bytes.Length; i++)
            {
                // The x2 format means lowercase hex, where each byte is a 2-character string.
                result.Append(bytes[i].ToString("x2"));
            }

            return result.ToString();
        }
    }
}

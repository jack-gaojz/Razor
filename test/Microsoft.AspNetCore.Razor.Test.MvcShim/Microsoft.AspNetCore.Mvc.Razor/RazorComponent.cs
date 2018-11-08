// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Mvc.Razor
{
    public abstract class RazorComponent
    {
        public abstract void BuildRenderTree();

        public void EndContext()
        {
        }

        public void BeginContext(int position, int length, bool isLiteral)
        {
        }

        public virtual void Write(object value)
        {
        }

        public virtual void WriteLiteral(object value)
        {
        }

        public virtual void BeginWriteAttribute(
            string name,
            string prefix,
            int prefixOffset,
            string suffix,
            int suffixOffset,
            int attributeValuesCount)
        {
        }

        public void WriteAttributeValue(
            string prefix,
            int prefixOffset,
            object value,
            int valueOffset,
            int valueLength,
            bool isLiteral)
        {
        }

        public virtual void EndWriteAttribute()
        {
        }

        public void AddHtmlAttributeValue(
            string prefix,
            int prefixOffset,
            object value,
            int valueOffset,
            int valueLength,
            bool isLiteral)
        {
        }
    }
}

﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = Test.Utilities.CSharpSecurityCodeFixVerifier<
    Microsoft.NetFramework.Analyzers.DoNotUseInsecureDtdProcessingAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace Microsoft.NetFramework.Analyzers.UnitTests
{
    public partial class DoNotUseInsecureDtdProcessingAnalyzerTests
    {
        private static DiagnosticResult GetCSharpResultAt(int line, int column)
            => VerifyCS.Diagnostic(DoNotUseInsecureDtdProcessingAnalyzer.RuleXmlDocumentWithNoSecureResolver).WithLocation(line, column);

        [Fact]
        public async Task XmlDocumentDefaultResolversInXmlReaderSettingsPre452ShouldGenerateDiagnostic()
        {
            await VerifyCSharpAnalyzerAsync(
                ReferenceAssemblies.NetFramework.Net451.Default,
                @"
using System;
using System.Reflection;
using System.Xml;

namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(path, settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
        }
    }
}
",
            GetCSharpResultAt(14, 31)
            );

        }

        [Fact]
        public async Task XmlDocumentDefaultResolversInXmlReaderSettingsPre452ShouldNotGenerateDiagnostic()
        {
            await VerifyCSharpAnalyzerAsync(
                ReferenceAssemblies.NetFramework.Net452.Default,
                @"
using System;
using System.Reflection;
using System.Xml;

namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(path, settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
        }
    }
}
"
            );
        }
    }
}

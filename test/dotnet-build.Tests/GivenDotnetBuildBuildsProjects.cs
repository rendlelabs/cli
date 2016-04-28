// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using FluentAssertions;
using Microsoft.DotNet.Tools.Test.Utilities;
using Xunit;

namespace Microsoft.DotNet.Tools.Builder.Tests
{
    public class GivenDotnetBuildBuildsProjects : TestBase
    {
        [Fact]
        public void It_builds_projects_with_Unicode_in_path()
        {
            var testInstance = TestAssetsManager
                .CreateTestInstance("TestAppWithUnicodéPath")
                .WithLockFiles();

            var testProjectDirectory = testInstance.TestRoot;

            var buildCommand = new BuildCommand("");
            buildCommand.WorkingDirectory = testProjectDirectory;

            buildCommand.ExecuteWithCapturedOutput()
                .Should()
                .Pass();
        }

        [Fact]
        public void It_builds_projects_with_Unicode_in_path_project_path_passed()
        {
            var testInstance = TestAssetsManager
                .CreateTestInstance("TestAppWithUnicodéPath")
                .WithLockFiles();

            var testProject = Path.Combine(testInstance.TestRoot, "project.json");

            new BuildCommand(testProject)
                .ExecuteWithCapturedOutput()
                .Should()
                .Pass();
        }

        [Fact]
        public void It_builds_projects_with_xmlDoc_and_spaces_in_the_path()
        {
            var testInstance = TestAssetsManager
                .CreateTestInstance("TestLibraryWithXmlDoc", identifier: "With Space")
                .WithLockFiles();

            testInstance.TestRoot.Should().Contain(" ");

            var output = new DirectoryInfo(Path.Combine(testInstance.TestRoot, "output"));

            new BuildCommand("", output: output.FullName, framework: DefaultLibraryFramework)
                .WithWorkingDirectory(testInstance.TestRoot)
                .ExecuteWithCapturedOutput()
                .Should()
                .Pass();

            output.Should().HaveFiles(new[]
            {
                "TestLibraryWithXmlDoc.dll",
                "TestLibraryWithXmlDoc.xml"
            });
        }
    }
}
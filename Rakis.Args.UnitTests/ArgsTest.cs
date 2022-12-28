/*
 * Copyright (c) 2021-2022. Bert Laverman
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Rakis.Logging;

namespace Rakis.Args.UnitTests
{
    [TestClass]
    public class ArgsTest
    {
        [TestMethod]
        public void TestCharOpts()
        {
            Logger.Configuration()
                .WithRootConsoleLogger().withThreshold(LogLevel.TRACE).AddToConfig()
                .Build();

            string[] args = { "-v", "-ca", "-f", "file"};

            var argParser = new ArgParser(args)
                .WithOption('x')
                .WithOption('v')
                .WithOption('c')
                .WithOption('a')
                .WithOption('f', true)
                .Parse();
            Assert.IsFalse(argParser.Has('x'), "The x-flag SHOULD NOT be found.");
            Assert.IsFalse(argParser.Has('y'), "The (unknown) y-flag SHOULD NOT be found.");
            Assert.IsTrue(argParser.Has('v'), "The v-flag SHOULD be found.");
            Assert.IsNull(argParser['v'], "The v-flag SHOULD HAVE NO argument");
            Assert.IsTrue(argParser.Has('c'), "The c-flag SHOULD be found.");
            Assert.IsNull(argParser['c'], "The c-flag SHOULD HAVE NO argument");
            Assert.IsTrue(argParser.Has('a'), "The a-flag SHOULD be found.");
            Assert.IsNull(argParser['a'], "The a-flag SHOULD HAVE NO argument");
            Assert.IsTrue(argParser.Has('f'), "The f-flag SHOULD be found.");
            Assert.IsNotNull(argParser['f'], "The f-flag SHOULD HAVE an argument");
        }

        [TestMethod]
        public void TestStringOpts()
        {
            Logger.Configuration()
                .WithRootConsoleLogger().withThreshold(LogLevel.TRACE).AddToConfig()
                .Build();

            string[] args = { "--verbose", "-ca", "--file=file" };

            var argParser = new ArgParser(args)
                .WithOption('x', "extended")
                .WithOption('v', "verbose")
                .WithOption('c', "check")
                .WithOption('a', "append")
                .WithOption('f', "file", true)
                .Parse();
            Assert.IsFalse(argParser.Has('x'), "The x-flag SHOULD NOT be found.");
            Assert.IsFalse(argParser.Has("extended"), "The extended-flag SHOULD NOT be found.");
            Assert.IsFalse(argParser.Has('y'), "The (unknown) y-flag SHOULD NOT be found.");
            Assert.IsTrue(argParser.Has('v'), "The v-flag SHOULD be found.");
            Assert.IsTrue(argParser.Has("verbose"), "The verbose-flag SHOULD be found.");
            Assert.IsNull(argParser['v'], "The v-flag SHOULD HAVE NO argument");
            Assert.IsNull(argParser["verbose"], "The verbose-flag SHOULD HAVE NO argument");
            Assert.IsTrue(argParser.Has('c'), "The c-flag SHOULD be found.");
            Assert.IsTrue(argParser.Has("check"), "The check-flag SHOULD be found.");
            Assert.IsNull(argParser['c'], "The c-flag SHOULD HAVE NO argument");
            Assert.IsNull(argParser["check"], "The check-flag SHOULD HAVE NO argument");
            Assert.IsTrue(argParser.Has('a'), "The a-flag SHOULD be found.");
            Assert.IsTrue(argParser.Has("append"), "The append-flag SHOULD be found.");
            Assert.IsNull(argParser['a'], "The a-flag SHOULD HAVE NO argument");
            Assert.IsNull(argParser["append"], "The append-flag SHOULD HAVE NO argument");
            Assert.IsTrue(argParser.Has('f'), "The f-flag SHOULD be found.");
            Assert.IsTrue(argParser.Has("file"), "The file-flag SHOULD be found.");
            Assert.IsNotNull(argParser['f'], "The f-flag SHOULD HAVE an argument");
            Assert.IsNotNull(argParser["file"], "The file-flag SHOULD HAVE an argument");
        }
    }
}
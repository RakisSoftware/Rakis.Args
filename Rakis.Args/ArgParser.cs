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
using System.Collections.Generic;

namespace Rakis.Args
{
    /**
     * <summary>This is the central parser of arguments.</summary>
     */
    public class ArgParser
    {
        private static readonly ILogger log = Logger.GetLogger(typeof(ArgParser));

        public string[] CommandLineArgs { get; init; }
        private Dictionary<char, Option> charOptions = new();
        private Dictionary<string, Option> stringOptions = new();

        /**
         * <summary>Creates a Parser/OptionBuilder, ready to process the <paramref name="args"/>.</summary>
         */
        public ArgParser(string[] args)
        {
            CommandLineArgs = args;
        }

        /**
         * <summary>Add or replace an option with both a short (<paramref name="shortOpt"/>) and a long (<paramref name="longOpt"/>) name.
         * If <paramref name="hasArg"/> is set to true (default false) the option must have an argument.</summary>
         */
        public ArgParser WithOption(char shortOpt, string longOpt, bool hasArg =false)
        {
            var o = new Option(shortOpt, longOpt, hasArg);
            if (shortOpt != '\0')
            {
                if (!charOptions.ContainsKey(shortOpt))
                {
                    log.Trace?.Log($"Adding option '{shortOpt}', HasArg={o.HasArg}");
                    charOptions.Add(shortOpt, o);
                }
                else
                {
                    charOptions[shortOpt] = o;
                }
            }
            if (longOpt != null)
            {
                if (!stringOptions.ContainsKey(longOpt))
                {
                    log.Trace?.Log($"Adding option \"{longOpt}\", HasArg={o.HasArg}");
                    stringOptions.Add(longOpt, o);
                }
                else
                {
                    stringOptions[longOpt] = o;
                }
            }
            return this;
        }

        /**
         * <summary>Add or replace an option with only a short (<paramref name="optChar"/>) name.
         * If <paramref name="hasArg"/> is set to true (default false) the option must have an argument.</summary>
         */
        public ArgParser WithOption(char optChar, bool hasArg = false)
        {
            return WithOption(optChar, null, hasArg);
        }

        /**
         * <summary>Add or replace an option with only a long (<paramref name="optString"/>) name.
         * If <paramref name="hasArg"/> is set to true (default false) the option must have an argument.</summary>
         */
        public ArgParser WithOption(string optString, bool hasArg = false)
        {
            return WithOption('\0', optString, hasArg);
        }

        /**
         * <summary>Process the given argument list (to the constructor) and return an <see cref="Args"/> object with
         * the result.</summary>
         */
        public Args Parse()
        {
            Args result = new();

            uint i = 0;
            while (i < CommandLineArgs.Length)
            {
                string thisArg = CommandLineArgs[i];
                if (!thisArg.StartsWith("-"))
                {
                    break;
                }

                log.Trace?.Log($"Parsing \"{thisArg}\"");
                if (thisArg.StartsWith("--"))
                {
                    int index = thisArg.IndexOf('=');
                    if (index == -1) index = thisArg.Length;
                    string optName = thisArg.Substring(2, index - 2);
                    string value = (index == thisArg.Length) ? null : thisArg.Substring(index + 1);

                    if (stringOptions.ContainsKey(optName))
                    {
                        var opt = stringOptions[optName];
                        if (opt.HasArg)
                        {
                            if (value == null)
                            {
                                throw new BadArgException(optName, $"Option \"{optName}\" should have a value.");
                            }
                            log.Trace?.Log($"Adding \"{opt}\" with value \"{value}\"");
                            result.SetOpt(opt, value);
                        }
                        else
                        {
                            if (value != null)
                            {
                                throw new BadArgException(optName, $"Option \"{optName}\" should not have a value in \"{thisArg}\".");
                            }
                            log.Trace?.Log($"Adding {opt}");
                            result.SetOpt(opt);
                        }
                    }
                    else
                    {
                        throw new BadArgException(optName, $"Unknown option \"{optName}\"");
                    }
                }
                else
                {
                    foreach (char c in thisArg[1..])
                    {
                        if (charOptions.ContainsKey(c))
                        {
                            var opt = charOptions[c];
                            if (opt.HasArg)
                            {
                                if (++i >= CommandLineArgs.Length)
                                {
                                    throw new BadArgException(c, $"Missing argument for '{c}'.");
                                }
                                log.Trace?.Log($"Adding '{opt}' with value \"{CommandLineArgs[i]}\"");
                                result.SetOpt(opt, CommandLineArgs[i]);
                            }
                            else
                            {
                                log.Debug?.Log($"Adding '{opt}'");
                                result.SetOpt(opt);
                            }
                        }
                        else
                        {
                            throw new BadArgException(c, $"Unknown option '{c}'");
                        }
                    }
                }
                i++;
            }
            while (i < CommandLineArgs.Length)
            {
                log.Debug?.Log($"Adding parameter \"{CommandLineArgs [i]}\".");
                result.Parameters.Add(CommandLineArgs[i++]);
            }

            return result;
        }
    }
}

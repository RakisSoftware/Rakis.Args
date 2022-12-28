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

using System.Collections.Generic;

namespace Rakis.Args
{
    /**
     * <summary>An object of this class contains the result of parsing the argument list.</summary>
     */
    public class Args
    {
        /**
         * <summary>A list of the arguments that were not part of the options.</summary>
         */
        public List<string> Parameters { get; init; } = new();

        /**
         * <summary>A <see cref="HashSet{string}"/> containing all options without an argument.</summary>
         */
        public HashSet<string> BoolOpts { get; init; } = new();

        /**
         * <summary>A <see cref="Dictionary{string, string}"/> of all options <strong>with</strong> an argument.</summary>
         */
        public Dictionary<string, string> ArgOpts { get; init; } = new();

        /**
         * <summary>Adds an option, optionally with an argument. Only the first will be added.</summary>
         */
        internal void SetOpt(string longOpt, string value =null)
        {
            if (longOpt != null)
            {
                if (value != null)
                {
                    if (!ArgOpts.ContainsKey(longOpt)) ArgOpts.Add(longOpt, value);
                }
                else if (!BoolOpts.Contains(longOpt))
                {
                    BoolOpts.Add(longOpt);
                }
            }
        }

        /**
         * <summary>Adds an option, optionally with an argument. Only the first will be added.</summary>
         */
        internal void SetOpt(char shortOpt, string value =null)
        {
            if (shortOpt != '\0')
            {
                SetOpt(shortOpt.ToString(), value);
            }
        }

        /**
         * <summary>Adds an option, optionally with an argument. Only the first will be added.</summary>
         */
        internal void SetOpt(Option opt, string value =null)
        {
            if (opt.HasArg ^ (value == null))
            {
                SetOpt(opt.ShortOpt, value);
                SetOpt(opt.LongOpt, value);
            }
        }

        /**
         * <summary>Return true if the given option was present.</summary>
         */
        public bool Has(string key) => BoolOpts.Contains(key) || ArgOpts.ContainsKey(key);

        /**
         * <summary>Return true if the given option was present.</summary>
         */
        public bool Has(char key) => BoolOpts.Contains(key.ToString()) || ArgOpts.ContainsKey(key.ToString());

        /**
         * <summary>Return the argument to option <paramref name="key"/>, or null if no argument is present.</summary>
         */
        public string this[string key] { get => ArgOpts.GetValueOrDefault(key, null); }

        /**
         * <summary>Return the argument to option <paramref name="key"/>, or null if no argument is present.</summary>
         */
        public string this[char key] { get => ArgOpts.GetValueOrDefault(key.ToString(), null); }

    }
}
